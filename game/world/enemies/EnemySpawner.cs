using Godot;

namespace Lastdew
{
	public partial class EnemySpawner : Node3D
	{
		[Export]
		private int EnemiesToSpawn { get; set; } = 1;
		private MissionTeamData MissionTeamData { get; set; }
		private float TimeBetweenSpawns { get; } = 2f;
		private float Timer { get; set; } = 0.25f;
		private PackedScene EnemyScene { get; } = GD.Load<PackedScene>("res://game/world/enemies/enemy_TEST.tscn");
		
		public void Initialize(MissionTeamData missionTeamData)
		{
			MissionTeamData = missionTeamData;
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
			CallDeferred(MethodName.AddChild, enemy);
			if (MissionTeamData.UnselectedPcs.Count > 0)
			{
				enemy.CallDeferred(Enemy.MethodName.SetTarget, MissionTeamData.UnselectedPcs[0]);
			}
			else if (MissionTeamData.SelectedPc != null)
			{
				enemy.CallDeferred(Enemy.MethodName.SetTarget, MissionTeamData.SelectedPc);
			}
			else
			{
				GD.PushWarning("Couldn't find target PC for enemy");
			}
		}
	}
}
