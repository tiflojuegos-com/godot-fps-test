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
        deskinstance = desksound.play3d(Transform.origin.x, Transform.origin.y, Transform.origin.z, loopMode.simpleLoop);
        deskinstance.maxDistance = 50;
        deskinstance.minDistance = 0.1f;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        //deskinstance.x =  surface.Transform.origin.x;
//deskinstance.y = surface.Transform.origin.y;
        //deskinstance.z = surface.Transform.origin.z;
              }

}