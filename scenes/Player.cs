using Godot;
using System;
using tfj.exploudEngine;

public class Player : KinematicBody
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Signal]
    public delegate void Walk();
private int speed=10;
    private float HAcceleration = 10;
    private float AirAcceleration = 1;
    private float NormalAcceleration = 6;
    private float Gravity = 20;
    private float Jump=10;
    private bool FullContact = false;
    private float MouseSensitivity = 0.3f;
    private Vector3 Direction;
    private Vector3 HVelocity;
    private Vector3 Movement;
    private Vector3 GravityVec;
    private Spatial Head;
    private RayCast GroundCheck;
    private float distance = 0;
    private eSound jumpsound;
    private eInstance jumpinstance;
    float angle;
        // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Head = GetNode<Spatial>("Head");
        GroundCheck = GetNode<RayCast>("GroundCheck");
        jumpsound = Globals.engine.loadSound("sounds/duck.ogg");
        Input.SetMouseMode(Input.MouseMode.Captured);
    }

    public override void _Input(InputEvent @event)
    {
if(@event is InputEventMouseMotion&& Input.GetMouseMode()==Input.MouseMode.Captured)
        {
            InputEventMouseMotion mouseEvent = @event as InputEventMouseMotion;
            RotateY(Mathf.Deg2Rad(-mouseEvent.Relative.x * MouseSensitivity));
            Head.RotateX(Mathf.Deg2Rad(-mouseEvent.Relative.y*MouseSensitivity));
            Vector3 HeadRot = Head.RotationDegrees;
            HeadRot.x = Mathf.Clamp(HeadRot.x, -89, 89);
            Head.RotationDegrees = HeadRot;
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        Direction = new Vector3();
        FullContact = GroundCheck.IsColliding()==true;
if(!IsOnFloor())
        {
            GravityVec += Vector3.Down * Gravity * delta;
            HAcceleration = AirAcceleration;
        }
else if(IsOnFloor()&&FullContact)
        {
            GravityVec = -GetFloorNormal() * Gravity;
            HAcceleration = NormalAcceleration;
        }
else
        {
            GravityVec = -GetFloorNormal();
            HAcceleration = NormalAcceleration;
        }
if(Input.IsActionJustPressed("Jump")&&IsOnFloor() && GroundCheck.IsColliding())
        {
            GravityVec = Vector3.Up * Jump;
            jumpinstance = jumpsound.play(0, loopMode.noLoop);
        }
if(Input.IsActionPressed("MoveForward"))
        {
            Direction -= Transform.basis.z;
        }
if(Input.IsActionPressed("MoveBackward"))
            {
            Direction += Transform.basis.z;
        }
if(Input.IsActionPressed("MoveLeft"))
            {
            Direction -= Transform.basis.x;
        }
        if (Input.IsActionPressed("MoveRight"))
                    {
            Direction += Transform.basis.x;
        }
        Direction = Direction.Normalized();
        HVelocity = HVelocity.LinearInterpolate(Direction * speed, HAcceleration * delta);
        Movement.z = HVelocity.z + GravityVec.z;
        Movement.x = HVelocity.x + GravityVec.x;
        Movement.y = GravityVec.y;
        MoveAndSlide(Movement, Vector3.Up);
        distance += Direction.Length();
        if(IsOnFloor()&&GroundCheck.IsColliding())
        {
            if(distance>=30)
            {
                EmitSignal("Walk");
                distance = 0;
                Console.WriteLine("Posicion actual: " + Transform.origin.x + ", " + Transform.origin.y + ", " + Transform.origin.z);
            }
        }
        Globals.engine.listener.x = Translation.x;
        Globals.engine.listener.y = Translation.y+Head.Translation.y;
        Globals.engine.listener.z = Translation.z;
        Globals.engine.listener.rotation =(int)Mathf.Rad2Deg(Rotation.y);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
