using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Components;
using UnityEngine;
using Unity.Transforms;

public class CollisionSystem : SystemBase
{
    const float EPS = 16;
    const float BOUND_DAMPING = -0.5f;
    protected override void OnUpdate()
    {
        NativeArray<BoxCollider> boxes = GetEntityQuery(typeof(BoxCollider)).ToComponentDataArray<BoxCollider>(Allocator.TempJob);
        Entities.ForEach((ref SphParticle pi, ref Translation translation) => {
            for(int i = 0; i < boxes.Length; i++){
                BoxCollider box = boxes[i];
                if(pi.velocity.x > 0 && box.isColliding(new float3(pi.position.x + EPS, pi.position.y, pi.position.z))){
                    pi.position = new float3(box.center.x - (box.size.x/2) - EPS, pi.position.y, pi.position.z);
                    pi.velocity *= BOUND_DAMPING;
                }
                if(pi.velocity.x < 0 && box.isColliding(new float3(pi.position.x - EPS, pi.position.y, pi.position.z))){
                    pi.position = new float3(box.center.x + (box.size.x/2) + EPS, pi.position.y, pi.position.z);
                    pi.velocity *= BOUND_DAMPING;
                }
                if(pi.velocity.y > 0 && box.isColliding(new float3(pi.position.x, pi.position.y + EPS, pi.position.z))){
                    pi.position = new float3(pi.position.x, box.center.y - (box.size.y/2) - EPS, pi.position.z);
                    pi.velocity *= BOUND_DAMPING;
                }               
                if(pi.velocity.y < 0 && box.isColliding(new float3(pi.position.x, pi.position.y - EPS, pi.position.z))){
                    pi.position = new float3(pi.position.x, box.center.y + (box.size.y/2) + EPS, pi.position.z);
                    pi.velocity *= BOUND_DAMPING;
                }
                if(pi.velocity.z > 0 && box.isColliding(new float3(pi.position.x, pi.position.y, pi.position.z + EPS))){
                    pi.position = new float3(pi.position.x, pi.position.y, box.center.z - (box.size.z/2) - EPS);
                    pi.velocity *= BOUND_DAMPING;
                }
                if(pi.velocity.z < 0 && box.isColliding(new float3(pi.position.x, pi.position.y, pi.position.z - EPS))){
                    pi.position = new float3(pi.position.x, pi.position.y, box.center.z + (box.size.z/2) + EPS);
                    pi.velocity *= BOUND_DAMPING;
                }
                translation.Value = pi.position;
            }
        }).WithReadOnly(boxes).WithDisposeOnCompletion(boxes).ScheduleParallel();
    }
}
