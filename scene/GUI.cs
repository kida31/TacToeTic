using Godot;
using System;
using Game;

public partial class GUI : Control
{
    private Match _currentMatch = null;
    private Label _playerTurnLabel;
    private Label _gameOverLabel;

    private Logger logger;

    public override void _Ready()
    {
        logger = Logger.newForNode(this);
        _playerTurnLabel = GetNode<Label>("PlayerTurnLabel");
        _gameOverLabel = GetNode<Label>("GameOverLabel");

        logger.Info("Ready");
    }

    public override void _Process(double delta)
    {
        if (_playerTurnLabel == null)
        {
            GD.PushError("MISSING PLAYER TURN LABEL");
            return;
        }

        if (_gameOverLabel == null)
        {
            GD.PushError("MISSING GAME OVER LABEL");
            return;
        }

        if (_currentMatch == null)
        {
            GD.PushError("MISSING CURRENT MATCH");
            return;
        }

        if (_currentMatch.IsOngoing)
        {
            _playerTurnLabel.Show();
            _playerTurnLabel.Text = $"{PlayerUtil.PlayerToString(_currentMatch.CurrentPlayer)}'s turn";
            _gameOverLabel.Hide();
        }
        else
        {
            _playerTurnLabel.Hide();
        }
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