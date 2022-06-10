using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using Config;

namespace Jobs{
    [BurstCompatible]
    struct ComputePositionsInGrid : IJobParallelFor
    {
        public NativeArray<float3> positions;
        public NativeArray<int3> positionInGrid;

        public void Execute(int index)
        {
            positionInGrid[index] = new int3(
                (int)(positions[index].x * SPATIAL_PARTITIONAING.NUMBER_OF_CELLS_X / Simulation.floorX),
                (int)(positions[index].y * SPATIAL_PARTITIONAING.NUMBER_OF_CELLS_Y / Simulation.wallSize),
                (int)(positions[index].z * SPATIAL_PARTITIONAING.NUMBER_OF_CELLS_Z / Simulation.floorY)
            );
        }
    }
}