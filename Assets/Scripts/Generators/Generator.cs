using Unity.Entities;
using Unity.Mathematics;
using Components;

namespace Generators{
    public abstract class Generator
    {
        public static int ID = 0;
        private System.Random random;
        protected Entity prefab;
        protected EntityManager entityManager;

        public abstract void start();
        public abstract void update();
        
        public Generator(Entity prefab, EntityManager entityManager){
            this.prefab = prefab;
            this.entityManager = entityManager;
            this.random = new System.Random();
        }
        private float getRand(){
            return random.Next(-100, 100) / 100;
        }
        protected float3 getNoiseVector(){
            return new float3(getRand(), getRand(), getRand());
        }
        protected float3 getEmptyVector(){
            return new float3(0, 0, 0);
        }
        protected Entity createEntity(){
            Entity entity = this.entityManager.Instantiate(this.prefab);
            this.entityManager.AddComponentData(entity, new UniqueID{
                Value = ++ID
            });
            return entity;
        }
    }
}