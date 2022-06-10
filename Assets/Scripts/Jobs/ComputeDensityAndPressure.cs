using Unity.Jobs;
using Components.SPH;
using Unity.Collections;
namespace Jobs{
    // [BurstCompatible]
    struct ComputeDensityAndPressure : IJobParallelFor
    {
        public NativeArray<Density> densities;
        public NativeArray<Pressure> pressures;
        public NativeArray<Position> positions;

       
        public void Execute(int index)
        {
            
        }
    }
}