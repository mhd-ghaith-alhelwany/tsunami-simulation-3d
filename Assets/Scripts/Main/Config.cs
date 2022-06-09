namespace Main{
    public static class Config{
        public const float PI = 3.141592653F;
        public const float wallSize = 256;
        public const float floorX = 512;
        public const float floorY = 512;
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
        public const float VISC = 50.0f;
        public const float DT = 0.0007f;
        public const int numberOfLayersInSea = 5;
        public const int wallsThickness = 20;
    }
}