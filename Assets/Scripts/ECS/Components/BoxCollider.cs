using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

public struct BoxCollider : IComponentData
{
    public float3 size, center;
    public bool isColliding(float3 point)
    {
        return (point.x >= (center.x - (size.x/2))) && 
               (point.x <= (center.x + (size.x/2))) &&
               (point.y >= (center.y - (size.y/2))) &&
               (point.y <= (center.y + (size.y/2))) &&
               (point.z >= (center.z - (size.z/2))) &&
               (point.z <= (center.z + (size.z/2)));
    }
}
