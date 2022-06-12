using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using Config;

namespace Jobs{
    [BurstCompatible]
    struct CollideWithBounds : IJobParallelFor
    {
        public NativeArray<float3> positions;
        public NativeArray<float3> velocities;

        public void Execute(int index)
        {
            float3 position = positions[index];
            if(velocities[index][1] > 0 && position[1] + SPH.EPS >= +Simulation.RoomSizeZ/2 - Simulation.wallsThickness) { position[1] = +Simulation.RoomSizeZ/2 - SPH.EPS - Simulation.wallsThickness; velocities[index] *= SPH.BOUND_DAMPING;} else 
            if(velocities[index][1] < 0 && position[1] - SPH.EPS <= -Simulation.RoomSizeZ/2 + Simulation.wallsThickness) { position[1] = -Simulation.RoomSizeZ/2 + SPH.EPS + Simulation.wallsThickness; velocities[index] *= SPH.BOUND_DAMPING;} else 
            if(velocities[index][0] > 0 && position[0] + SPH.EPS >= +Simulation.RoomSizeX/2   - Simulation.wallsThickness) { position[0] = +Simulation.RoomSizeX/2   - SPH.EPS - Simulation.wallsThickness; velocities[index] *= SPH.BOUND_DAMPING;} else 
            if(velocities[index][0] < 0 && position[0] - SPH.EPS <= -Simulation.RoomSizeX/2   + Simulation.wallsThickness) { position[0] = -Simulation.RoomSizeX/2   + SPH.EPS + Simulation.wallsThickness; velocities[index] *= SPH.BOUND_DAMPING;} else 
            if(velocities[index][2] > 0 && position[2] + SPH.EPS >= +Simulation.RoomSizeY/2   - Simulation.wallsThickness) { position[2] = +Simulation.RoomSizeY/2   - SPH.EPS - Simulation.wallsThickness; velocities[index] *= SPH.BOUND_DAMPING;} else 
            if(velocities[index][2] < 0 && position[2] - SPH.EPS <= -Simulation.RoomSizeY/2   + Simulation.wallsThickness) { position[2] = -Simulation.RoomSizeY/2   + SPH.EPS + Simulation.wallsThickness; velocities[index] *= SPH.BOUND_DAMPING;}
            positions[index] = position;
        }
    }
}