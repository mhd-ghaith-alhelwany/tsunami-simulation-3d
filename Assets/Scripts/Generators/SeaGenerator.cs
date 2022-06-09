using Config;
using Unity.Mathematics;
using Unity.Entities;

namespace Generators{
    public class SeaGenerator: FluidGenerator
    {
        private int layers;

        public SeaGenerator(Entity prefab, EntityManager entityManager, int layers) : base(prefab, entityManager)
        {
            this.layers = layers;
        }

        public override void start()
        {
            int I = (int)(Simulation.floorX/Simulation.particleSize) - 1;
            int J = (int)(Simulation.floorY/Simulation.particleSize) - 1; 
            int K = layers;
            for(int i = 1; i < I; i++)
                for(int j = 1; j < J; j++)
                    for(int k = 0; k < K; k++)
                        this.create(this.getPositionVector(i, j, k), this.getEmptyVector());
        }
        public float3 getPositionVector(int i, int j, int k)
        {
            return new float3(
                i * Simulation.particleSize - (Simulation.floorY / 2), 
                (k + 1) * Simulation.particleSize + (Simulation.wallsThickness / 2) - (Simulation.wallSize / 2), 
                j * Simulation.particleSize - (Simulation.floorX / 2)
            );
        }
        public override void update()
        {
        }
    }
}