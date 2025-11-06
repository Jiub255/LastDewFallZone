using System;
using Godot;

namespace Lastdew
{
	public partial class Enemy : CharacterBody3D
	{
		public event Action OnDeath;
		
		private EnemyTarget _target;
		
		public const float TURN_SPEED = 360f;
		private const string GETTING_HIT_ANIM_NAME = "CharacterArmature|HitRecieve_2";
		private const string BLEND_AMOUNT_PATH = "parameters/movement_blend_tree/idle_move/blend_amount";
		// TODO: Make EnemyData resource and get attack, speed, etc. from that.
        private const int ATTACK = 10;

        public EnemyTarget Target
        {
	        get => _target;
	        set
	        {
		        if (_target?.Pc != null) _target.Pc.OnDeath -= ChangeToIdleState;
		        _target = value;
		        if (_target?.Pc != null) _target.Pc.OnDeath += ChangeToIdleState;
	        }
        }

        public Vector3 LastTargetPosition { get; set; }
        public NavigationAgent3D NavigationAgent { get; private set; }
		public AnimationNodeStateMachinePlayback AnimStateMachine { get; private set; }
        public int Health { get; private set; } = 5;

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
		
		public void GetHit(int damage, PlayerCharacter attackingPc)
		{
			Health -= damage;
            if (Health <= 0)
			{
				Health = 0;
				Die();
				return;
			}
			AnimStateMachine.Travel(GETTING_HIT_ANIM_NAME);
            
			//this.PrintDebug($"{Name} getting hit by {attackingPc.Name}");
			
			// TODO: Have Enemy switch targets when hit? Maybe have a chance it happens?
			// Also, change to combat state?
			
			// Target = new EnemyTarget(
			// 	attackingPc,
			// 	Vector3.Forward.Rotated(Vector3.Up, GD.Randf() * Mathf.Tau));
		}

		// Called from animation method track
		private void HitTarget()
		{
			Target?.Pc.GetHit(this, ATTACK);
		}

		private void ChangeToIdleState()
		{
			if (Health > 0)
			{
				StateMachine.ChangeState(EnemyStateNames.IDLE);
			}
		}

		private void Die()
		{
			StateMachine.ChangeState(EnemyStateNames.DEAD);
			OnDeath?.Invoke();
		}
    }
}
