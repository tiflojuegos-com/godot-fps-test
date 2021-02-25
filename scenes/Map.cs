            using Godot;
using System;
using tfj.exploudEngine;

public class Map : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private eSound stepsound;
    private eInstance stepinstance;
    private Random random = new Random();
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    public void playstep()
    {
        stepsound = Globals.engine.loadSound("sounds/carpet/" + random.Next(0, 9-1) + ".ogg");
        stepinstance = stepsound.play(0, loopMode.noLoop);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
