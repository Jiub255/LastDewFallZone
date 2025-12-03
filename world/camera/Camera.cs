using Godot;

namespace Lastdew
{	
	public partial class Camera : Node3D
	{
		public ClickHandler ClickHandler { get; private set; }
		
		[Export]
		private CameraSettings Settings { get; set; }
		
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
	
		public override void _Ready()
		{
			base._Ready();
	
			ClickHandler = GetNode<ClickHandler>("%ClickHandler");
			Viewport = GetViewport();
			OuterGimbal = GetNode<OuterGimbal>("%OuterGimbal");
			InnerGimbal = GetNode<InnerGimbal>("%InnerGimbal");
			Zoomer = GetNode<Zoomer>("%Zoomer");
	
			OuterGimbal.Initialize(Settings.RotationSpeedY);
			InnerGimbal.Initialize(Settings.RotationMinX, Settings.RotationMaxX, Settings.RotationSpeedX);
			Zoomer.Initialize(
				Settings.ZoomSensitivity,
				Settings.ZoomSpeed,
				Settings.ZoomMinDistance,
				Settings.ZoomMaxDistance,
				Settings.ZoomDefaultDistance
			);
	
			TargetPosition = Position;
			SetScreenSize();
			GetTree().Root.Connect(
				Viewport.SignalName.SizeChanged,
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

            if (!Dragging)
            {
				if (!Settings.EdgeScrollingEnabled || !MoveTargetPositionWithEdgeScroll((float)delta))
                {
					MoveTargetPositionWithKeyboard((float)delta);
				}
				MoveCamera(delta);
            }	
        }

        public override void _UnhandledInput(InputEvent @event)
        {
	        base._UnhandledInput(@event);

	        switch (@event)
	        {
		        case InputEventMouseMotion motionEvent when Input.IsActionPressed(InputNames.CAMERA_ROTATE):
			        OuterGimbal.RotateHorizontal(motionEvent);
			        InnerGimbal.RotateVertical(motionEvent);
			        break;
		        case InputEventMouseMotion motionEvent:
			        if (Dragging)
			        {
				        MoveWithMouseDragFollow(motionEvent);
			        }
			        break;
		        case InputEventMouseButton mouseButtonEvent:
			        switch (mouseButtonEvent.ButtonIndex)
			        {
				        case MouseButton.WheelUp:
					        Zoomer.ZoomIn();
					        break;
				        case MouseButton.WheelDown:
					        Zoomer.ZoomOut();
					        break;
			        }
			        break;
	        }
        }
	
		private void SetScreenSize()
		{
			Vector2 screenSize = Viewport.GetVisibleRect().Size;
			ScreenWidth = screenSize.X;
			ScreenHeight = screenSize.Y;
			EdgeDistance = ScreenHeight * (Settings.ScrollZoneThicknessPct / 100f);
		}
		
		private void SetMovementBasisVectors()
		{
			Forward = -OuterGimbal.Basis.Z;
			Right = OuterGimbal.Basis.X;
	
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
            Vector3 startpointWorld = ScreenToWorldPoint(startpoint);
            Vector3 endpointWorld = ScreenToWorldPoint(endpoint);
			if (startpointWorld == Vector3.Zero || endpointWorld == Vector3.Zero)
			{
                return;
            }
            Vector3 difference = startpointWorld - endpointWorld;
            // Move the camera using the opposite vector.
            Position += difference;
        }

		private Vector3 ScreenToWorldPoint(Vector2 screenPoint)
		{
			PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
			// TODO: Cache camera in ready.
            Camera3D camera = GetNode<Camera3D>("%Camera3D");
            const int rayLength = 1000;

            Vector3 origin = camera.ProjectRayOrigin(screenPoint);
			Vector3 end = origin + camera.ProjectRayNormal(screenPoint) * rayLength;
			PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(origin, end);
            query.CollisionMask = 0b10000;
            //query.CollideWithAreas = true;

            Godot.Collections.Dictionary result = spaceState.IntersectRay(query);
			// TODO: Make this safer. Make sure it collided first.
            return result.TryGetValue("position", out Variant worldPoint) ? (Vector3)worldPoint : Vector3.Zero;
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
				TargetPosition = Position + (movement * Settings.ScrollSpeed * delta);
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
			TargetPosition = Position + (movement * Settings.MovementSensitivity * delta);
		}

        private void MoveCamera(double delta)
        {
            Position = Position.MoveToward(TargetPosition, Settings.MovementSpeed * (float)delta);
        }
	}
}
