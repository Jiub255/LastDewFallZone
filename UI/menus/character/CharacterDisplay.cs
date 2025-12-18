using Godot;

namespace Lastdew
{
	public partial class CharacterDisplay : VBoxContainer
	{
		private RichTextLabel NameLabel { get; set; }
		private TextureRect CharacterIcon { get; set; }
		private RichTextLabel StatsLabel { get; set; }
		private TeamData TeamData { get; set; }
		private SfxButton Previous { get; set; }
		private SfxButton Next { get; set; }

		public override void _Ready()
		{
			base._Ready();
			
			NameLabel = GetNode<RichTextLabel>("%NameLabel");
			CharacterIcon = GetNode<TextureRect>("%CharacterIcon");
			StatsLabel = GetNode<RichTextLabel>("%StatsLabel");
			Previous = GetNode<SfxButton>("%PreviousButton");
			Next = GetNode<SfxButton>("%NextButton");

			Previous.Pressed += PreviousPc;
			Next.Pressed += NextPc;
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			
			Previous.Pressed -= PreviousPc;
			Next.Pressed -= NextPc;
			if (TeamData == null)
			{
				return;
			}
			TeamData.OnMenuSelectedChanged -= Setup;
		}

		public void Initialize(TeamData teamData)
		{
			TeamData = teamData;
			teamData.OnMenuSelectedChanged += Setup;
		}

		public void Setup()
		{
			PlayerCharacter pc = TeamData.Pcs[TeamData.MenuSelectedIndex];
			NameLabel.Text = pc.Data.Name;
			CharacterIcon.Texture = pc.Data.Icon;
			SetupStatsLabel(pc);
			pc.OnEquipmentChanged += Setup;
		}

		private void SetupStatsLabel(PlayerCharacter pc)
		{
			string statText =  $"Injury: {pc.StatManager.Health.Injury}\n"
			+ $"Pain: {pc.StatManager.Health.Pain}\n"
			+ $"Attack: {pc.StatManager.Attack}\n"
			+ $"Defense: {pc.StatManager.Defense}\n"
			+ $"Engineering: {pc.StatManager.Engineering}\n"
			+ $"Farming: {pc.StatManager.Farming}\n"
			+ $"Medical: {pc.StatManager.Medical}\n"
			+ $"Scavenging: {pc.StatManager.Scavenging}";
			
			StatsLabel.Text = statText;
		}
		
		private void PreviousPc()
		{
			PlayerCharacter pc = TeamData.Pcs[TeamData.MenuSelectedIndex];
			pc.OnEquipmentChanged -= Setup;
			if (TeamData.MenuSelectedIndex == 0)
			{
				TeamData.MenuSelectedIndex = TeamData.Pcs.Count - 1;
			}
			else
			{
				TeamData.MenuSelectedIndex--;
			}
		}
		
		private void NextPc()
		{
			PlayerCharacter pc = TeamData.Pcs[TeamData.MenuSelectedIndex];
			pc.OnEquipmentChanged -= Setup;
			if (TeamData.MenuSelectedIndex == TeamData.Pcs.Count - 1)
			{
				TeamData.MenuSelectedIndex = 0;
			}
			else
			{
				TeamData.MenuSelectedIndex++;
			}
		}
	}
}
