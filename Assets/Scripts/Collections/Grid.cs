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
    }
    public class Grid<T>
    {
        private int x, y, z;
        private Cell<T>[,,] cells;
        public Grid(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.cells = new Cell<T>[x, y, z];
            for(int i = 0; i < y; i++)
                for(int j = 0; j < y; j++)
                    for(int k = 0; k < y; k++)
                        this.cells[i, j, k] = new Cell<T>(i, j);
        }
        public Cell<T> getCell(int x, int y, int z)
        {
            return this.cells[x, y, z];
        }
    }
}