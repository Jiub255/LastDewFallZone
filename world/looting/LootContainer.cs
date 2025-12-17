using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Lastdew
{	
	public partial class LootContainer : StaticBody3D
	{
		private List<Item> _commons = [];
		private List<Item> _uncommons = [];
		private List<Item> _rares = [];
		private List<Item> _uniques = [];
		
		// TODO: Do random loot based off rarity and tags eventually. Just hand filling item arrays for now.
		[Export] public Godot.Collections.Array<ItemTags> Tags { get; private set; } = [ItemTags.CRAFTING];
		//[Export]
		public Godot.Collections.Array<Item> Loot { get; private set; } = [];
		[Export]
		public float LootDuration { get; private set; } = 1f;
		// TODO: Have experience tied to loot duration? And somehow have quality of items tied to it too?
		// Maybe have quantity/quality of items be the choice, then base duration and experience off that.
		[Export]
		public int Experience { get; private set; } = 1;
		[Export]
		public int MinimumItemAmount { get; private set; } = 1;
		[Export]
		public int MaximumItemAmount { get; private set; } = 5;
		
		public bool Empty { get; set; }
		public bool BeingLooted { get; set; }
		public Vector3 LootingPosition { get; private set; }
		
		public override void _Ready()
		{
			base._Ready();
			
			LootingPosition = GetNode<Node3D>("%LootingPosition").GlobalPosition;
			PopulateLoot();
		}

		private void PopulateLoot()
		{
			Loot.Clear();
			
			GatherItems();

			Random rng = new();
			int numberOfItems = rng.Next(MinimumItemAmount, MaximumItemAmount + 1);
			for (int i = 0; i < numberOfItems; i++)
			{
				ChooseRandomItem();
			}
			
			// Not really necessary, just saving memory.
			_commons.Clear();
			_uncommons.Clear();
			_rares.Clear();
			_uniques.Clear();

			GD.Print($"Items added to loot container {Name}");
			foreach (Item item in Loot)
			{
				GD.Print($"{item.ItemRarity} {item.Name}");
			}
		}

		private void GatherItems()
		{
			_commons = GatherItemsOfRarity(Rarity.COMMON);
			_uncommons = GatherItemsOfRarity(Rarity.UNCOMMON);
			_rares = GatherItemsOfRarity(Rarity.RARE);
			_uniques = GatherItemsOfRarity(Rarity.UNIQUE);
		}

		private List<Item> GatherItemsOfRarity(Rarity rarity)
		{
			List<Item> items = [];
			items.AddRange(Databases.Craftables.Items
				.Where(item => item.Tags.Any(itemTag => Tags.Contains(itemTag)))
				.Where(item => item.ItemRarity == rarity));
			
			GD.Print($"\n{rarity} items\n");
			foreach (Item item in items)
			{
				GD.Print(item.Name);
			}
			GD.Print("\n");
			
			return items;
		}

		private void ChooseRandomItem(int random = -1)
		{
			Random rng = new();
			if (random == -1)
			{
				random = rng.Next(0, 100);
			}
			switch (random)
			{
				// Unique
				case < 5:
					int uniqueIndex = rng.Next(0, _uniques.Count);
					if (_uniques.Count > 0)
					{
						Loot.Add(_uniques[uniqueIndex]);
						break;
					}
					// Falldown to rare case
					ChooseRandomItem(5);
					break;
				// Rare
				case < 15:
					int rareIndex = rng.Next(0, _rares.Count);
					if (_rares.Count > 0)
					{
						Loot.Add(_rares[rareIndex]);
						break;
					}
					// Falldown to uncommon case
					ChooseRandomItem(15);
					break;
				// Uncommon
				case < 30:
					int uncommonIndex = rng.Next(0, _uncommons.Count);
					if (_uniques.Count > 0)
					{
						Loot.Add(_uncommons[uncommonIndex]);
						break;
					}
					// Falldown to common case
					ChooseRandomItem(30);
					break;
				// Common
				case < 100:
					int commonIndex = rng.Next(0, _commons.Count);
					if (_uniques.Count > 0)
					{
						Loot.Add(_commons[commonIndex]);
					}
					else
					{
						GD.PushWarning("No item found with correct rarity.");
					}
					break;
			}
		}
	}
}
