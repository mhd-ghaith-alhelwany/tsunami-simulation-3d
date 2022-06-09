// using Unity.Entities;
// using Unity.Jobs;
// using Components;
// using Unity.Transforms;
// using Main;

// public class WallCollisionSystem : SystemBase
// {
//     protected override void OnUpdate()
//     {
//         float EPS = Config.EPS;
//         float floorX = Config.floorX;
//         float floorY = Config.floorY;
//         float wallSize = Config.wallSize;
//         float BOUND_DAMPING = Config.BOUND_DAMPING;
//         float wallsThickness = Config.wallsThickness;
        
//         Entities.ForEach((ref SphParticle pi, ref Translation translation) => {
//             if(pi.velocity[1] > 0 && pi.position[1] + EPS >= +wallSize/2 - wallsThickness) { pi.position[1] = +wallSize/2 - EPS - wallsThickness; pi.velocity *= BOUND_DAMPING;} else 
//             if(pi.velocity[1] < 0 && pi.position[1] - EPS <= -wallSize/2 + wallsThickness) { pi.position[1] = -wallSize/2 + EPS + wallsThickness; pi.velocity *= BOUND_DAMPING;} else 
//             if(pi.velocity[0] > 0 && pi.position[0] + EPS >= +floorX/2   - wallsThickness) { pi.position[0] = +floorX/2   - EPS - wallsThickness; pi.velocity *= BOUND_DAMPING;} else 
//             if(pi.velocity[0] < 0 && pi.position[0] - EPS <= -floorX/2   + wallsThickness) { pi.position[0] = -floorX/2   + EPS + wallsThickness; pi.velocity *= BOUND_DAMPING;} else 
//             if(pi.velocity[2] > 0 && pi.position[2] + EPS >= +floorY/2   - wallsThickness) { pi.position[2] = +floorY/2   - EPS - wallsThickness; pi.velocity *= BOUND_DAMPING;} else 
//             if(pi.velocity[2] < 0 && pi.position[2] - EPS <= -floorY/2   + wallsThickness) { pi.position[2] = -floorY/2   + EPS + wallsThickness; pi.velocity *= BOUND_DAMPING;}
//             translation.Value = pi.position;
//         }).ScheduleParallel();
//     }
// }
