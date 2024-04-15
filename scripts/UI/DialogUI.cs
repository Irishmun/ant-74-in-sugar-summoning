using Godot;
using Godot.Collections;
using System;
using System.Collections;

public partial class DialogUI : Node
{

    public static DialogUI Instance { get; private set; }

    [ExportGroup("Settings")]
    [Export] private float TypeSpeed;
    [Export] private float TimeBeforeHide;
    [ExportGroup("Dialog")]
    [Export(PropertyHint.File, "*.json")] private string DialogFile;
    [ExportGroup("Nodes")]
    [Export] private CanvasItem TextParent;
    [Export] private RichTextLabel TextNode;
    [Export] private AnimationPlayer Animator;
    [Export] private string ShowName, HideName;
    [Export] private Control FinishedIndicator;

    private bool _displayingText, _showingAllText;
    private string _currentText, _currentKey;
    private float _displayedPercentage = 0f;
    private int _maxTextCharacters = 0;
    private float _finishedTime = 0f;
    private float _deltaf;

    private float _delay = 0, _currentDelay = 0;
    private bool _useDelay = false;

    private bool _finishedDialog = false;

    private Queue _lines = new Queue();
    public bool FinishedCurrentText { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        //_signalBus.DisplayDialog += DisplayDialogUI;
        Init();
    }

    public void Init()
    {
        GD.Print("Init dialog");
        LoadTextFile();
        TextParent.Visible = false;
        TextNode.Text = string.Empty;
        TextNode.Visible = false;
        FinishedIndicator.Visible = false;
    }

    public override void _Process(double delta)
    {
        if (_displayingText == true)
        {
            _deltaf = (float)delta;
            if (_useDelay == true)
            {
                if (_currentDelay < _delay)
                {
                    _currentDelay += _deltaf;
                    return;
                }
            }
            if (_showingAllText == true)
            {//current line done, hide if no more lines
                _finishedTime += _deltaf;
                if (_finishedTime >= TimeBeforeHide)
                {
                    if (_lines.Count == 0)
                    {
                        HideDialog();
                        return;
                    }
                    _showingAllText = false;
                    _displayingText = false;
                    DialogSetup();//<---
                    return;
                }
            }
            if (TypeSpeed > 0)
            {
                _displayedPercentage += (_deltaf * TypeSpeed);
                _displayedPercentage = Mathf.Clamp(_displayedPercentage, 0f, 1f);
            }
            else
            {
                _displayedPercentage = 1f;
            }
            TextNode.VisibleCharacters = (int)(_maxTextCharacters * _displayedPercentage);
            if (TextNode.VisibleCharacters >= _maxTextCharacters)
            {
                _showingAllText = true;
                FinishedIndicator.Visible = true;
            }
        }
    }
    private void DisplayDialogUI()
    {
        if (_lines == null)
        {
            return;
        }
        if (_displayingText == false)
        {
            DialogSetup();
        }
        else if (_displayingText == true && _showingAllText == false)
        {
            TextNode.VisibleCharacters = _maxTextCharacters;
            _displayedPercentage = 1f;
            _showingAllText = true;
            _useDelay = false;
        }
        else
        {
            _finishedTime = TimeBeforeHide + 1;
        }
    }
    private void DialogSetup()
    {
        if (_lines.Count == 0)
        {
            HideDialog();
            return;
        }
        _currentText = (string)_lines.Dequeue();
        _maxTextCharacters = _currentText.Length;
        ShowDialog();
    }
    private void ShowDialog()
    {
        if (_displayingText == true)
        {
            WriteDialog();
            return;
        }
        TextNode.Text = string.Empty;
        FinishedIndicator.Visible = false;
        //play animation
        if (TextParent.Visible == false)
        {
            TextParent.Visible = true;
            Animator.Play(ShowName);
        }
        else
        {
            //assume in dialog
            _displayedPercentage = 0f;
            WriteDialog();
        }
    }
    private void HideDialog()
    {
        if (TextParent.Visible == false)
        {
            return;
        }
        TextNode.VisibleCharacters = 0;
        Animator.Play(HideName);
    }

    public void WriteDialog()
    {
        _finishedTime = 0f;
        TextNode.Visible = true;
        TextNode.VisibleCharacters = 0;
        TextNode.Text = _currentText;
        _displayingText = true;
    }

    public void FinishHideDialog()
    {
        TextNode.Text = _currentText;
        _displayedPercentage = 0f;
        _displayingText = false;
        _showingAllText = false;
        FinishedCurrentText = true;
        TextParent.Visible = false;
        _finishedDialog = true;
    }

    public void DisplayDialog(GodotObject sender)
    {
        GD.Print($"[{this as GodotObject}]dialog from: " + sender);
        _useDelay = false;
        _finishedDialog = false;
        DisplayDialogUI();
    }

    public void DisplayDialogWithDelay(float delay, GodotObject sender)
    {
        GD.Print($"[{this as GodotObject}]delayed dialog from: {sender}. delayed for {delay} seconds");
        _useDelay = true;
        _delay = delay;
        _finishedDialog = false;
        DisplayDialogUI();
    }

    private void LoadTextFile()
    {
        if (String.IsNullOrWhiteSpace(DialogFile) == true || ResourceLoader.Exists(DialogFile) == false)
        {
            GD.Print("No dialog file, or does not exist.");
            return;
        }
        using (FileAccess file = FileAccess.Open(DialogFile, FileAccess.ModeFlags.Read))
        {
            string[] lines = System.Text.Json.JsonSerializer.Deserialize<string[]>(file.GetAsText());
            GD.Print(lines[0]);
            foreach (string item in lines)
            {
                _lines.Enqueue(item);
            }
        }
    }

    public override void _Input(InputEvent e)
    {
        if (e.IsActionReleased("Use"))
        {
            if (_displayingText == true)
            {
                DisplayDialogUI();
            }
        }
    }
    public bool FinishedDialog { get => _finishedDialog; set => _finishedDialog = value; }

}
