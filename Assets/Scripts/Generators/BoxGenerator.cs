using Main;
using Unity.Mathematics;
using UnityEngine;

namespace Generators{
    public class BoxGenerator : Generator
    {
        private ECSGame game;

        private float3 center, size;
        private bool collides;

        public BoxGenerator(GameObject prefab, float3 size, float3 center, bool collides) : base(prefab)
        {
            this.size = size;
            this.center = center;
            this.collides = collides;
        }

        public void start()
        {
            GameObject g = this.createGameObject(this.center);
            g.transform.localScale = this.size;
        }
    }
}