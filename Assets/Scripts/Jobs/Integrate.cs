using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using Config;

namespace Jobs{
    [BurstCompatible]
    struct Integrate : IJobParallelFor
    {
        public NativeArray<float3> positions;
        public NativeArray<float3> velocities;
        public NativeArray<float3> forces;
        public NativeArray<float> densities;
        
        public void Execute(int index)
        {
            velocities[index] += SPH.DT *  forces[index] / densities[index];
            positions[index] = positions[index] + (SPH.DT * velocities[index]);
        }
    }
}