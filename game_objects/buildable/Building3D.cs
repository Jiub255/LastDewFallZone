using Godot;

namespace Lastdew
{
	public partial class Building3D : Node3D
	{
		private const float RADIUS_INCREASE = 1f;
		
		private bool _overlapping;
		
		public MeshInstance3D MeshInstance { get; private set; }
		private Color OkColor { get; set; } = Colors.Green;
		private Color NoColor { get; set; } = Colors.Red;
		private BoxShape3D BoxShape { get; set; }

		public override void _Ready()
		{
			ProcessMode = ProcessModeEnum.Always;
			MeshInstance = GetNode<MeshInstance3D>("%MeshInstance3D");
			
			CollisionShape3D collisionShape = GetNode<CollisionShape3D>("%CollisionShape3D");
			// TODO: Just assuming box collision shapes for buildings for now.
			Setup((BoxShape3D)collisionShape.Shape);
		}
		
		public bool Overlapping
		{
			get => _overlapping;
			private set
			{
				if (_overlapping == value)
				{
					return;
				}

				SetColor(!value);
				_overlapping = value;
			}
		}

		public override void _PhysicsProcess(double delta)
		{
			base._Process(delta);
			Overlapping = IsOverlapping();
		}

		private void Setup(BoxShape3D shape)
		{
			SetupCastShape(shape);
			SetupIndicatorMesh();
			SetColor(!IsOverlapping());
		}

		private bool IsOverlapping()
		{
			return this.ShapeCast3D(BoxShape, 0b1).Count > 1;
		}

		private void SetupCastShape(BoxShape3D shape)
		{
			BoxShape3D biggerShape = (BoxShape3D)shape.Duplicate();
			biggerShape.Size = new Vector3(
				shape.Size.X + RADIUS_INCREASE * 2,
				3,
				shape.Size.Z + RADIUS_INCREASE * 2);
			BoxShape = biggerShape;
		}

		private void SetupIndicatorMesh()
		{
			BoxMesh boxMesh = (BoxMesh)MeshInstance.Mesh;
			boxMesh.Size = new Vector3(
				BoxShape.Size.X,
				0.01f,
				BoxShape.Size.Z);
		}

		private void SetColor(bool ok)
		{
			StandardMaterial3D material = (StandardMaterial3D)MeshInstance.Mesh.SurfaceGetMaterial(0);
			material.AlbedoColor = ok ? OkColor : NoColor;
		}
	}
}
