using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Components.SPH;

namespace Generators{
    public abstract class FluidGenerator : Generator
    {
        public FluidGenerator(Entity prefab, EntityManager entityManager) : base(prefab, entityManager){}
        public Entity create(float3 position, float3 velocity)
        {
            Entity entity = base.createEntity();
            entityManager.AddComponentData(entity, new Translation{Value = position + base.getNoiseVector()});
            entityManager.AddComponentData(entity, new Force{Value = new float3(0, 0, 0)});
            entityManager.AddComponentData(entity, new Velocity{Value = velocity});
            entityManager.AddComponentData(entity, new Density{Value = 0f});
            entityManager.AddComponentData(entity, new Pressure{Value = 0f});
            return entity;
        }
    }
}