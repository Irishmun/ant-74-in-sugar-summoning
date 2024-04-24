using Godot;
using System;

public static class ExtensionMethods
{
    public static bool IsValid<T>(this T node) where T : GodotObject
    {
        return node != null
            && GodotObject.IsInstanceValid(node)
            && !node.IsQueuedForDeletion();
    }
}
