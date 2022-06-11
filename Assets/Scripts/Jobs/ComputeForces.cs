using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine;
using Config;

namespace Jobs{
    [BurstCompatible]
    struct ComputeForces : IJobParallelFor
    {
        [NativeDisableParallelForRestriction] public NativeArray<float3> positions;
        [NativeDisableParallelForRestriction] public NativeArray<float3> velocities;
        [NativeDisableParallelForRestriction] public NativeArray<float3> forces;
        [NativeDisableParallelForRestriction] public NativeArray<float> densities;
        [NativeDisableParallelForRestriction] public NativeArray<float> pressures;
        [NativeDisableParallelForRestriction] public NativeArray<int> ids;
        [ReadOnly] public NativeArray<int> neighbours;
        [ReadOnly] public NativeArray<int2> neighboursMinMax;

        public void Execute(int index)
        {
            float3 G = new float3(0.0f, -10.0f, 0);
            float3 pressureForce = new float3(0, 0, 0);
            float3 viscocityForce = new float3(0, 0, 0);
            for(int i = neighboursMinMax[index][0]; i < neighboursMinMax[index][1]; i++){
                if(ids[index] != ids[neighbours[i]]){
                    float3 line = positions[neighbours[i]] - positions[index];
                    float l = getMagnitude(positions[neighbours[i]], positions[index]);
                    if(l < SPH.H){
                        pressureForce += (-line / l) * SPH.MASS * (pressures[index] + pressures[neighbours[i]]) / (2 * densities[index]) * SPH.SPIKY_GRAD * Mathf.Pow(SPH.H - l, 3);
                        viscocityForce += SPH.VISC * SPH.MASS * (velocities[neighbours[i]] - velocities[index]) / densities[neighbours[i]] * SPH.VISC_LAP * (SPH.H - l);
                    }
                }
            }
            float3 gravityForce = G * SPH.MASS / densities[index];
            forces[index] = pressureForce + viscocityForce + gravityForce;
        }

        private static float sqrMagnitude(float3 pi, float3 pj){return ((pi.x - pj.x) * (pi.x - pj.x)) + ((pi.y - pj.y) * (pi.y - pj.y)) + ((pi.z - pj.z) * (pi.z - pj.z));}
        private static float getMagnitude(float3 pi, float3 pj){return Mathf.Sqrt(sqrMagnitude(pi, pj));}
    }
}