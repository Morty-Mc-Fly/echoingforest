using Godot;
using System;


public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity =7.5f;
	public const float CamSensitivity = 0.003f;
	private float gravity = (float) ProjectSettings.GetSetting("physics/3d/default_gravity");
	private Vector3 velocity = Vector3.Zero;
	
	private Node3D _head;
	private Camera3D _cam;



	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
		_head = GetNode<Node3D>("Head");
		_cam = GetNode<Camera3D>("Head/Camera3D");
	}	
	
	
	public override void _Input(InputEvent @event)
	{
		if(@event is InputEventMouseMotion m)
		{
			_head.RotateY(-m.Relative.X * CamSensitivity);
			_cam.RotateX(-m.Relative.Y * CamSensitivity);
			
			Vector3 camRot = _cam.Rotation;
			camRot.X = Mathf.Clamp(camRot.X,Mathf.DegToRad(-80f), Mathf.DegToRad(80f));
			_cam.Rotation = camRot;
		}
		
		else if(@event is InputEventKey k && k.Keycode == Key.Escape)
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector3 direction = velocity;

	
		if(!IsOnFloor())
		{
			velocity.Y -= gravity * (float)delta;		
		}
		//handle Jumping
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		Vector2 inputDir = Input.GetVector("move left", "move right", "move forwards", "move backwards");
		direction = (_head.GlobalTransform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
	
		
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
		
		
	}
}
