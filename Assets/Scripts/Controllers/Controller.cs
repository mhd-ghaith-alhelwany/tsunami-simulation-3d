using Unity.Collections;
using Unity.Mathematics;
using System.Collections.Generic;
using Models;
using Collections;
using Config;
using System.Diagnostics;

namespace Controllers{
    public abstract class Controller
    {
        public abstract void update();

        private List<Particle> particles;
        protected int particlesCount;

        public Grid<int> particlesGrid;

        public NativeArray<float3> positions;
        public NativeArray<float3> velocities;
        public NativeArray<float3> forces;
        public NativeArray<float> densities;
        public NativeArray<float> pressures;
        public NativeArray<int> ids;
        public NativeArray<int3> positionsInGrid;

        public void setParticles(List<Particle> particles){
            this.particles = particles; 
            this.particlesCount = particles.Count;
            positions = this.getPositions();
            velocities = this.getVelocities();
            forces = this.getForces();
            densities = this.getDensities();
            pressures = this.getPressures();
            ids = this.getIds();
            positionsInGrid = this.getPositionsInGrid();
        }

        public void setGrid(NativeArray<int3> positionsInGrid){
            particlesGrid = new Grid<int>(SPATIAL_PARTITIONAING.NUMBER_OF_CELLS_X, SPATIAL_PARTITIONAING.NUMBER_OF_CELLS_Y, SPATIAL_PARTITIONAING.NUMBER_OF_CELLS_Z);
            for(int i = 0; i < positionsInGrid.Length; i++){
                particlesGrid.getCell(positionsInGrid[i].x, positionsInGrid[i].y, positionsInGrid[i].z).add(i);
            }
        }

        public List<int> getNeighbours(int3 positionInGrid){
            return this.particlesGrid.getNeightbours(positionInGrid.x, positionInGrid.y, positionInGrid.z);
        }

        public NativeArray<int> getNeighboursNativeArray(int3 positionInGrid){
            List<int> neighboursList = this.getNeighbours(positionInGrid);
            NativeArray<int> neightboursNativeArray = new NativeArray<int>(neighboursList.Count, Allocator.TempJob);
            for(int i = 0; i < neighboursList.Count; i++) neightboursNativeArray[i] = neighboursList[i];
            return neightboursNativeArray;
        }

        public NativeMultiHashMap<int, int> getNeighboursNativeArrays(NativeArray<int3> positionsInGrid){
            NativeMultiHashMap<int, int> values = new NativeMultiHashMap<int, int>(particlesCount, Allocator.TempJob);
            for(int i = 0; i < positionsInGrid.Length; i++){
                NativeArray<int> neighbours = this.getNeighboursNativeArray(positionsInGrid[i]);
                for(int j = 0; j < neighbours.Length; j++)
                    values.Add(i, neighbours[j]);
                neighbours.Dispose();
            }
            return values;
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
        protected NativeArray<int> getIds(){
            NativeArray<int> values = new NativeArray<int>(this.particles.Count, Allocator.TempJob);
            for(int i = 0; i < values.Length; i++) values[i] = this.particles[i].getId();
            return values;
        }
        protected NativeArray<int3> getPositionsInGrid(){
            NativeArray<int3> values = new NativeArray<int3>(this.particles.Count, Allocator.TempJob);
            for(int i = 0; i < values.Length; i++) values[i] = this.particles[i].getPositionInGrid();
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