using Godot;
using System;
using Game;

public partial class Cell : Node2D
{
    public event Action Pressed;
    
    public Game.Cell CellReference { get; private set; }

    // Nodes
    private Sprite2D _icon;
    private Node2D _overlay;
    private Sprite2D _overlayIcon;
    private Label _label;
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
        _label = GetNode<Label>("%Label");
        
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
            _overlay.Modulate = Colors.White;
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
            _overlay.Modulate = _overlay.Modulate.Lerp(new Color(0f, 0f, 0f, 0.0f), (float) delta * 2f);
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

        var shader = _icon.Material as ShaderMaterial;
        if (CellReference?.OwnedBy != Player.None && CellReference?.Duration == 1)
        {
            shader?.SetShaderParameter("isFlashing", true);
        }
        else
        {
            shader?.SetShaderParameter("isFlashing", false);
        }
        
        _label.Text = CellReference?.Duration.ToString();
        
        
    }
    
}
