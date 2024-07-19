using System;
using System.Linq;
using Godot;

namespace Game;

public sealed class Match
{
    public delegate void MatchOverHandler(Player winner);

    public delegate void TurnStartedHandler(Player playersTurn);

    public event MatchOverHandler MatchOver;
    public event TurnStartedHandler TurnStarted;

    public Player CurrentPlayer { get; private set; }
    public Grid CurrentGrid { get; }

    // Rules
    private readonly int _cellDuration;

    // State
    public bool IsOngoing { get; private set; }
    private Logger logger;

    public Match(Player firstTurnPlayer = Player.Circle, int cellDuration = 6)
    {
        CurrentGrid = new Grid();
        CurrentPlayer = firstTurnPlayer;
        _cellDuration = cellDuration;
        IsOngoing = true;
        logger = new Logger($"Match#{GetHashCode()}");
        logger.Info($"Started. {PlayerUtil.PlayerToString(CurrentPlayer)}'s Turn");
    }

    // Progress match by setting cell validly
    public bool TrySetCell(Cell cell)
    {
        // Check if valid turn
        if (!IsOngoing)
        {
            logger.Warn("Game is not currently running");
            return false;
        }

        if (!CellBelongsToGrid(cell))
        {
            logger.Warn("Cell does not belong to grid");
            return false;
        }

        if (cell.OwnedBy != Player.None)
        {
            logger.Warn("Cell is already occupied");
            return false;
        }

        foreach (var c in CurrentGrid.Cells)
        {
            c.Tick();
        }

        cell.SetOwner(CurrentPlayer, _cellDuration);
        CheckForWinner();
        
        CurrentPlayer = CurrentPlayer == Player.Circle ? Player.Cross : Player.Circle;
        return true;
    }

    private bool CellBelongsToGrid(Cell c)
    {
        return CurrentGrid.Cells.Any(cell => cell == c);
    }

    private void CheckForWinner()
    {
        if (HasHorizontalThree() || HasDiagonalThree() || HasVerticalThree())
        {
            var winner = CurrentPlayer;
            IsOngoing = false;
            MatchOver?.Invoke(winner);
        }
    }

    private bool HasDiagonalThree()
    {
        var cells = CurrentGrid.Cells;
        if (cells[4].OwnedBy == Player.None) return false;
        if (cells[0].OwnedBy == cells[4].OwnedBy && cells[0].OwnedBy == cells[8].OwnedBy)
        {
            return true;
        }

        return cells[2].OwnedBy == cells[4].OwnedBy && cells[2].OwnedBy == cells[6].OwnedBy;
    }

    private bool HasVerticalThree()
    {
        var cells = CurrentGrid.Cells;
        for (var i = 0; i < 3; i++)
        {
            if (cells[i].OwnedBy == Player.None) continue;
            if (cells[i].OwnedBy == cells[i + 3].OwnedBy && cells[i].OwnedBy == cells[i + 6].OwnedBy)
            {
                return true;
            }
        }

        return false;
    }

    private bool HasHorizontalThree()
    {
        var cells = CurrentGrid.Cells;
        for (var i = 0; i < 3; i++)
        {
            if (cells[i * 3].OwnedBy == Player.None) continue;
            if (cells[i * 3].OwnedBy == cells[i * 3 + 1].OwnedBy && cells[i * 3].OwnedBy == cells[i * 3 + 2].OwnedBy)
            {
                return true;
            }
        }

        return false;
    }
}