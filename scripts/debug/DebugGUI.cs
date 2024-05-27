using Godot;
using ImGuiNET;
using System;
using System.Numerics;

public partial class DebugGUI : Godot.Node
{
#if DEBUG && GODOT_PC
    public override void _Process(double delta)
    {
        ImGui.Begin("FPS", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoNav | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoMouseInputs);
        ImGui.Text(Engine.GetFramesPerSecond().ToString("0.000")+"fps");
        ImGui.End();
        if (Player.Instance != null)
        {
            ImGui.Begin("player", ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoNav | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoMouseInputs);
            ImGui.PushStyleColor(ImGuiCol.Text, new System.Numerics.Vector4(0, 1, 0, 1));
            Godot.Camera3D cam = GetViewport().GetCamera3D();
            Godot.Vector2 camPos = cam.UnprojectPosition(Player.Instance.DebugGlobalCoinStackPosition);
            ImGui.SetWindowPos(new System.Numerics.Vector2(camPos.X, camPos.Y));
            ImGui.Text(Player.Instance.DebugInfo);
            ImGui.PopStyleColor();
            ImGui.End();
        }
    }
#endif
}
