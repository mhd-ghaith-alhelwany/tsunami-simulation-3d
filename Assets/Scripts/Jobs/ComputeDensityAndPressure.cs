using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using Config;

namespace Jobs{
    [BurstCompatible]
    struct ComputeDensityAndPressure : IJobParallelFor
    {
        [NativeDisableParallelForRestriction] public NativeArray<float3> positions;
        [NativeDisableParallelForRestriction] public NativeArray<float> densities;
        [NativeDisableParallelForRestriction] public NativeArray<float> pressures;

        public void Execute(int index)
        {
            int count = positions.Length;
            densities[index] = 0;
            for(int i = 0; i < count; i++){
                float l2 = sqrMagnitude(positions[index], positions[i]);
                if(l2 < SPH.H2)
                    densities[index] += SPH.MASS * SPH.POLY6 * (SPH.H2 - l2) * (SPH.H2 - l2) * (SPH.H2 - l2);
            }
            pressures[index] = SPH.GAS_CONST * (densities[index] - SPH.REST_DENS);
        }

        private static float sqrMagnitude(float3 pi, float3 pj){return ((pi.x - pj.x) * (pi.x - pj.x)) + ((pi.y - pj.y) * (pi.y - pj.y)) + ((pi.z - pj.z) * (pi.z - pj.z));}
    }
}