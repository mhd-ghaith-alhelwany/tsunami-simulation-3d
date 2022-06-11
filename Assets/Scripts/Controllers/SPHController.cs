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

        public void computeDensityAndPressure(NativeArray<int3> positionsInGrid, NativeArray<float3> allPositions, NativeArray<float> allDensities, NativeArray<float> allPressures)
        {
            NativeArray<JobHandle> jobHandles = new NativeArray<JobHandle>(this.particlesCount, Allocator.Temp);
            List<NativeArray<float2>> results = new List<NativeArray<float2>>();
            List<NativeArray<int>> neighbouringIndexes = new List<NativeArray<int>>();

            for(int i = 0; i < this.particlesCount; i++){
                results.Add(new NativeArray<float2>(1, Allocator.TempJob));
                neighbouringIndexes.Add(this.getNeighboursNativeArray(positionsInGrid[i]));
                ComputeDensityAndPressure job = new ComputeDensityAndPressure(){
                    neighbouringIndexes = neighbouringIndexes[i],
                    positions = allPositions,
                    position = allPositions[i],
                    result = results[i],
                };
                jobHandles[i] = job.Schedule();
            }
            JobHandle.CompleteAll(jobHandles);
            for(int i = 0; i < this.particlesCount; i++){
                allDensities[i] = results[i][0].x;
                allPressures[i] = results[i][0].y;
                results[i].Dispose();
                Debug.Log(100 * neighbouringIndexes[i].Length / particlesCount);
                neighbouringIndexes[i].Dispose();
            }
            jobHandles.Dispose();
        }

        public void computeForces(NativeArray<int3> positionsInGrid, NativeArray<int> allIds, NativeArray<float3> allPositions, NativeArray<float> allDensities, NativeArray<float3> allVelocities, NativeArray<float3> allForces, NativeArray<float> allPressures)
        {
            NativeArray<JobHandle> jobHandles = new NativeArray<JobHandle>(this.particlesCount, Allocator.Temp);
            List<NativeArray<float3>> results = new List<NativeArray<float3>>();
            List<NativeArray<int>> neighbouringIndexes = new List<NativeArray<int>>();
            for(int i = 0; i < this.particlesCount; i++){
                results.Add(new NativeArray<float3>(1, Allocator.TempJob));
                neighbouringIndexes.Add(this.getNeighboursNativeArray(positionsInGrid[i]));
                ComputeForces job = new ComputeForces(){
                    neighbouringIndexes = neighbouringIndexes[i],
                    positions = allPositions,
                    velocities = allVelocities,
                    forces = allForces,
                    densities = allDensities,
                    pressures = allPressures,
                    ids = allIds,
                    position = allPositions[i],
                    density = allDensities[i],
                    force = allForces[i],
                    id = allIds[i],
                    pressure = allPressures[i],
                    velocity = allVelocities[i],
                    result = results[i]
                };
                jobHandles[i] = job.Schedule();
            }
            JobHandle.CompleteAll(jobHandles);
            for(int i = 0; i < this.particlesCount; i++){
                allForces[i] = results[i][0];
                results[i].Dispose();
                neighbouringIndexes[i].Dispose();
            }
            jobHandles.Dispose();
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
            this.computeDensityAndPressure(positionsInGrid, positions, densities, pressures);
            this.computeForces(positionsInGrid, ids, positions, densities, velocities, forces, pressures);
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
        }
    }
}