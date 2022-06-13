namespace Config{

    public static class Simulation{
        public const int numberOfParticlesX = 20;
        public const int numberOfParticlesY = 10;
        public const int numberOfParticlesZ = 50;

        public const float RoomSizeX = (numberOfParticlesX) * 16 * 1.3f;
        public const float RoomSizeY = (numberOfParticlesY) * 16 * 10f;
        public const float RoomSizeZ = (numberOfParticlesZ) * 16 * 1.3f;
        public const int wallsThickness = 20;
    }
    
    public static class SPH{
        public const float PI = 3.141592653F;
        public const float EPS = 16;
        public const float BOUND_DAMPING = -0.5f;
        public const float H = 16;
        public const float H2 = H * H;
        public const float MASS = 2.5f;
        public const float POLY6 = 4.0f / (PI * (H*H*H*H*H*H*H*H));
        public const float GAS_CONST = 2000.0f;
        public const float REST_DENS = 300.0f;
        public const float SPIKY_GRAD = -10.0f / (PI * (H*H*H*H*H));
        public const float VISC_LAP = 40.0f / (PI * (H*H*H*H*H));
        public const float VISC = 200f;
        public const float DT = 0.0007f;
    }

    public static class ECS{
        public const int INNER_LOOP_BATCH_COUNT = 50;
    }

    public static class SPATIAL_PARTITIONAING{
        public const int NUMBER_OF_CELLS_X = Simulation.numberOfParticlesX;
        public const int NUMBER_OF_CELLS_Y = Simulation.numberOfParticlesY;
        public const int NUMBER_OF_CELLS_Z = Simulation.numberOfParticlesZ;
    }
}
