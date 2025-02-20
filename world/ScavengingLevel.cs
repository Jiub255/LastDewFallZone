using Godot;

namespace Lastdew
{
	public partial class ScavengingLevel : Level
	{                           
		private EnemySpawner EnemySpawner { get; set; }
		
		public override Vector3 Initialize(TeamData teamData)
		{
			EnemySpawner = GetNode<EnemySpawner>("%EnemySpawner");
			EnemySpawner.Initialize(teamData);
			
			return base.Initialize(teamData);
		}
	}
}
