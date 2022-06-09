using Jobs;
using Unity.Jobs;
namespace Controllers{
    public class SPHController {
        public SPHController(){}
        public void start(){}
        public void update(){
            var job = new ComputeDensityAndPressure(){};
            job.Schedule(1, 64);

            // init arrays
            // init jobs
            // call jobs in order
            // dispose arrays
        }
    }
}