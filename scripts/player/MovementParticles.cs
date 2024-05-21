using Godot;
using System;

public partial class MovementParticles : GpuParticles3D
{
    [Export] private Player ParentPlayer;
    [Export] private bool EmitOnlyWhenRunning = true;

    public void TryEmitMovementParticle(Transform3D xform, Vector3 velocity, Color color, Color custom, uint flags)
    {//animation method
        if (EmitOnlyWhenRunning == true && ParentPlayer.IsRunning == false)
        { return; }
        this.EmitParticle(xform, velocity, color, custom, flags);
    }
}
