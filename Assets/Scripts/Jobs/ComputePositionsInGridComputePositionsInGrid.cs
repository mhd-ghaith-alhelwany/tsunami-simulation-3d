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
                (int)maxMin(0, (int)((positions[index].x + Simulation.RoomSizeX / 2) * SPATIAL_PARTITIONAING.NUMBER_OF_CELLS_X / Simulation.RoomSizeX), SPATIAL_PARTITIONAING.NUMBER_OF_CELLS_X - 1),
                (int)maxMin(0, (positions[index].y * SPATIAL_PARTITIONAING.NUMBER_OF_CELLS_Y / Simulation.RoomSizeY), SPATIAL_PARTITIONAING.NUMBER_OF_CELLS_Y - 1),
                (int)maxMin(0, (int)((positions[index].z + Simulation.RoomSizeZ / 2) * SPATIAL_PARTITIONAING.NUMBER_OF_CELLS_Z / Simulation.RoomSizeZ), SPATIAL_PARTITIONAING.NUMBER_OF_CELLS_Z - 1)
            );
        }

        public float maxMin(float x, float y, float z){
            if(y > z) return z;
            if(y < x) return x;
            return y;
        }
    }
}