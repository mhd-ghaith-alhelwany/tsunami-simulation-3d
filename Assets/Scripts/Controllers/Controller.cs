using Unity.Collections;
using Unity.Mathematics;
using System.Collections.Generic;
using Models;
using Collections;
using Config;
using UnityEngine;

namespace Controllers{
    public abstract class Controller
    {
        public abstract void update();

        private List<Particle> particles;
        protected int particlesCount;
        public void setParticles(List<Particle> particles){this.particles = particles; this.particlesCount = particles.Count;}

        public Grid<int> particlesGrid;

        public void setGrid(NativeArray<int3> positionsInGrid)
        {
            particlesGrid = new Grid<int>(SPATIAL_PARTITIONAING.NUMBER_OF_CELLS_X, SPATIAL_PARTITIONAING.NUMBER_OF_CELLS_Y, SPATIAL_PARTITIONAING.NUMBER_OF_CELLS_Z);
            for(int i = 0; i < positionsInGrid.Length; i++){
                particlesGrid.getCell(positionsInGrid[i].x, positionsInGrid[i].y, positionsInGrid[i].z).add(i);
            }
        }

        protected NativeArray<float3> getPositions(){
            NativeArray<float3> values = new NativeArray<float3>(this.particles.Count, Allocator.TempJob);
            for(int i = 0; i < values.Length; i++) values[i] = this.particles[i].getPosition();
            return values;
        }
        protected NativeArray<float3> getVelocities(){
            NativeArray<float3> values = new NativeArray<float3>(this.particles.Count, Allocator.TempJob);
            for(int i = 0; i < values.Length; i++) values[i] = this.particles[i].getVelocity();
            return values;
        }
        protected NativeArray<float3> getForces(){
            NativeArray<float3> values = new NativeArray<float3>(this.particles.Count, Allocator.TempJob);
            for(int i = 0; i < values.Length; i++) values[i] = this.particles[i].getForce();
            return values;
        }
        protected NativeArray<float> getDensities(){
            NativeArray<float> values = new NativeArray<float>(this.particles.Count, Allocator.TempJob);
            for(int i = 0; i < values.Length; i++) values[i] = this.particles[i].getDensity();
            return values;
        }
        protected NativeArray<float> getPressures(){
            NativeArray<float> values = new NativeArray<float>(this.particles.Count, Allocator.TempJob);
            for(int i = 0; i < values.Length; i++) values[i] = this.particles[i].getPressure();
            return values;
        }

        protected void setPositions(NativeArray<float3> values){
            for(int i = 0; i < values.Length; i++) this.particles[i].setPosition(values[i]);
        }
        protected void setVelocities(NativeArray<float3> values){
            for(int i = 0; i < values.Length; i++) this.particles[i].setVelocity(values[i]);
        }
        protected void setForces(NativeArray<float3> values){
            for(int i = 0; i < values.Length; i++) this.particles[i].setForce(values[i]);
        }
        protected void setDensities(NativeArray<float> values){
            for(int i = 0; i < values.Length; i++) this.particles[i].setDensity(values[i]);
        }
        protected void setPressures(NativeArray<float> values){
            for(int i = 0; i < values.Length; i++) this.particles[i].setPressure(values[i]);
        }
    }
}