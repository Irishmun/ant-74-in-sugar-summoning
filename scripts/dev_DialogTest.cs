using Godot;
using System;

public partial class dev_DialogTest : Node
{
    [Export] private Player player;

    private bool _firstFrame = true;
    private string GameUINonMenuNode = "/root/GameUI/Non-main menu";
    private bool Enabled = true;
    private SceneFade _sceneFade;
    private DialogUI dialog;
    public override void _Ready()
    {
        player.MayDoStuff = false;
        _sceneFade = GetNode<SceneFade>(SceneFade.TREE);
        _sceneFade.FadeFromScene();
        dialog = GetNode<DialogUI>(DialogUI.TREE);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Enabled == false)
        { return; }
        if (_firstFrame == true)
        {
            GetNode(GameUINonMenuNode).ProcessMode = ProcessModeEnum.Inherit;
            dialog.DisplayDialogWithDelay(1.1f, this);
            _firstFrame = false;
        }
        else
        {
            if (_sceneFade.IsFading == true)
            { return; }
            //GD.Print("finished dialog: "+DialogUI.Instance.FinishedDialog);            
            if (dialog.FinishedDialog == true)
            {
                GetNode<HudUI>(HudUI.TREE).Visible = true;
                player.MayDoStuff = true;
                GD.Print($"time: {GameTimer.Instance.Time}({GameTimer.Instance.RawTime})");
                GameTimer.Instance.StartTimer(true);
                Enabled = false;
            }
            
        }
    }
}
