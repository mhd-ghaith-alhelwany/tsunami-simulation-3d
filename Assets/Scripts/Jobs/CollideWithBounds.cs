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
            if(position[0] + SPH.EPS >= +Simulation.RoomSizeX/2 - Simulation.wallsThickness) { position[0] = +Simulation.RoomSizeX/2 - SPH.EPS - Simulation.wallsThickness; velocities[index] = new float3(velocities[index].x * SPH.BOUND_DAMPING, velocities[index].y, velocities[index].z);}
            if(position[0] - SPH.EPS <= -Simulation.RoomSizeX/2 + Simulation.wallsThickness) { position[0] = -Simulation.RoomSizeX/2 + SPH.EPS + Simulation.wallsThickness; velocities[index] = new float3(velocities[index].x * SPH.BOUND_DAMPING, velocities[index].y, velocities[index].z);}
            if(position[1] + SPH.EPS >= +Simulation.RoomSizeY/2 - Simulation.wallsThickness) { position[1] = +Simulation.RoomSizeY/2 - SPH.EPS - Simulation.wallsThickness; velocities[index] = new float3(velocities[index].x, velocities[index].y * SPH.BOUND_DAMPING, velocities[index].z);}
            if(position[1] - SPH.EPS <= -Simulation.RoomSizeY/2 + Simulation.wallsThickness) { position[1] = -Simulation.RoomSizeY/2 + SPH.EPS + Simulation.wallsThickness; velocities[index] = new float3(velocities[index].x, velocities[index].y * SPH.BOUND_DAMPING, velocities[index].z);}
            if(position[2] + SPH.EPS >= +Simulation.RoomSizeZ/2 - Simulation.wallsThickness) { position[2] = +Simulation.RoomSizeZ/2 - SPH.EPS - Simulation.wallsThickness; velocities[index] = new float3(velocities[index].x, velocities[index].y, velocities[index].z * SPH.BOUND_DAMPING);}
            if(position[2] - SPH.EPS <= -Simulation.RoomSizeZ/2 + Simulation.wallsThickness) { position[2] = -Simulation.RoomSizeZ/2 + SPH.EPS + Simulation.wallsThickness; velocities[index] = new float3(velocities[index].x, velocities[index].y, velocities[index].z * SPH.BOUND_DAMPING);}
            positions[index] = position;
        }
    }
}