using Unity.Jobs;
using Jobs;
using Unity.Collections;
using Unity.Mathematics;
using Config;
using System.Collections.Generic;
using UnityEngine;

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
            this.setGrid(positionsInGrid);
        }

        public void computeDensityAndPressure(NativeArray<int3> positionsInGrid, NativeArray<float3> positions, NativeArray<float> densities, NativeArray<float> pressures, NativeMultiHashMap<int, int> neighbours)
        {
            ComputeDensityAndPressure job = new ComputeDensityAndPressure(){
                densities = densities,
                neighbours = neighbours,
                positions = positions, 
                pressures = pressures,
            };
            JobHandle handle = job.Schedule(this.particlesCount, ECS.INNER_LOOP_BATCH_COUNT);
            handle.Complete();
        }

        public void computeForces(NativeArray<int3> positionsInGrid, NativeArray<int> allIds, NativeArray<float3> allPositions, NativeArray<float> allDensities, NativeArray<float3> allVelocities, NativeArray<float3> allForces, NativeArray<float> allPressures, NativeMultiHashMap<int, int> neighbours)
        {
            ComputeForces job = new ComputeForces(){
                neighbours = neighbours,
                positions = allPositions,
                velocities = allVelocities,
                forces = allForces,
                densities = allDensities,
                pressures = allPressures,
                ids = allIds,
            };
            JobHandle handle = job.Schedule(this.particlesCount, ECS.INNER_LOOP_BATCH_COUNT);
            handle.Complete();
        }

        public void integrate(NativeArray<float3> positions, NativeArray<float> densities, NativeArray<float3> velocities, NativeArray<float3> forces)
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

        public void collideWithBounds(NativeArray<float3> positions, NativeArray<float3> velocities)
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
            NativeArray<float3> positions = this.getPositions();
            NativeArray<float3> velocities = this.getVelocities();
            NativeArray<float3> forces = this.getForces();
            NativeArray<float> densities = this.getDensities();
            NativeArray<float> pressures = this.getPressures();
            NativeArray<int> ids = this.getIds();
            NativeArray<int3> positionsInGrid = this.getPositionsInGrid();
            
            this.ComputePositionsInGrid(positions, positionsInGrid);
            NativeMultiHashMap<int, int> neighbours = this.getNeighboursNativeArrays(positionsInGrid);

            this.computeDensityAndPressure(positionsInGrid, positions, densities, pressures, neighbours);    
            this.computeForces(positionsInGrid, ids, positions, densities, velocities, forces, pressures, neighbours);
            this.integrate(positions, densities, velocities, forces);
            this.collideWithBounds(positions, velocities);

            this.setPositions(positions);
            this.setVelocities(velocities);
            this.setForces(forces);
            this.setDensities(densities);
            this.setPressures(pressures);
            
            positions.Dispose();
            velocities.Dispose();
            forces.Dispose();
            densities.Dispose();
            pressures.Dispose();
            ids.Dispose();
            positionsInGrid.Dispose();
            neighbours.Dispose();
        }
    }
}