using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;
using Config;

namespace Jobs{
    [BurstCompatible]
    struct ComputeForces : IJob
    {
        [ReadOnly] public NativeArray<float3> positions;
        [ReadOnly] public NativeArray<float3> velocities;
        [ReadOnly] public NativeArray<float3> forces;
        [ReadOnly] public NativeArray<float> densities;
        [ReadOnly] public NativeArray<float> pressures;
        [ReadOnly] public NativeArray<int> ids;

        public float3 position;
        public float3 velocity;
        public float3 force;
        public float density;
        public float pressure;
        public int id;

        public NativeArray<float3> result;


        public void Execute()
        {
            float3 G = new float3(0.0f, -10.0f, 0);
            int count = positions.Length;
            float3 pressureForce = new float3(0, 0, 0);
            float3 viscocityForce = new float3(0, 0, 0);
            for(int i = 0; i < count; i++){
                if(id != ids[i]){
                    float3 line = positions[i] - position;
                    float l = getMagnitude(positions[i], position);
                    if(l < SPH.H){
                        pressureForce += (-line / l) * SPH.MASS * (pressure + pressures[i]) / (2 * density) * SPH.SPIKY_GRAD * Mathf.Pow(SPH.H - l, 3);
                        viscocityForce += SPH.VISC * SPH.MASS * (velocities[i] - velocity) / densities[i] * SPH.VISC_LAP * (SPH.H - l);
                    }
                }
            }
            float3 gravityForce = G * SPH.MASS / density;
            result[0] = pressureForce + viscocityForce + gravityForce;
        }

        private static float sqrMagnitude(float3 pi, float3 pj){return ((pi.x - pj.x) * (pi.x - pj.x)) + ((pi.y - pj.y) * (pi.y - pj.y)) + ((pi.z - pj.z) * (pi.z - pj.z));}
        private static float getMagnitude(float3 pi, float3 pj){return Mathf.Sqrt(sqrMagnitude(pi, pj));}
    }
}