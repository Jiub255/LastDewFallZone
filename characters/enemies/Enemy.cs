using System;
using Godot;

namespace Lastdew
{
	public partial class Enemy : CharacterBody3D
	{
		public event Action OnDeath;
		
		private EnemyTarget _target;
		
		private const string GETTING_HIT_ANIM_NAME = "CharacterArmature|HitRecieve_2";
		private const string BLEND_AMOUNT_PATH = "parameters/movement_blend_tree/idle_move/blend_amount";
		
        [Export]
		public EnemyData Data { get; private set; }

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

        public int Health { get; private set; }
        public Vector3 LastTargetPosition { get; set; }
        public NavigationAgent3D NavigationAgent { get; private set; }
		public AnimationNodeStateMachinePlayback AnimStateMachine { get; private set; }

        private EnemyStateMachine StateMachine { get; set; }
		private AnimationTree EnemyAnimationTree { get; set; }

		private AudioStreamPlaybackPolyphonic AudioPlayback { get; set; }
		private AudioStream Punch1 { get; } = GD.Load<AudioStream>(Sfx.PUNCH_1);
		private AudioStream Death1 { get; } = GD.Load<AudioStream>(Sfx.DEATH_1);


		public override void _Ready()
		{
			base._Ready();
			
			AudioStreamPlayer3D audioStreamPlayer = GetNode<AudioStreamPlayer3D>("%AudioStreamPlayer3D");
			audioStreamPlayer.Play();
			audioStreamPlayer.Bus = new StringName("Combat");
			AudioPlayback = (AudioStreamPlaybackPolyphonic)audioStreamPlayer.GetStreamPlayback();

			NavigationAgent = GetNode<NavigationAgent3D>("%NavigationAgent3D");
			AnimStateMachine = (AnimationNodeStateMachinePlayback)GetNode<AnimationTree>("%AnimationTree").Get("parameters/playback");
			EnemyAnimationTree = GetNode<AnimationTree>("%AnimationTree");
	        
			StateMachine = new EnemyStateMachine(this);
			
			Health = Data.MaxHealth;
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
			int actualDamage = Math.Max(0, damage - Data.Defense);
			Health -= actualDamage;
            if (Health <= 0)
			{
				Health = 0;
				Die();
				return true;
			}
			AnimStateMachine.Travel(GETTING_HIT_ANIM_NAME);
            
			// this.PrintDebug($"{Data.EnemyType} {Name} getting hit by {attackingPc.Name} for {actualDamage} damage.\n" +
			//                 $"Health: {Health}");
			
			// TODO: Have Enemy switch targets when hit? Maybe have a chance it happens?
			// Also, change to combat state?
			
			// Target = new EnemyTarget(
			// 	attackingPc,
			// 	Vector3.Forward.Rotated(Vector3.Up, GD.Randf() * Mathf.Tau));
			
			return false;
		}

		// Called from animation method track
		private void HitTarget()
		{
			AudioPlayback.PlayStream(Punch1);
			Target?.Pc.GetHit(this, Data.Attack);
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
			AudioPlayback.PlayStream(Death1);
			StateMachine.ChangeState(EnemyStateNames.DEAD);
			OnDeath?.Invoke();
		}
    }
}
