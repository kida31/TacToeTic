using Godot;
using System;
using Game;

public partial class GUI : Control
{
    private Match _currentMatch = null;
    private RichTextLabel _gameOverLabel;
    private CanvasItem _dimOverlay;
    private Label _scoreLabel;
    private Logger _logger;

    // Not relevant anywhere, so they are squeezed here
    private int _crossScore = 0;
    private int _circleScore = 0;
    
    public override void _Ready()
    {
        _logger = Logger.newForNode(this);
        _gameOverLabel = GetNode<RichTextLabel>("%GameOverLabel");
        if (_gameOverLabel == null) _logger.Error("GameOver Label is missing");
        _dimOverlay = GetNode<CanvasItem>("%DimOverlay");
        _scoreLabel = GetNode<Label>("%Scores");

        _logger.Info("Ready");
    }

    public void SetMatch(Match match)
    {
        if (_currentMatch != null)
        {
            _currentMatch.MatchOver -= OnMatchOver;
        }

        _currentMatch = match;
        _currentMatch.MatchOver += OnMatchOver;

        _gameOverLabel.Hide();
        _dimOverlay.Hide();
    }

    private void OnMatchOver(Player winner)
    {
        _gameOverLabel.Text = $"[center]GAME OVER\n[color=orange]{PlayerUtil.PlayerToString(winner)}[/color] wins[/center]";

        _gameOverLabel.Show();
        _dimOverlay.Show();
        
        switch (winner)
        {
            case Player.Circle:
                _circleScore++;
                break;
            case Player.Cross:
                _crossScore++;
                break;
        }
        
        _scoreLabel.Text = $"{_circleScore} : {_crossScore}";
    }
}