using Godot;
using System;

public partial class dev_DialogTest : Node
{
    [Export] private Player player;

    private bool _firstFrame = true;
    private string GameUINonMenuNode = "/root/GameUI/Non-main menu";
    private bool Enabled = true;
    public override void _Ready()
    {
        player.MayDoStuff = false;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Enabled == false)
        { return;}
        if (_firstFrame == true)
        {
            GetNode(GameUINonMenuNode).ProcessMode = ProcessModeEnum.Inherit;
            DialogUI.Instance.DisplayDialog(this);
            _firstFrame = false;
        }
        else
        {
            //GD.Print("finished dialog: "+DialogUI.Instance.FinishedDialog);
            if (DialogUI.Instance.FinishedDialog == true)
            {                
                player.MayDoStuff = true;
                Enabled = false;
            }
        }
    }
}
