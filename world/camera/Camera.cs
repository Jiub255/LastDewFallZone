using Godot;

namespace Lastdew
{	
	public partial class Camera : Node3D
	{
		public ClickHandler ClickHandler { get; private set; }
		
		[Export(PropertyHint.Range, "5, 50, 1")]
		private float MovementSpeed { get; set; } = 50f;
		
		[ExportGroup("Keyboard Movement")]
		[Export(PropertyHint.Range, "5, 50, 1")]
		private float MovementSensitivity { get; set; } = 15f;
		
		[ExportGroup("Edge Scrolling")]
		[Export(PropertyHint.Range, "5, 50, 1")]
		private float ScrollSpeed { get; set; } = 15f;
		[Export(PropertyHint.Range, "0, 50, 0.1")]
		private float ScrollZoneThicknessPct { get; set; } = 10f;
		
		[ExportGroup("Mouse Dragging")]
		[Export(PropertyHint.Range, "0.05, 5, 0.05")]
		private float DragSpeed { get; set; } = 0.35f;
		
		[ExportGroup("Rotation")]
		[Export(PropertyHint.Range, "0, 30, 1")]
		private float RotationMinX { get; set; } = 15f;
		[Export(PropertyHint.Range, "60, 90, 1")]
		private float RotationMaxX { get; set; } = 75f;
		// TODO: Convert these rotation speeds from radians per pixel or whatever to something that makes more sense.
		[Export(PropertyHint.Range, "0.0001, 0.1, 0.0001")]
		private float RotationSpeedX { get; set; } = 0.005f;
		[Export(PropertyHint.Range, "0.0001, 0.1, 0.0001")]
		private float RotationSpeedY { get; set; } = 0.005f;
		
		[ExportGroup("Zoom")]
		[Export(PropertyHint.Range, "0, 100, 0.001")]
		private float ZoomSensitivity { get; set; } = 1f;
		[Export(PropertyHint.Range, "0, 100, 0.001")]
		private float ZoomSpeed { get; set; } = 10f;
		[Export(PropertyHint.Range, "0, 15, 0.1")]
		private float ZoomMinDistance { get; set; } = 10f;
		[Export(PropertyHint.Range, "16, 255, 0.1")]
		private float ZoomMaxDistance { get; set; } = 50f;
		[Export(PropertyHint.Range, "5, 35, 0.1")]
		private float ZoomDefaultDistance { get; set; } = 15f;
		
		private Vector3 Forward { get; set; }
		private Vector3 Right { get; set;}
		private Vector3 TargetPosition { get; set;}
		private float ScreenWidth { get; set; }
		private float ScreenHeight { get; set; }
		private float EdgeDistance { get; set; }
		private bool Dragging { get; set; }
		private Viewport Viewport { get; set; }
		
		private OuterGimbal OuterGimbal { get; set; }
		private InnerGimbal InnerGimbal { get; set; }
		private Zoomer Zoomer { get; set; }
	
		// TODO: Move all export vars from children and put here, then just pass them down in initialize methods.
	
		public override void _Ready()
		{
			base._Ready();
	
			ClickHandler = GetNode<ClickHandler>("%ClickHandler");
			Viewport = GetViewport();
			OuterGimbal = GetNode<OuterGimbal>("%OuterGimbal");
			InnerGimbal = GetNode<InnerGimbal>("%InnerGimbal");
			Zoomer = GetNode<Zoomer>("%Zoomer");
	
			OuterGimbal.Initialize(RotationSpeedY);
			InnerGimbal.Initialize(RotationMinX, RotationMaxX, RotationSpeedX);
			Zoomer.Initialize(
				ZoomSensitivity,
				ZoomSpeed,
				ZoomMinDistance,
				ZoomMaxDistance,
				ZoomDefaultDistance
			);
	
			TargetPosition = Position;
			SetScreenSize();
			GetTree().Root.Connect(
				Window.SignalName.SizeChanged,
				Callable.From(SetScreenSize));
			SetMovementBasisVectors();
		}
	
		public override void _Process(double delta)
        {
            base._Process(delta);

            if (Input.IsActionJustReleased(InputNames.CAMERA_DRAG))
            {
                Dragging = false;
            }

            if (Input.IsActionPressed(InputNames.CAMERA_ROTATE))
            {
                return;
            }

            SetMovementBasisVectors();
			
            if (Input.IsActionJustPressed(InputNames.CAMERA_DRAG))
            {
                Dragging = true;
            }

			// TODO: Fix the edge scroll logic here. MoveCamera still needs to be called when edge scrolling.
            if (!Dragging/*  && !MoveTargetPositionWithEdgeScroll((float)delta) */)
            {
                MoveTargetPositionWithKeyboard((float)delta);
                MoveCamera(delta);
            }
        }

        public override void _Input(InputEvent @event)
		{
			base._Input(@event);
			
			if (@event is InputEventMouseMotion motionEvent)
			{
				if (Input.IsActionPressed(InputNames.CAMERA_ROTATE))
				{
					OuterGimbal.RotateHorizontal(motionEvent);
					InnerGimbal.RotateVertical(motionEvent);
				}
				else if (Dragging)
				{
					MoveWithMouseDragFollow(motionEvent);
				}
			}
			else if (@event is InputEventMouseButton mouseButtonEvent)
			{
				if (mouseButtonEvent.ButtonIndex == MouseButton.WheelUp)
				{
					Zoomer.ZoomIn();
				}
				else if (mouseButtonEvent.ButtonIndex == MouseButton.WheelDown)
				{
					Zoomer.ZoomOut();
				}
			}
		}
	
		private void SetScreenSize()
		{
			Vector2 screenSize = Viewport.GetVisibleRect().Size;
			ScreenWidth = screenSize.X;
			ScreenHeight = screenSize.Y;
			EdgeDistance = ScreenHeight * (ScrollZoneThicknessPct / 100f);
		}
		
		private void SetMovementBasisVectors()
		{
			Forward = -OuterGimbal.Transform.Basis.Z;
			Right = OuterGimbal.Transform.Basis.X;
	
			// Project vectors onto the horizontal (xz) plane.
			Forward = new Vector3(Forward.X, 0, Forward.Z);
			Right = new Vector3(Right.X, 0, Right.Z);
	
			Forward = Forward.Normalized();
			Right = Right.Normalized();
		}
		
		/* private void MoveWithMouseDrag(InputEventMouseMotion motionEvent)
		{
			Vector2 dragMovement = motionEvent.Relative;
			Vector3 movement = (Forward * dragMovement.Y) + (Right * -dragMovement.X);
			movement = movement.Normalized();
			TargetPosition = Position + (movement * DragSpeed);
		} */
		
		private void MoveWithMouseDragFollow(InputEventMouseMotion motionEvent)
		{
            // Get start and end points of mouse movement.
            Vector2 endpoint = motionEvent.Position;
            Vector2 startpoint = endpoint - motionEvent.Relative;
            // Translate them to world space at whatever constant z-value. Use raycast?
            // Get the vector difference between the world space points.
            Vector3 startPointWorld = ScreenToWorldPoint(startpoint);
            Vector3 endPointWorld = ScreenToWorldPoint(endpoint);
			if (startPointWorld == Vector3.Zero || endPointWorld == Vector3.Zero)
			{
                return;
            }
            Vector3 difference = startPointWorld - endPointWorld;
            // Move the camera using the opposite vector.
            Position += difference;
        }

		private Vector3 ScreenToWorldPoint(Vector2 screenPoint)
		{
			PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
			// TODO: Cache camera in ready.
            Camera3D camera = GetNode<Camera3D>("%Camera3D");
            int rayLength = 1000;

            Vector3 origin = camera.ProjectRayOrigin(screenPoint);
			Vector3 end = origin + camera.ProjectRayNormal(screenPoint) * rayLength;
			PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(origin, end);
            query.CollisionMask = 0b10000;
            //query.CollideWithAreas = true;

            Godot.Collections.Dictionary result = spaceState.IntersectRay(query);
			// TODO: Make this safer. Make sure it collided first.
            return result.ContainsKey("position") ? (Vector3)result["position"] : Vector3.Zero;
        }
		
		/// <returns>true if mouse in edge scrolling zone.</returns>
		private bool MoveTargetPositionWithEdgeScroll(float delta)
		{
			Vector2 mousePosition = Viewport.GetMousePosition();
			Vector2 mouseMovement = Vector2.Zero;
			
			if (mousePosition.X > ScreenWidth - EdgeDistance)
			{
				mouseMovement = new Vector2(1, mouseMovement.Y);
			}
			else if (mousePosition.X < EdgeDistance)
			{
				mouseMovement = new Vector2(-1, mouseMovement.Y);
			}
			
			if (mousePosition.Y > ScreenHeight - EdgeDistance)
			{
				mouseMovement = new Vector2(mouseMovement.X, -1);
			}
			else if (mousePosition.Y < EdgeDistance)
			{
				mouseMovement = new Vector2(mouseMovement.X, 1);
			}
			
			if (mouseMovement != Vector2.Zero)
			{
				Vector3 movement = (Forward * mouseMovement.Y) + (Right * mouseMovement.X);
				movement = movement.Normalized();
				TargetPosition = Position + (movement * ScrollSpeed * delta);
				return true;
			}
			return false;
		}
		
		private void MoveTargetPositionWithKeyboard(float delta)
		{
			float x = Input.GetAxis(InputNames.CAMERA_LEFT, InputNames.CAMERA_RIGHT);
			float y = Input.GetAxis(InputNames.CAMERA_BACKWARD, InputNames.CAMERA_FORWARD);
			Vector3 movement = (Forward * y) + (Right * x);
			movement = movement.Normalized();
			TargetPosition = Position + (movement * MovementSensitivity * delta);
		}

        private void MoveCamera(double delta)
        {
            Position = Position.MoveToward(TargetPosition, MovementSpeed * (float)delta);
        }
	}
}
