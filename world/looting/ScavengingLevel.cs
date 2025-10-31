using Godot;

namespace Lastdew
{
	public partial class ScavengingLevel : Level
	{
		[Export]
		private int EnemiesToSpawn { get; set; } = 1;
		private EnemySpawner EnemySpawner { get; set; }
		
		public override void Initialize(TeamData teamData)
		{
			EnemySpawner = GetNode<EnemySpawner>("%EnemySpawner");
			EnemySpawner.Initialize(EnemiesToSpawn);

			base.Initialize(teamData);
		}
	}
}
