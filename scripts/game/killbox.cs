using Godot;

public partial class killbox : Area3D
{
    [Export] private Node3D RespawnPoint;

    public override void _Ready()
    {
        BodyEntered += Killbox_BodyEntered;
    }

    public override void _ExitTree()
    {
        BodyEntered -= Killbox_BodyEntered;
    }

    private void Killbox_BodyEntered(Node3D body)
    {
        if (RespawnPoint.IsValid() == false)
        {
            body.Position = Vector3.Up;
            return;
        }

        body.Position = RespawnPoint.Position;
    }
}
