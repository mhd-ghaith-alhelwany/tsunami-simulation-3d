using UnityEngine;
using Main;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Components;

namespace Generators.ECS{
    public class BucketGenerator: Generator
    {
        private ECSGame game;
        private Vector3 gridSize;
        private Vector3 startingPoint;
        private GameObjectConversionSettings settings;
        private Entity prefab;
        private EntityManager entityManager;

        public BucketGenerator(ECSGame game, Vector3 gridSize, Vector3 startingPoint) : base()
        {
            this.game = game;
            this.gridSize = gridSize;
            this.startingPoint = startingPoint;
        }

        override
        public void start()
        {
            this.settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
            this.prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(this.game.getFluidPrefab(), settings);
            this.entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            Vector3 boxSize = this.game.getBoxSize();
            float particleSize = this.game.getParticleSize();
            int id = 1;

            for(int i = 0; i < this.gridSize[0]; i++){
                float randi = 0;
                randi += getRand();
                for(int j = 0; j < this.gridSize[1]; j++){
                    for(int k = 0; k < this.gridSize[2]; k++){
                        float randk = 0;
                        randk += getRand();
                        this.CreateEntity(
                            id, 
                            i * particleSize + startingPoint[0] + randi,
                            j * particleSize + startingPoint[1],
                            k * particleSize + startingPoint[2] + randk
                        );
                        id++;
                    }
                }
            }
        }


        public void CreateEntity(int id, float x, float y, float z)
        {            
            var instance = this.entityManager.Instantiate(prefab);

            var t = this.game.getTransform().TransformPoint(new float3(x, y, z));

            this.entityManager.SetComponentData(instance, new Translation{
                Value = t
            });
            this.entityManager.AddComponentData(instance, new SphParticle{
                particleId = id,
                force = new float3(0.0f, 0.0f, 0.0f),
                position = new float3(x, y, z),
                velocity = new float3(0.0f, 0.0f, 0.0f),
                density = 0.0f,
                pressure = 0.0f
            });
        }

        private float getRand()
        {
            return new System.Random().Next((int)this.game.getParticleSize() / 4);
        }

        override
        public void update()
        {
        }
    }
}