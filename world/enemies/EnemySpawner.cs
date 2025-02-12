using Godot;

namespace Lastdew
{
	public partial class EnemySpawner : Node3D
	{
		[Export]
		private int EnemiesToSpawn { get; set; } = 1;
		private TeamData TeamData { get; set; }
		private float TimeBetweenSpawns { get; } = 2f;
		private float Timer { get; set; } = 0.25f;
		private PackedScene EnemyScene { get; } = GD.Load<PackedScene>("res://world/enemies/enemy_TEST.tscn");
		private bool Started { get; set; }
		
		public void Initialize(TeamData teamData)
		{
			TeamData = teamData;
			Started = true;
		}

		public override void _Process(double delta)
		{
			base._Process(delta);

			if (Started && EnemiesToSpawn > 0)
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
			CallDeferred(MethodName.AddChild, enemy);
			if (TeamData.Pcs.Count > 0)
			{
				enemy.CallDeferred(Enemy.MethodName.SetTarget, TeamData.Pcs[0]);
			}
			/* if (TeamData.UnselectedPcs.Count > 0)
			{
				enemy.CallDeferred(Enemy.MethodName.SetTarget, TeamData.UnselectedPcs[0]);
			}
			else if (TeamData.SelectedPc != null)
			{
				enemy.CallDeferred(Enemy.MethodName.SetTarget, TeamData.SelectedPc);
			}*/
			else
			{
				GD.PushWarning("Couldn't find target PC for enemy");
			} 
		}
	}
}
