using Godot;
using System;
using tfj.exploudEngine;

public class desk : CSGCombiner
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private eSound desksound;
    private eInstance deskinstance;
    private CSGBox surface;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        desksound = Globals.engine.loadSound("sounds/desk.ogg");
        surface = GetNode<CSGBox>("surface");
        deskinstance = desksound.play3d(surface.Translation.x, surface.Translation.y, surface.Translation.z, loopMode.simpleLoop);
        deskinstance.maxDistance = 100;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
