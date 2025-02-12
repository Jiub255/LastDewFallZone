using Godot;
using System;

namespace Lastdew
{
	[GlobalClass]
	public partial class Level : Node3D
	{
		private EnemySpawner EnemySpawner { get; set; }
		
		public override void _Ready()
		{
			EnemySpawner ??= GetNode<EnemySpawner>("%EnemySpawner");
		}
		
		public void Initialize(TeamData teamData)
		{
			EnemySpawner ??= GetNode<EnemySpawner>("%EnemySpawner");
			EnemySpawner.Initialize(teamData);
		}
	}
}
