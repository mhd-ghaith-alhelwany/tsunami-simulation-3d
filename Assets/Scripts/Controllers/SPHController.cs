using Unity.Jobs;
using Jobs;
using Unity.Collections;
using Unity.Mathematics;
using Config;
using UnityEngine;
using System.Diagnostics;

namespace Controllers{
    public class SPHController : Controller
    {
        public void ComputePositionsInGrid(NativeArray<float3> positions, NativeArray<int3> positionsInGrid)
        {
            ComputePositionsInGrid job = new ComputePositionsInGrid(){
                positions = positions,
                positionInGrid = positionsInGrid
            };
            JobHandle jobHandle = job.Schedule(this.particlesCount, ECS.INNER_LOOP_BATCH_COUNT);
            jobHandle.Complete();
        }

        public void computeDensityAndPressure()
        {
            ComputeDensityAndPressure job = new ComputeDensityAndPressure(){
                densities = densities,
                neighbours = neighbours,
                neighboursMinMax = neighboursMinMax,
                positions = positions, 
                pressures = pressures,
            };
            JobHandle handle = job.Schedule(this.particlesCount, ECS.INNER_LOOP_BATCH_COUNT);
            handle.Complete();
        }

        public void computeForces()
        {
            ComputeForces job = new ComputeForces(){
                neighbours = neighbours,
                neighboursMinMax = neighboursMinMax,
                positions = positions,
                velocities = velocities,
                forces = forces,
                densities = densities,
                pressures = pressures,
                ids = ids,
            };
            JobHandle handle = job.Schedule(this.particlesCount, ECS.INNER_LOOP_BATCH_COUNT);
            handle.Complete();
        }

        public void integrate()
        {
            Integrate job = new Integrate(){
                positions = positions,
                velocities = velocities,
                forces = forces,
                densities = densities,
            };
            JobHandle jobHandle = job.Schedule(this.particlesCount, ECS.INNER_LOOP_BATCH_COUNT);
            jobHandle.Complete();
        }

        public void collideWithBounds()
        {
            CollideWithBounds job = new CollideWithBounds(){
                positions = positions,
                velocities = velocities,
            };
            JobHandle jobHandle = job.Schedule(this.particlesCount, ECS.INNER_LOOP_BATCH_COUNT);
            jobHandle.Complete();
        }

        public override void update()
        {   
            this.ComputePositionsInGrid(positions, positionsInGrid);
            this.setGrid(positionsInGrid);
            this.setNeighboursArrays(positionsInGrid);
            this.computeDensityAndPressure();
            this.computeForces();
            this.integrate();
            this.collideWithBounds();
            this.setPositions(positions);
            this.setVelocities(velocities);
        }
    }
}