using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Game;

public partial class Grid : Node2D
{
    private Match _currentMatch;

    private List<Cell> _cellNodes;
    private Logger logger;
    public override void _Ready()
    {
        logger = Logger.newForNode(this);
        _cellNodes = GetNode<Node2D>("%Cells")
            .GetChildren()
            .Select(n => n as Cell)
            .ToList();

        foreach (var node in _cellNodes)
        {
            if (node is not Cell cell) continue;

            cell.Pressed += () =>
            {
                _currentMatch?.TrySetCell(cell.CellReference);
            };
        }
    }

    public void SetMatch(Match match)
    {
        logger?.Info($"Set Match. Setting up {_cellNodes.Count} cells");
        _currentMatch = match;
        for (int i = 0; i < _cellNodes.Count; i++)
        {
            var cell2d = _cellNodes[i];
            var cellRef = _currentMatch.CurrentGrid.Cells[i];
            cell2d.SetCell(cellRef);
            cell2d.SetMatch(match);
        }
    }
}