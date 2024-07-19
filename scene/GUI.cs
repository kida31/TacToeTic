using Godot;
using System;
using Game;

public partial class GUI : Control
{
    private Match _currentMatch = null;
    private Label _playerTurnLabel;
    private Label _gameOverLabel;

    private Logger _logger;

    public override void _Ready()
    {
        _logger = Logger.newForNode(this);
        _gameOverLabel = GetNode<Label>("GameOverLabel");
        if (_gameOverLabel == null) _logger.Error("GameOver Label is missing");
        if (_currentMatch == null) _logger.Error("Match not set");

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
        _playerTurnLabel.Show();
    }

    private void OnMatchOver(Player winner)
    {
        _gameOverLabel.Text = $"GAME OVER\n{PlayerUtil.PlayerToString(winner)} wins";

        _gameOverLabel.Show();
        _playerTurnLabel.Hide();
    }
}