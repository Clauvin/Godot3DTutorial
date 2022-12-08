using Godot;
using System;

public class Main : Node
{
    // Don't forget to rebuild the project so the editor knows about the new export variable.

#pragma warning disable 649
    // We assign this in the editor, so we don't need the warning about not being assigned.
    [Export]
    public PackedScene MobScene;
#pragma warning restore 649

    public override void _Ready()
    {
        GD.Randomize();
        GetNode<Control>("UserInterface/Retry").Hide();
    }

    // We also specified this function name in PascalCase in the editor's connection window
    public void OnMobTimerTimeout()
    {
        // Create a new instance of the Mob scene.
        Mob mob = (Mob)MobScene.Instance();

        // Choose a random location on the SpawnPath.
        // We store the reference to the SpawnLocation node.
        var mobSpawnLocation = GetNode<PathFollow>("SpawnPath/SpawnLocation");
        // And give it a random offset.
        mobSpawnLocation.UnitOffset = GD.Randf();

        Vector3 playerPosition = GetNode<Player>("Player").Transform.origin;
        mob.Initialize(mobSpawnLocation.Translation, playerPosition);

        AddChild(mob);

        // ...
        // We connect the mob to the score label to update the score upon squashing one.
        mob.Connect(nameof(Mob.Squashed), GetNode<ScoreLabel>("UserInterface/ScoreLabel"), nameof(ScoreLabel.OnMobSquashed));
    }

    // We also specified this function name in PascalCase in the editor's connection window
    public void OnPlayerHit()
    {
        GetNode<Timer>("MobTimer").Stop();
        GetNode<Control>("UserInterface/Retry").Show();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_accept") && GetNode<Control>("UserInterface/Retry").Visible)
        {
            // This restarts the current scene.
            GetTree().ReloadCurrentScene();
        }
    }
}
