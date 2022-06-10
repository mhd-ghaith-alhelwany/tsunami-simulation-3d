using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Components;
using Components.SPH;
using Jobs;
using Unity.Jobs;

public class SPHSystem : SystemBase
{

    NativeArray<UniqueID> ids;
    NativeArray<Force> forces;
    NativeArray<Density> densities;
    NativeArray<Position> positions;
    NativeArray<Pressure> pressures;
    NativeArray<Velocity> velocities;
    NativeArray<Translation> translations;
    int numberOfEntities;
    

    private void initializeArrays()
    {
        this.ids = GetEntityQuery(typeof(UniqueID)).ToComponentDataArray<UniqueID>(Allocator.TempJob);
        this.forces = GetEntityQuery(typeof(Force)).ToComponentDataArray<Force>(Allocator.TempJob);
        this.densities = GetEntityQuery(typeof(Density)).ToComponentDataArray<Density>(Allocator.TempJob);
        this.positions = GetEntityQuery(typeof(Position)).ToComponentDataArray<Position>(Allocator.TempJob);
        this.pressures = GetEntityQuery(typeof(Pressure)).ToComponentDataArray<Pressure>(Allocator.TempJob);
        this.velocities = GetEntityQuery(typeof(Velocity)).ToComponentDataArray<Velocity>(Allocator.TempJob);
        this.translations = GetEntityQuery(typeof(Translation)).ToComponentDataArray<Translation>(Allocator.TempJob);
    }

    private void disposeArrays()
    {
        this.ids.Dispose();
        this.forces.Dispose();
        this.densities.Dispose();
        this.positions.Dispose();
        this.pressures.Dispose();
        this.velocities.Dispose();
        this.translations.Dispose();
        this.numberOfEntities = this.densities.Length;
    }
    protected override void OnUpdate()
    {
        // init arrays
        // init jobs
        // call jobs in order
        // dispose arrays

        this.initializeArrays();

        JobHandle jobHandle = new ComputeDensityAndPressure{
            positions = this.positions,
            densities = this.densities,
            pressures = this.pressures,
        }.Schedule(numberOfEntities, 10);
        jobHandle.Complete();

        this.disposeArrays();
    }
}
