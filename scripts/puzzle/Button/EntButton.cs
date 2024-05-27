using Godot;
using ImGuiNET;
using System;
using System.Linq;

public partial class EntButton : Node3D
{
    [Export] private Node3D ButtonRestNode, ButtonPressedNode;
    [Export] private Area3D ButtonArea;
    [Export] private ButtonInteractable[] Targets;

    private bool _pressed = false;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ButtonArea.BodyEntered += Button_BodyEntered;
        ButtonArea.BodyExited += Button_BodyExited;
        SetButtonState(false);
    }

#if DEBUG && GODOT_PC
    public override void _Process(double delta)
    {
        Godot.Camera3D cam = GetViewport().GetCamera3D();
        if (this.IsVisibleToCamera3D(cam, out Vector2 camPos))
        {
            ImGui.Begin("button", ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoNav | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoMouseInputs);
            ImGui.PushStyleColor(ImGuiCol.Text, new System.Numerics.Vector4(0, 1, 0, 1));
            ImGui.SetWindowPos(new System.Numerics.Vector2(camPos.X, camPos.Y));
            ImGui.Text($"Name: {Name}\nModel: {GetModel()}\nPressed: {_pressed}\nTargets: {string.Join("\n", Targets.AsEnumerable())}");
            ImGui.PopStyleColor();
            ImGui.End();
        }
    }
#endif


    private void Button_BodyExited(Node3D body)
    {
        if (body is RigidBody3D || (body is Player && body is Area3D == false))
        {
            SetButtonState(false);
        }
    }

    private void Button_BodyEntered(Node3D body)
    {
        if (body is RigidBody3D || body is Player && body is Area3D == false)
        {
            SetButtonState(true);
        }
    }

    private void SetButtonState(bool pressed)
    {
        GD.Print("Pressed: " + pressed);
        ButtonRestNode.Visible = !pressed;
        ButtonPressedNode.Visible = pressed;
        _pressed = pressed;

        for (int i = 0; i < Targets.Length; i++)
        {
            if (pressed == true)
            {
                Targets[i].OnButtonPressed();
                continue;
            }
            Targets[i].OnButtonReleased();
        }
    }

#if DEBUG
    private string GetModel()
    {
        return _pressed ? ButtonPressedNode.Name : ButtonRestNode.Name;
    }
#endif

    public bool Pressed => _pressed;
}
