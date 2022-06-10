using UnityEngine;
using System.Collections.Generic;
using Models;
using Unity.Jobs;
using Jobs;
using Unity.Collections;
using Unity.Mathematics;
using Config;

namespace Controllers{
    public class SPHController : Controller
    {    

        public void computeDensityAndPressure(NativeArray<float3> positions, NativeArray<float> densities, NativeArray<float> pressures)
        {
            ComputeDensityAndPressure job = new ComputeDensityAndPressure(){
                positions = positions,
                densities = densities,
                pressures = pressures
            };
            JobHandle jobHandle = job.Schedule(this.particlesCount, ECS.INNER_LOOP_BATCH_COUNT);
            jobHandle.Complete();
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