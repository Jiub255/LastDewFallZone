using Godot;

namespace Lastdew
{
	public partial class CharacterDisplay : VBoxContainer
	{
		private RichTextLabel NameLabel { get; set; }
		private TextureRect CharacterIcon { get; set; }
		private RichTextLabel StatsLabel { get; set; }
		private TeamData TeamData { get; set; }
		private Button Previous { get; set; }
		private Button Next { get; set; }

		public override void _Ready()
		{
			base._Ready();
			
			NameLabel = GetNode<RichTextLabel>("%NameLabel");
			CharacterIcon = GetNode<TextureRect>("%CharacterIcon");
			StatsLabel = GetNode<RichTextLabel>("%StatsLabel");
			Previous = GetNode<Button>("%PreviousButton");
			Next = GetNode<Button>("%NextButton");

			Previous.Pressed += PreviousPc;
			Next.Pressed += NextPc;
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			
			Previous.Pressed -= PreviousPc;
			Next.Pressed -= NextPc;
		}

		public void Initialize(TeamData teamData)
		{
			TeamData = teamData;
		}

		public void SetupDisplay(PlayerCharacter pc)
		{
			NameLabel.Text = pc.Name;
			CharacterIcon.Texture = pc.Icon;
			SetupStatsLabel(pc);
		}

		private void SetupStatsLabel(PlayerCharacter pc)
		{
			string statText = "";
			statText += $"Injury: {pc.Health.Injury}\n";
			statText += $"Pain: {pc.Health.Pain}\n";
			foreach (Stat stat in pc.StatManager)
			{
				statText += $"{stat.Type}: {stat.Value}\n";
			}
			StatsLabel.Text = statText;
		}
		
		private void PreviousPc()
		{
			TeamData.MenuSelectedIndex--;
			if (TeamData.MenuSelectedIndex < 0)
			{
				TeamData.MenuSelectedIndex = TeamData.Pcs.Count - 1;
			}
		}
		
		private void NextPc()
		{
			TeamData.MenuSelectedIndex++;
			if (TeamData.MenuSelectedIndex > TeamData.Pcs.Count - 1)
			{
				TeamData.MenuSelectedIndex = 0;
			}
		}
	}
}
