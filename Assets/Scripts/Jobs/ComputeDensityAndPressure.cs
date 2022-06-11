using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using Config;

namespace Jobs
{
    [BurstCompatible]
    struct ComputeDensityAndPressure : IJobParallelFor
    {
        [NativeDisableParallelForRestriction] public NativeArray<float3> positions;
        [NativeDisableParallelForRestriction] public NativeArray<float> densities;
        [NativeDisableParallelForRestriction] public NativeArray<float> pressures;
        [ReadOnly] public NativeMultiHashMap<int, int> neighbours;

        public void Execute(int index)
        {
            int i;
            float density = 0, pressure = 0;
            if (neighbours.TryGetFirstValue(index, out i, out var iterator)){
                do{
                    float l2 = sqrMagnitude(positions[index], positions[i]);
                    if (l2 < SPH.H2)
                        density += SPH.MASS * SPH.POLY6 * (SPH.H2 - l2) * (SPH.H2 - l2) * (SPH.H2 - l2);
                } while (neighbours.TryGetNextValue(out i, ref iterator));
            }
            pressure = SPH.GAS_CONST * (density - SPH.REST_DENS);
            densities[index] = density;
            pressures[index] = pressure;
        }

        private static float sqrMagnitude(float3 pi, float3 pj) { return ((pi.x - pj.x) * (pi.x - pj.x)) + ((pi.y - pj.y) * (pi.y - pj.y)) + ((pi.z - pj.z) * (pi.z - pj.z)); }
    }
}