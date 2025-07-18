using Godot;

namespace Lastdew
{
	public partial class ScavengingLevel : Level
	{
		[Export]
		private int EnemiesToSpawn { get; set; } = 1;
		private EnemySpawner EnemySpawner { get; set; }
		
		public override Vector3 Initialize(TeamData teamData)
		{
			EnemySpawner = GetNode<EnemySpawner>("%EnemySpawner");
			EnemySpawner.Initialize(teamData, EnemiesToSpawn);

			return base.Initialize(teamData);
		}
	}
}
