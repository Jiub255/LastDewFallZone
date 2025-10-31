using Godot;

namespace Lastdew
{
	public partial class EnemySpawner : Node3D
	{	
		private const float TIME_BETWEEN_SPAWNS = 2f;
		private int EnemiesToSpawn { get; set; }
		private float Timer { get; set; }
		private PackedScene EnemyScene { get; } = GD.Load<PackedScene>(UiDs.TEST_ENEMY);
		
		public void Initialize(int enemiesToSpawn)
		{
			EnemiesToSpawn = enemiesToSpawn;
		}

		public override void _Process(double delta)
		{
			base._Process(delta);

			if (EnemiesToSpawn <= 0)
			{
				return;
			}
			Timer += (float)delta;
			if (Timer > TIME_BETWEEN_SPAWNS)
			{
				Timer = 0;
				SpawnEnemy();
			}
		}
		
		private void SpawnEnemy()
		{
			EnemiesToSpawn--;
			Enemy enemy = EnemyScene.Instantiate<Enemy>();
			CallDeferred(Node.MethodName.AddChild, enemy);
		}
	}
}
