using UnityEngine;
using Unity.Entities;
using Main;
using Unity.Transforms;

namespace Generators{
    public class WaveGenerator : Generator
    {
        private ECSGame game;

        private float position;

        private GameObjectConversionSettings settings;
        private Entity prefab;
        private EntityManager entityManager;
        private GameObject objectPrefab;
        public WaveGenerator(GameObject objectPrefab, ECSGame game, float position) : base()
        {
            this.game = game;
            this.position = position;
            this.objectPrefab = objectPrefab;
        }

        public override void start()
        {
            Vector3 scale = new Vector3(this.game.getFloorSize()[0], 10, 10);
            Vector3 position = new Vector3(0, 0, this.position);

            this.objectPrefab.transform.localScale = scale;
            this.objectPrefab.transform.localPosition = position;

            this.settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
            this.prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(this.objectPrefab, settings);
            this.entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            var instance = this.entityManager.Instantiate(prefab);
            var t = this.game.getTransform().TransformPoint(position);
            this.entityManager.AddComponentData(instance, new Translation{
                Value = t,
            });
            this.entityManager.AddComponentData(instance, new BoxCollider{
                size = scale,
                center = position
            });

        }

        public override void update()
        {
            return;
        }
    }
}