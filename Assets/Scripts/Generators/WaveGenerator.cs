using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Config;
using System.Collections.Generic;
using Models;

namespace Generators{
    public class WaveGenerator: FluidGenerator
    {
        int  wavesCreated, wavesToCreate, framesCounter, updateRate, X, Y, Z;
        public WaveGenerator(GameObject prefab) : base(prefab)
        {
            this.wavesToCreate = Config.Simulation.numberOfWaves;
            this.wavesCreated = 0;
            this.framesCounter = 0;
            this.updateRate = 20;
            this.X = (int)(Config.Simulation.RoomSizeX / Config.SPH.H);
            this.Y = Config.Simulation.waveSize;
            this.Z = Config.Simulation.waveSize * 4;
        }

        public override List<Particle> start(List<Particle> particles)
        {
            return particles;
        }

        public float3 getVelocityVector()
        {
            return new float3(
                0, (2500), (5000)
            );
        }

        public float3 getPositionVector(int i, int j, int k)
        {
            return new float3(
                -Simulation.RoomSizeX/2 + Simulation.wallsThickness/2 + (i * SPH.H),
                -Simulation.RoomSizeY/2 + Simulation.wallsThickness + SPH.H - 1 - (j * SPH.H),
                -Simulation.RoomSizeZ/2 + Simulation.wallsThickness + (k * SPH.H)
            );
        }


        public override List<Particle> update(List<Particle> particles)
        {
            this.framesCounter++;
            if(this.wavesToCreate == this.wavesCreated) return null;
            if(this.framesCounter != updateRate) return particles;
            this.wavesCreated++;
            this.framesCounter = 0;
            for(int i = 5; i < X - 5; i++)
                for(int j = 0; j < Y; j++)
                    for(int k = 0; k < Z; k++)
                        particles.Add(this.create(this.getPositionVector(i, j, k), this.getVelocityVector()));
            return particles;
        }
    }
}