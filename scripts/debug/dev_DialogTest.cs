using Godot;
using System;

public partial class dev_DialogTest : Node
{
    [Export] private Player player;

    private bool _firstFrame = true;
    private string GameUINonMenuNode = "/root/GameUI/Non-main menu";
    private bool Enabled = true;
    private SceneFade _sceneFade;
    public override void _Ready()
    {
        player.MayDoStuff = false;
        _sceneFade = GetNode<SceneFade>(SceneFade.SCENE_FADE_TREE);
        _sceneFade.FadeFromScene();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Enabled == false)
        { return; }
        if (_firstFrame == true)
        {
            GetNode(GameUINonMenuNode).ProcessMode = ProcessModeEnum.Inherit;
            DialogUI.Instance.DisplayDialogWithDelay(1.1f, this);
            _firstFrame = false;
        }
        else
        {
            if (_sceneFade.IsFading == true)
            { return; }
            //GD.Print("finished dialog: "+DialogUI.Instance.FinishedDialog);
            if (DialogUI.Instance.FinishedDialog == true)
            {
                GetNode<HudUI>(HudUI.HUD_UI_TREE).Visible = true;
                player.MayDoStuff = true;
                GD.Print($"time: {GameTimer.Instance.Time}({GameTimer.Instance.RawTime})");
                GameTimer.Instance.StartTimer(true);
                Enabled = false;
            }
        }
    }
}
