namespace Game;

public enum Player
{
    Circle,
    Cross,
    None,
}

static class PlayerUtil
{
    public static string PlayerToString(Player player)
    {
        return player switch
        {
            Player.Circle => "O",
            Player.Cross => "X",
            _ => "???"
        };
    }
}