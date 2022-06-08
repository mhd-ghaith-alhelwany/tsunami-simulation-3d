using UnityEngine;
using Unity.Entities;
using Main;
using Unity.Mathematics;
using Unity.Transforms;

namespace Generators.ECS.Objects{
    public class BoxGenerator : ObjectGenerator
    {
        private ECSGame game;

        private float3 center, size;

        private GameObjectConversionSettings settings;
        private Entity prefab;
        private EntityManager entityManager;

        public BoxGenerator(GameObject objectPrefab, ECSGame game, float3 size, float3 center) : base(objectPrefab)
        {
            this.game = game;
            this.size = size;
            this.center = center;
        }

        public override void generate()
        {
            base.objectPrefab.transform.localScale = this.size;
            base.objectPrefab.transform.localPosition = this.center;

            this.settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
            this.prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(base.objectPrefab, settings);
            this.entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            var instance = this.entityManager.Instantiate(prefab);
            var t = this.game.getTransform().TransformPoint(center);
            this.entityManager.AddComponentData(instance, new BoxCollider{
                size = size,
                center = center
            });

        }
    }
}