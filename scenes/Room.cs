    using Godot;
using System;
using tfj.exploudEngine;

public class Room : CSGBox
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private eSound stepsound;
    private eInstance stepinstance;
    private Random random;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        random = new Random();
    }

    public void playstep()
    {
        stepsound = Globals.engine.loadSound("sounds/carpet/" + random.Next(1, 9) + ".ogg");
        stepinstance = stepsound.play(0, loopMode.noLoop);
    }


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
