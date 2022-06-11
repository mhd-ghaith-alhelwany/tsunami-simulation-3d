using System.Collections.Generic;

namespace Collections{
    public class Cell<T>
    {
        private List<T> list;
        private int i, j;
        public Cell(int i, int j){
            this.i = i;
            this.j = j;
            this.list = new List<T>();
        }
        public void add(T t){
            this.list.Add(t);
        }
        public List<T> getList()
        {
            return this.list;
        }
    }
    public class Grid<T>
    {
        private Cell<T>[,,] cells;
        private int X, Y, Z;
        public Grid(int x, int y, int z)
        {
            this.X = x; this.Y = y; this.Z = z;
            this.cells = new Cell<T>[x, y, z];
            for(int i = 0; i < x; i++)
                for(int j = 0; j < y; j++)
                    for(int k = 0; k < z; k++)
                        this.cells[i, j, k] = new Cell<T>(i, j);
        }
        public Cell<T> getCell(int x, int y, int z)
        {
            return this.cells[x, y, z];
        }

        private int[] moves = new int[3]{0, 1, -1};
        private bool outsideGrid(int x, int y, int z){
            return x < 0 || y < 0 || z < 0 || x >= X || y >= Y || z >= Z;
        }
        public List<T> getNeightbours(int x, int y, int z)
        {
            List<T> list = new List<T>();
            for(int i = 0; i < 3; i++)
                for(int ii = 0; ii < 3; ii++)
                    for(int iii = 0; iii < 3; iii++){
                        int xx = x + moves[i];
                        int yy = y + moves[ii];
                        int zz = z + moves[iii];
                        if(!this.outsideGrid(xx, yy, zz))
                            list.AddRange(cells[xx, yy, zz].getList());
                    }
            return list;
        }
    }
}