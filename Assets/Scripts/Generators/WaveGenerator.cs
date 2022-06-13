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
        public WaveGenerator(GameObject prefab, int wavesToCreate) : base(prefab)
        {
            this.wavesToCreate = wavesToCreate;
            this.wavesCreated = 0;
            this.framesCounter = 0;
            this.updateRate = 20;
            this.X = (int)(Config.Simulation.RoomSizeX / Config.SPH.H) - 1;
            this.Y = 2;
            this.Z = 10;
        }

        public override List<Particle> start(List<Particle> particles)
        {
            return particles;
        }

        public float3 getVelocityVector()
        {
            return new float3(
                0, 5000, 5000
            );
        }

        public float3 getPositionVector(int i, int j, int k)
        {
            return new float3(
                -Simulation.RoomSizeX/2 + Simulation.wallsThickness/2 + (i * SPH.H),
                -Simulation.RoomSizeY/2 + Simulation.wallsThickness + SPH.H - 1 - (j * SPH.H),
                -Simulation.RoomSizeZ/2 + Simulation.wallsThickness + (k * SPH.H)
            ) + this.getNoiseVector();
        }


        public override List<Particle> update(List<Particle> particles)
        {
            this.framesCounter++;
            if(this.wavesToCreate == this.wavesCreated) return null;
            if(this.framesCounter != updateRate) return particles;
            this.wavesCreated++;
            this.framesCounter = 0;
            for(int i = 2; i < X - 2; i++)
                for(int j = 0; j < Y; j++)
                    for(int k = 0; k < Z; k++)
                        particles.Add(this.create(this.getPositionVector(i, j, k), this.getVelocityVector()));
            Debug.Log("Wave " + this.wavesCreated.ToString() + " was created");
            return particles;
        }
    }
}