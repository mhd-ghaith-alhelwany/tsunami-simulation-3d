using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Components;
using UnityEngine;
using Unity.Transforms;

public class SPHSystem : SystemBase
{
    const float H = 16;
    const float H2 = H * H;
    const float MASS = 2.5f;
    const float POLY6 = 4.0f / (Mathf.PI * (H*H*H*H*H*H*H*H));
    const float GAS_CONST = 2000.0f;
    const float REST_DENS = 300.0f;
    const float SPIKY_GRAD = -10.0f / (Mathf.PI * (H*H*H*H*H));
    const float VISC_LAP = 40.0f / (Mathf.PI * (H*H*H*H*H));
    const float VISC = 200.0f;
    const float DT = 0.0007f;

    protected override void OnUpdate()
    {
        float3 G = new float3(0.0f, -10.0f, 0);

        NativeArray<SphParticle> particles = GetEntityQuery(typeof(SphParticle)).ToComponentDataArray<SphParticle>(Allocator.TempJob);
        
        Entities.ForEach((ref SphParticle pi, ref Translation translation) => {

            //calculate density + pressure
            float density = 0;
            for(int i = 0; i < particles.Length; i++){
                SphParticle pj = particles[i];
                float l2 = getsqrMagnitude(pi.position, pj.position);
                if(l2 < H2)
                    density += MASS * POLY6 * Mathf.Pow(H2 - l2, 3);
            }
            float pressure = GAS_CONST * (density - REST_DENS);
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
                    if(l < H){
                        pressureForce += (-ij / l) * MASS * (pi.pressure + pj.pressure) / (2 * pj.density) * SPIKY_GRAD * Mathf.Pow(H - l, 3);
                        viscocityForce += VISC * MASS * (pj.velocity - pi.velocity) / pj.density * VISC_LAP * (H - l);
                    }
                }
            }
            float3 gravityForce = G * MASS / pi.density;
            pi.force = pressureForce + viscocityForce + gravityForce;

            //integrate
            pi.velocity += DT * pi.force / pi.density;
            pi.position = pi.position + (DT * pi.velocity);

            //render
            translation.Value = pi.position;

        }).WithDisposeOnCompletion(particles).Schedule();
    }

    private static float getsqrMagnitude(float3 pi, float3 pj)
    {
        return ((pi.x - pj.x) * (pi.x - pj.x)) + 
               ((pi.y - pj.y) * (pi.y - pj.y)) +
               ((pi.z - pj.z) * (pi.z - pj.z));
    }

    private static float getMagnitude(float3 pi, float3 pj)
    {
        return Mathf.Sqrt(getsqrMagnitude(pi, pj));
    }
}
