using Unity.Entities;
using Unity.Jobs;
using Components;
using Unity.Transforms;
using Main;

public class WallCollisionSystem : SystemBase
{
    const float EPS = 16;
    const float BOUND_DAMPING = -0.5f;
    float wallLength = Config.wallSize;
    float floorX = Config.floorX;
    float floorY = Config.floorY;

    protected override void OnUpdate()
    {
        Entities.ForEach((ref SphParticle pi, ref Translation translation) => {
            
            translation.Value = pi.position;
        }).ScheduleParallel();
    }
}
