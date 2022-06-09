using Unity.Transforms;
using Unity.Entities;
using Main;
using Unity.Mathematics;

namespace Generators{
    public class BoxGenerator : Generator
    {
        private ECSGame game;

        private float3 center, size;
        private bool collides;

        public BoxGenerator(Entity prefab, EntityManager entityManager, float3 size, float3 center, bool collides) : base(prefab, entityManager)
        {
            this.size = size;
            this.center = center;
            this.collides = collides;
        }

        public override void start()
        {
            Entity entity = this.createEntity();
            entityManager.AddComponentData(entity, new Translation{Value = this.center});
            entityManager.AddComponentData(entity, new NonUniformScale{Value = this.size});
        }

        public override void update()
        {
        }
    }
}