using System.Collections.Generic;

namespace Game;

public class Grid
{
    public List<Cell> Cells => new(_cells);
    private readonly List<Cell> _cells;

    public Grid()
    {
        _cells = new List<Cell>();
        for (var i = 0; i < 9; i++)
        {
            _cells.Add(new Cell());
        }
    }
}