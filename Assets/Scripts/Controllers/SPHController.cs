using Unity.Jobs;
using Jobs;
using Unity.Collections;
using Unity.Mathematics;
using Config;
using System.Collections.Generic;

namespace Controllers{
    public class SPHController : Controller
    {
        public void ComputePositionsInGrid(NativeArray<float3> positions)
        {
            NativeArray<int3> positionsInGrid = new NativeArray<int3>(positions.Length, Allocator.TempJob);
            ComputePositionsInGrid job = new ComputePositionsInGrid(){
                positions = positions,
                positionInGrid = positionsInGrid
            };
            JobHandle jobHandle = job.Schedule(this.particlesCount, ECS.INNER_LOOP_BATCH_COUNT);
            jobHandle.Complete();
            this.setGrid(positionsInGrid);
            positionsInGrid.Dispose();
        }

        public void computeDensityAndPressure(NativeArray<float3> positions, NativeArray<float> densities, NativeArray<float> pressures)
        {
            NativeArray<JobHandle> jobHandles = new NativeArray<JobHandle>(this.particlesCount, Allocator.Temp);
            List<NativeArray<float2>> results = new List<NativeArray<float2>>();
            for(int i = 0; i < this.particlesCount; i++){
                results.Add(new NativeArray<float2>(1, Allocator.TempJob));
                ComputeDensityAndPressure job = new ComputeDensityAndPressure(){
                    positions = positions,
                    position = positions[i],
                    result = results[i],
                };
                jobHandles[i] = job.Schedule();
            }
            JobHandle.CompleteAll(jobHandles);
            for(int i = 0; i < this.particlesCount; i++){
                densities[i] = results[i][0].x;
                pressures[i] = results[i][0].y;
                results[i].Dispose();
            }
            jobHandles.Dispose();
        }

        public void computeForces(NativeArray<float3> positions, NativeArray<float> densities, NativeArray<float3> velocities, NativeArray<float3> forces, NativeArray<float> pressures)
        {
            ComputeForces job = new ComputeForces(){
                positions = positions,
                velocities = velocities,
                forces = forces,
                densities = densities,
                pressures = pressures
            };
            JobHandle jobHandle = job.Schedule(this.particlesCount, ECS.INNER_LOOP_BATCH_COUNT);
            jobHandle.Complete();
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

            this.ComputePositionsInGrid(positions);
            this.computeDensityAndPressure(positions, densities, pressures);
            this.computeForces(positions, densities, velocities, forces, pressures);
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
        }
    }
}