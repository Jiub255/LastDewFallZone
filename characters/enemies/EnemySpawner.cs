using Godot;

namespace Lastdew
{
	public partial class EnemySpawner : Node3D
	{
		[Export]
		private float TimeBetweenSpawns { get; set; } = 5f;
		[Export]
		private double StartDelay { get; set; } = 5f;
		[Export]
		private int EnemiesToSpawn { get; set; } = 1;
		private float Timer { get; set; }
		private PackedScene EnemyScene { get; } = GD.Load<PackedScene>(Uids.TEST_ENEMY);

		
		public override void _Process(double delta)
		{
			base._Process(delta);

			if (EnemiesToSpawn <= 0)
			{
				return;
			}

			StartDelay -= delta;
			if (StartDelay > 0)
			{
				return;
			}
			
			Timer -= (float)delta;
			if (Timer < 0)
			{
				Timer = TimeBetweenSpawns;
				SpawnEnemy();
			}
		}
		
		private void SpawnEnemy()
		{
			EnemiesToSpawn--;
			Enemy enemy = EnemyScene.Instantiate<Enemy>();
			this.AddChildDeferred(enemy);
		}
	}
}
