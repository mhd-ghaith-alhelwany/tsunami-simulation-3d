using UnityEngine;
using Unity.Entities;
using Main;
using Unity.Mathematics;

namespace Generators{
    public class BoxGenerator : Generator
    {
        private ECSGame game;

        private float3 center, size;

        private GameObjectConversionSettings settings;
        private Entity prefab;
        private EntityManager entityManager;
        private GameObject objectPrefab;

        public BoxGenerator(GameObject objectPrefab, ECSGame game, float3 size, float3 center) : base()
        {
            this.game = game;
            this.size = size;
            this.center = center;
            this.objectPrefab = objectPrefab;
        }

        public override void start()
        {
            this.objectPrefab.transform.localScale = this.size;
            this.objectPrefab.transform.localPosition = this.center;

            this.settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
            this.prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(this.objectPrefab, settings);
            this.entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            var instance = this.entityManager.Instantiate(prefab);
            var t = this.game.getTransform().TransformPoint(center);
            this.entityManager.AddComponentData(instance, new BoxCollider{
                size = size,
                center = center
            });

        }

        public override void update()
        {
            return;
        }
    }
}