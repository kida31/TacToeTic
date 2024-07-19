using System;

namespace Game;

public class Cell
{
    public int Duration { get; private set; }
    public Player OwnedBy { get; private set; }

    private Match _match;

    public Cell()
    {
        Duration = 0;
        OwnedBy = Player.None;
    }

    public void Tick()
    {
        Duration = Math.Max(0, Duration - 1);
        if (Duration <= 0)
        {
            OwnedBy = Player.None;
        }
    }

    public void SetOwner(Player player, int duration)
    {
        OwnedBy = player;
        Duration = duration;
    }
}