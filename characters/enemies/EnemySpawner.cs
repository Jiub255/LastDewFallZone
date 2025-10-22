using Godot;

namespace Lastdew
{
	public partial class EnemySpawner : Node3D
	{	
		private int EnemiesToSpawn { get; set; }
		/// <summary>
		/// TODO: Might not need this here.
		/// </summary>
		private TeamData TeamData { get; set; }
		private static float TimeBetweenSpawns => 2f;
		private float Timer { get; set; } = 0.25f;
		private PackedScene EnemyScene { get; } = GD.Load<PackedScene>(UIDs.TEST_ENEMY);
		
		public void Initialize(TeamData teamData, int enemiesToSpawn)
		{
			TeamData = teamData;
			EnemiesToSpawn = enemiesToSpawn;
		}

		public override void _Process(double delta)
		{
			base._Process(delta);

			if (EnemiesToSpawn > 0)
			{
				Timer -= (float)delta;
				if (Timer < 0)
				{
					Timer = TimeBetweenSpawns;
					SpawnEnemy();
				}
			}
		}
		
		private void SpawnEnemy()
		{
			EnemiesToSpawn--;
			Enemy enemy = EnemyScene.Instantiate<Enemy>();
			CallDeferred(Node.MethodName.AddChild, enemy);
			// if (TeamData.Pcs.Count > 0)
			// {
			// 	enemy.SetDeferred(Enemy.PropertyName.Target, TeamData.Pcs[0]);
			// }
			// else
			// {
			// 	GD.PushWarning("Couldn't find target PC for enemy");
			// }
		}
	}
}
