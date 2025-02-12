using Godot;

namespace Lastdew
{
	/// <summary>
	/// This whole class is a hacky fix to the inventory being populated weird
	/// after crafting an item because of multiple PopulateInventoryUI() calls in one frame.
	/// The call deferred was messing things up.
	/// </summary>
	public partial class GameMenuTabs : TabContainer
	{
		private CharacterTab CharacterTab { get; set; }
		
		public override void _Ready()
		{
			CharacterTab = GetNode<CharacterTab>("%Character");

			TabChanged += OnTabChanged;
		}

		public override void _ExitTree()
		{
			TabChanged -= OnTabChanged;
		}
		
		private void OnTabChanged(long tabIndex)
		{
			if (tabIndex == 0)
			{
				CharacterTab.OnOpen();
			}
		}
	}
}
