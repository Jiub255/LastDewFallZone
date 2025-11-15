using Godot;
using System;
using System.Collections.Generic;

namespace Lastdew
{
	public partial class CharacterSelector : VBoxContainer
	{
		private int _index;
		
		public Button Previous { get; private set; }
		public Button Next { get; private set; }
	
		private RichTextLabel NameLabel { get; set; }
		private TextureRect CharacterIcon { get; set; }
		private RichTextLabel StatsLabel { get; set; }
		private List<PlayerCharacter> UnselectedPcs { get; set; }

		public override void _Ready()
		{
			base._Ready();
			
			NameLabel = GetNode<RichTextLabel>("%NameLabel");
			CharacterIcon = GetNode<TextureRect>("%CharacterIcon");
			StatsLabel = GetNode<RichTextLabel>("%StatsLabel");
			Previous = GetNode<Button>("%PreviousButton");
			Next = GetNode<Button>("%NextButton");
		}

		public void Initialize(List<PlayerCharacter> unselectedPcs)
		{
			UnselectedPcs = unselectedPcs;
			SetupDisplay(0);
		}

		public void SetupDisplay(int index)
		{
			PlayerCharacter pc = UnselectedPcs[index];
			NameLabel.Text = pc.Data.Name;
			CharacterIcon.Texture = pc.Data.Icon;
			SetupStatsLabel(pc);
		}

		private void SetupStatsLabel(PlayerCharacter pc)
		{
			string statText =  $"Injury: {pc.Health.Injury}\n"
			+ $"Pain: {pc.Health.Pain}\n"
			+ $"Attack: {pc.StatManager.Attack}\n"
			+ $"Defense: {pc.StatManager.Defense}\n"
			+ $"Engineering: {pc.StatManager.Engineering}\n"
			+ $"Farming: {pc.StatManager.Farming}\n"
			+ $"Medical: {pc.StatManager.Medical}\n"
			+ $"Scavenging: {pc.StatManager.Scavenging}";
			
			StatsLabel.Text = statText;
		}
	}
}
