using Unity.Entities;
using Unity.Mathematics;
namespace Components.SPH{
    public struct Force : IComponentData{
        public float3 Value;
    }
}