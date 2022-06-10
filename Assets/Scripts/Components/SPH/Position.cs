using Unity.Entities;
using Unity.Mathematics;
namespace Components.SPH{
    public struct Position : IComponentData{
        public float3 Value;
    }
}