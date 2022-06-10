using System.Collections.Generic;
namespace Components{
    class Cell<T>
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
        private int x, y;
        private Cell<T>[,] cells;
        public Grid(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.cells = new Cell<T>[x, y];
            for(int i = 0; i < y; i++)
                for(int j = 0; j < y; j++)
                    this.cells[i, j] = new Cell<T>(i, j);
        }
    }
}