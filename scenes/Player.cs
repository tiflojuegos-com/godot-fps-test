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
    [Signal]
    public delegate void wallhit();
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
    private eSound wallsound;
    private eInstance wallinstance;
    private bool isMoving=true;
    private bool walltrigger;
    private Timer collisionTimer;

        // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Head = GetNode<Spatial>("Head");
        GroundCheck = GetNode<RayCast>("GroundCheck");
        collisionTimer = GetNode<Timer>("walltimer");
        jumpsound = Globals.engine.loadSound("sounds/duck.ogg");
        wallsound = Globals.engine.loadSound("sounds/wall.ogg");
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
    distance += HVelocity.Length();
    if(IsOnFloor()&&GroundCheck.IsColliding()&&!IsOnWall())
    {
if(isMoving==true&& HVelocity.Length() == 20)
        {
            EmitSignal("Walk");
}
else if (distance>=300)
        {
            EmitSignal("Walk");
            distance = 0;
            Console.WriteLine("Posicion z actual: " + Transform.origin.z);
        }
    }
        Globals.engine.listener.x = Transform.origin.x;
        Globals.engine.listener.y = Transform.origin.y+Head.Transform.origin.y;
        Globals.engine.listener.z = Transform.origin.z;
        Globals.engine.listener.velX = HVelocity.x;
        Globals.engine.listener.velY = Movement.y;
        Globals.engine.listener.velZ = HVelocity.z;
                CheckListenerRotation();
        CheckIfIsColliding();
    }

    public void CheckListenerRotation()
    {
        if(Mathf.Rad2Deg(Rotation.y)<=0)
        {
            Globals.engine.listener.rotation = (int)Mathf.Rad2Deg(Rotation.y)+180;
        }
else 
        {
            Globals.engine.listener.rotation = 180+  (int)Mathf.Rad2Deg(Rotation.y);
        }
    }

public void CheckIfIsColliding()
    {
if(IsOnWall()==true||IsOnFloor()==true&&IsOnWall()==true)
        {
if(walltrigger==true)
            {
                collisionTimer.Start();
                EmitSignal("wallhit");
                walltrigger = false;
            }
        }
else if(IsOnFloor()==true||!IsOnFloor()==true)
        {
            walltrigger = true;
        }
    }

    public void onWallhit()
    {
        wallinstance = wallsound.play();
        if(wallinstance.playing==true&&collisionTimer.IsStopped())
        {
            wallinstance.stop();
        }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
