using Godot;

namespace Lastdew
{
	public partial class Enemy : CharacterBody3D
	{
        public const float TURN_SPEED = 360f;
		private const string GETTING_HIT_ANIM_NAME = "CharacterArmature|HitRecieve_2";
		private const string BLEND_AMOUNT_PATH = "parameters/movement_blend_tree/idle_move/blend_amount";
        private const int ATTACK = 10;

        public EnemyTarget Target { get; set; }
        public Vector3 LastTargetPosition { get; set; }
        public NavigationAgent3D NavigationAgent { get; set; }
		public AnimationNodeStateMachinePlayback AnimStateMachine { get; set; }

        private int Health { get; set; } = 5;
        private EnemyStateMachine StateMachine { get; set; }
		private AnimationTree EnemyAnimationTree { get; set; }

		public override void _Ready()
		{
			base._Ready();

			NavigationAgent = GetNode<NavigationAgent3D>("%NavigationAgent3D");
			AnimStateMachine = (AnimationNodeStateMachinePlayback)GetNode<AnimationTree>("%AnimationTree").Get("parameters/playback");
			EnemyAnimationTree = GetNode<AnimationTree>("%AnimationTree");
	        
			StateMachine = new EnemyStateMachine(this);
		}
		
		public override void _Process(double delta)
		{
			base._Process(delta);
			
			StateMachine.ProcessState((float)delta);
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			
			StateMachine.ExitTree();
		}

		public void SetBlendAmount(float blendAmount)
		{
            EnemyAnimationTree.Set(BLEND_AMOUNT_PATH, blendAmount);
		}
		
		/// <returns>true if hit killed enemy</returns>
		public bool GetHit(int damage, PlayerCharacter attackingPc)
		{
			Health -= damage;
            this.PrintDebug($"{Name} hit for {damage} damage, health: {Health}");
            if (Health <= 0)
			{
				Health = 0;
				Die();
				return true;
			}
			// TODO: Not sure if this does anything at the moment. When is Target null?
			// Figure it out later.
			//Target /*??*/= attackingPc;
			AnimStateMachine.Travel(GETTING_HIT_ANIM_NAME);
			return false;
		}

		// Called from animation method track
		// Does it need to be public?
		public void HitTarget()
		{
			bool pcIncapacitated = Target.Pc.GetHit(this, ATTACK);
			if (pcIncapacitated)
			{
				StateMachine.ChangeState(EnemyStateNames.IDLE);
            }
		}

		// TODO: Inform all PCs that this enemy is dead, so they look for a new target or set it to null.
		private void Die()
		{
			StateMachine.ChangeState(EnemyStateNames.DEAD);
		}
    }
}
