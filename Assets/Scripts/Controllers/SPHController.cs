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
        List<Particle> particles;
        int count;
        public void setParticles(List<Particle> particles){this.particles = particles; this.count = particles.Count;}

        private NativeArray<float3> getPositions(){
            NativeArray<float3> values = new NativeArray<float3>(particles.Count, Allocator.TempJob);
            for(int i = 0; i < values.Length; i++) values[i] = particles[i].getPosition();
            return values;
        }
        private NativeArray<float3> getVelocities(){
            NativeArray<float3> values = new NativeArray<float3>(particles.Count, Allocator.TempJob);
            for(int i = 0; i < values.Length; i++) values[i] = particles[i].getVelocity();
            return values;
        }
        private NativeArray<float3> getForces(){
            NativeArray<float3> values = new NativeArray<float3>(particles.Count, Allocator.TempJob);
            for(int i = 0; i < values.Length; i++) values[i] = particles[i].getForce();
            return values;
        }
        private NativeArray<float> getDensities(){
            NativeArray<float> values = new NativeArray<float>(particles.Count, Allocator.TempJob);
            for(int i = 0; i < values.Length; i++) values[i] = particles[i].getDensity();
            return values;
        }
        private NativeArray<float> getPressures(){
            NativeArray<float> values = new NativeArray<float>(particles.Count, Allocator.TempJob);
            for(int i = 0; i < values.Length; i++) values[i] = particles[i].getPressure();
            return values;
        }

        private void setPositions(NativeArray<float3> values){
            for(int i = 0; i < values.Length; i++) particles[i].setPosition(values[i]);
        }
        private void setVelocities(NativeArray<float3> values){
            for(int i = 0; i < values.Length; i++) particles[i].setVelocity(values[i]);
        }
        private void setForces(NativeArray<float3> values){
            for(int i = 0; i < values.Length; i++) particles[i].setForce(values[i]);
        }
        private void setDensities(NativeArray<float> values){
            for(int i = 0; i < values.Length; i++) particles[i].setDensity(values[i]);
        }
        private void setPressures(NativeArray<float> values){
            for(int i = 0; i < values.Length; i++) particles[i].setPressure(values[i]);
        }

        public void computeDensityAndPressure(NativeArray<float3> positions, NativeArray<float> densities, NativeArray<float> pressures)
        {
            ComputeDensityAndPressure job = new ComputeDensityAndPressure(){
                positions = positions,
                densities = densities,
                pressures = pressures
            };
            JobHandle jobHandle = job.Schedule(this.count, ECS.INNER_LOOP_BATCH_COUNT);
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
            JobHandle jobHandle = job.Schedule(this.count, ECS.INNER_LOOP_BATCH_COUNT);
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
            JobHandle jobHandle = job.Schedule(this.count, ECS.INNER_LOOP_BATCH_COUNT);
            jobHandle.Complete();
        }

        public void collideWithBounds(NativeArray<float3> positions, NativeArray<float3> velocities)
        {
            CollideWithBounds job = new CollideWithBounds(){
                positions = positions,
                velocities = velocities,
            };
            JobHandle jobHandle = job.Schedule(this.count, ECS.INNER_LOOP_BATCH_COUNT);
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