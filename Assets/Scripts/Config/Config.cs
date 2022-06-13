namespace Config
{

    public class Simulation
    {
        public static int numberOfParticlesX = 10;
        public static int numberOfParticlesY = 10;
        public static int numberOfParticlesZ = 40;

        public static float RoomSizeX = (numberOfParticlesX) * 16 * 1.3f;
        public static float RoomSizeY = (numberOfParticlesY) * 16 * 10f;
        public static float RoomSizeZ = (numberOfParticlesZ) * 16 * 1.3f;
        public static int wallsThickness = 20;
    }

    public class SPH
    {
        public static float PI = 3.141592653F;
        public static float EPS = 16;
        public static float H = 16;
        public static float BOUND_DAMPING = -0.5f;
        public static float H2 = H * H;
        public static float MASS = 2.5f;
        public static float POLY6 = 4.0f / (PI * (H * H * H * H * H * H * H * H));
        public static float SPIKY_GRAD = -10.0f / (PI * (H * H * H * H * H));
        public static float VISC_LAP = 40.0f / (PI * (H * H * H * H * H));
        public static float DT = 0.0007f;

        public static float GAS_CONST = 2000f;
        public static float VISC = 200f;
        public static float REST_DENS = 300f;
    }

    public class ECS
    {
        public static int INNER_LOOP_BATCH_COUNT = 50;
    }

    public class SPATIAL_PARTITIONAING
    {
        public static int NUMBER_OF_CELLS_X = Simulation.numberOfParticlesX;
        public static int NUMBER_OF_CELLS_Y = Simulation.numberOfParticlesY;
        public static int NUMBER_OF_CELLS_Z = Simulation.numberOfParticlesZ;
    }
}
