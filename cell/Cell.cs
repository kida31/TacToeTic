using Godot;
using System;
using Game;

public partial class Cell : Node2D
{
    private const float OverlayFadeInSpeed = 8f;
    private const float OverlayFadeOutSpeed = 2f;
    private const string FlashingAnimationName = "flashing_icon";
    public event Action Pressed;

    public Game.Cell CellReference { get; private set; }

    // Nodes
    private Sprite2D _icon;
    private Node2D _overlay;
    private Sprite2D _overlayIcon;
    private Label _debugLabel;

    private AnimationPlayer _animationPlayer;

    // Resources
    private Texture2D _crossIcon;
    private Texture2D _circleIcon;

    private Logger _logger;

    private Game.Match _matchReference = null;
    private bool _isSelected;

    public override void _Ready()
    {
        _logger = Logger.newForNode(this);

        // Nodes
        _icon = GetNode<Sprite2D>("%Icon");
        _overlay = GetNode<Node2D>("%Overlay");
        _overlayIcon = _overlay.GetNode<Sprite2D>("Icon");
        _debugLabel = GetNode<Label>("%DebugLabel");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        // Resources
        _crossIcon = GD.Load<Texture2D>("res://assets/ttt_cross_white.png");
        _circleIcon = GD.Load<Texture2D>("res://assets/ttt_circle_white.png");

        var area = GetNode<Area2D>("Area2D");
        area.InputEvent += OnInputEvent;
        area.MouseEntered += () => _isSelected = true;
        area.MouseExited += () => _isSelected = false;

        _logger.Info("Ready");
    }

    private void OnInputEvent(Node viewport, InputEvent @event, long shapeidx)
    {
        if (@event is not InputEventMouseButton mouseEvent) return;

        if (mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            Pressed?.Invoke();
        }
    }

    public void SetCell(Game.Cell cell)
    {
        CellReference = cell;
    }

    public void SetMatch(Game.Match match)
    {
        _matchReference = match;
    }

    public override void _Process(double delta)
    {
        if (_isSelected)
        {
            _overlay.Modulate = _overlay.Modulate.Lerp(Colors.White, (float) delta * OverlayFadeInSpeed);

            if (_matchReference != null)
            {
                _overlayIcon.Texture = _matchReference.CurrentPlayer == Player.Circle ? _circleIcon : _crossIcon;
            }
            else
            {
                GD.Print("EMPTY");
                _overlayIcon.Texture = null;
            }
        }
        else
        {
            _overlay.Modulate =
                _overlay.Modulate.Lerp(new Color(0f, 0f, 0f, 0.0f), (float) delta * OverlayFadeOutSpeed);
        }

        switch (CellReference?.OwnedBy)
        {
            case Player.Circle:
                _icon.Texture = _circleIcon;
                _icon.Show();
                break;
            case Player.Cross:
                _icon.Texture = _crossIcon;
                _icon.Show();
                break;
            default:
                _icon.Hide();
                break;
        }

        if (CellReference?.OwnedBy != Player.None && CellReference?.Duration == 1)
        {
            _animationPlayer.Play(FlashingAnimationName);
        }
        else
        {
            _animationPlayer.Stop();
        }

        _debugLabel.Text = CellReference?.Duration.ToString();
    }
}