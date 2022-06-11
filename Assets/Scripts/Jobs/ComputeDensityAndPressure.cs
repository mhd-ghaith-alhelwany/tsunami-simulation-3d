using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using Config;

namespace Jobs{
    [BurstCompatible]
    struct ComputeDensityAndPressure : IJob
    {
        [ReadOnly] public NativeArray<float3> positions;
        [ReadOnly]public NativeArray<int> neighbouringIndexes;
        [ReadOnly]public float3 position;
        public NativeArray<float2> result;

        public void Execute()
        {
            int count = neighbouringIndexes.Length;
            float density = 0, pressure = 0;
            for(int j = 0; j < count; j++){
                int i = neighbouringIndexes[j];
                float l2 = sqrMagnitude(position, positions[i]);
                if(l2 < SPH.H2)
                    density += SPH.MASS * SPH.POLY6 * (SPH.H2 - l2) * (SPH.H2 - l2) * (SPH.H2 - l2);
            }
            pressure = SPH.GAS_CONST * (density - SPH.REST_DENS);
            result[0] = new float2(density, pressure);
        }

        private static float sqrMagnitude(float3 pi, float3 pj){return ((pi.x - pj.x) * (pi.x - pj.x)) + ((pi.y - pj.y) * (pi.y - pj.y)) + ((pi.z - pj.z) * (pi.z - pj.z));}
    }
}