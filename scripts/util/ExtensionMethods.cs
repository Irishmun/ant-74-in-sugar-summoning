using Godot;
using System;

public static class ExtensionMethods
{
    /// <summary>returns whether node exists or not</summary>
    public static bool IsValid<T>(this T node) where T : GodotObject
    {
        return node != null
            && GodotObject.IsInstanceValid(node)
            && !node.IsQueuedForDeletion();
    }

    /// <summary>Remaps value from range 1 to range 2</summary>
    /// <param name="from1">low value in range 1</param>
    /// <param name="to1">high value in range 1</param>
    /// <param name="from2">low value in range 2</param>
    /// <param name="to2">high value in range 2</param>
    /// <returns>value remapped from range 1 to range 2</returns>

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
