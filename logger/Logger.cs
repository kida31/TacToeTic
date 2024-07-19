using Godot;
using System;
using System.Diagnostics;

/**
 * Not a real logger. Just pretty print to console.
 */
public class Logger
{
    private readonly string context;

    public Logger(string context)
    {
        this.context = context;
    }

    public static Logger newForNode(Node node)
    {
        Debug.Assert(node != null, nameof(node) + " != null");
        var logger = new Logger(node.GetPath().ToString());
        return logger;
    }

    public void Info(Object content)
    {
        GD.PrintRich($"[color=green][{context}][INFO] {content}[/color]");
    }
    
    public void Warn(Object content)
    {
        GD.PrintRich($"[color=yellow][{context}][WARN] {content}[/color]");
        GD.PushWarning($"[{context}] {content}");
    }
    
    public void Error(Object content)
    {
        GD.PrintRich($"[color=red][{context}][ERROR] {content}[/color]");
        GD.PushError($"[{context}] {content}");
    }
}
