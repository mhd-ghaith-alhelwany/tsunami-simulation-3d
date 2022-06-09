using Unity.Entities;
using Unity.Mathematics;
namespace Components.SPH{
    public struct Velocity : IComponentData{
        public float3 Value;
    }
}