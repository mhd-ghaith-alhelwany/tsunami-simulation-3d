using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Components;
using UnityEngine;
using Unity.Transforms;
using Main;

public class SPHSystem : SystemBase
{

    protected override void OnUpdate()
    {
        float3 G = new float3(0.0f, -10.0f, 0);
        
        NativeArray<SphParticle> particles = GetEntityQuery(typeof(SphParticle)).ToComponentDataArray<SphParticle>(Allocator.TempJob);
        
        Entities.ForEach((ref SphParticle pi, ref Translation translation) => {

            // //calculate density + pressure
            float density = 0;
            for(int i = 0; i < particles.Length; i++){
                SphParticle pj = particles[i];
                float l2 = sqrMagnitude(pi.position, pj.position);
                if(l2 < Config.H2)
                    density += Config.MASS * Config.POLY6 * (Config.H2 - l2) * (Config.H2 - l2) * (Config.H2 - l2);
            }
            float pressure = Config.GAS_CONST * (density - Config.REST_DENS);
            pi.density = density;
            pi.pressure = pressure;

            //calculate forces
            float3 pressureForce = new float3(0, 0, 0);
            float3 viscocityForce = new float3(0, 0, 0);
            for(int i = 0; i < particles.Length; i++){
                SphParticle pj = particles[i];
                if(pi.particleId != pj.particleId){
                    float3 ij = pj.position - pi.position;
                    float l = getMagnitude(pi.position, pj.position);
                    if(l < Config.H){
                        pressureForce += (-ij / l) * Config.MASS * (pi.pressure + pj.pressure) / (2 * pj.density) * Config.SPIKY_GRAD * Mathf.Pow(Config.H - l, 3);
                        viscocityForce += Config.VISC * Config.MASS * (pj.velocity - pi.velocity) / pj.density * Config.VISC_LAP * (Config.H - l);
                    }
                }
            }
            float3 gravityForce = G * Config.MASS / pi.density;
            pi.force = pressureForce + viscocityForce + gravityForce;

            //integrate
            pi.velocity += Config.DT * pi.force / pi.density;
            pi.position = pi.position + (Config.DT * pi.velocity);

            //render
            translation.Value = pi.position;

        }).WithReadOnly(particles).WithDisposeOnCompletion(particles).ScheduleParallel();
    }

    private static float sqrMagnitude(float3 pi, float3 pj)
    {
        return ((pi.x - pj.x) * (pi.x - pj.x)) + 
               ((pi.y - pj.y) * (pi.y - pj.y)) +
               ((pi.z - pj.z) * (pi.z - pj.z));
    }

    private static float getMagnitude(float3 pi, float3 pj)
    {
        return Mathf.Sqrt(sqrMagnitude(pi, pj));
    }
}
