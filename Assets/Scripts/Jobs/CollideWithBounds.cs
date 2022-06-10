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
            if(velocities[index][1] > 0 && position[1] + SPH.EPS >= +Simulation.wallSize/2 - Simulation.wallsThickness) { position[1] = +Simulation.wallSize/2 - SPH.EPS - Simulation.wallsThickness; velocities[index] *= SPH.BOUND_DAMPING;} else 
            if(velocities[index][1] < 0 && position[1] - SPH.EPS <= -Simulation.wallSize/2 + Simulation.wallsThickness) { position[1] = -Simulation.wallSize/2 + SPH.EPS + Simulation.wallsThickness; velocities[index] *= SPH.BOUND_DAMPING;} else 
            if(velocities[index][0] > 0 && position[0] + SPH.EPS >= +Simulation.floorX/2   - Simulation.wallsThickness) { position[0] = +Simulation.floorX/2   - SPH.EPS - Simulation.wallsThickness; velocities[index] *= SPH.BOUND_DAMPING;} else 
            if(velocities[index][0] < 0 && position[0] - SPH.EPS <= -Simulation.floorX/2   + Simulation.wallsThickness) { position[0] = -Simulation.floorX/2   + SPH.EPS + Simulation.wallsThickness; velocities[index] *= SPH.BOUND_DAMPING;} else 
            if(velocities[index][2] > 0 && position[2] + SPH.EPS >= +Simulation.floorY/2   - Simulation.wallsThickness) { position[2] = +Simulation.floorY/2   - SPH.EPS - Simulation.wallsThickness; velocities[index] *= SPH.BOUND_DAMPING;} else 
            if(velocities[index][2] < 0 && position[2] - SPH.EPS <= -Simulation.floorY/2   + Simulation.wallsThickness) { position[2] = -Simulation.floorY/2   + SPH.EPS + Simulation.wallsThickness; velocities[index] *= SPH.BOUND_DAMPING;}
            positions[index] = position;
        }
    }
}