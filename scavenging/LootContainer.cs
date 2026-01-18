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
		
		[Export]
		public Godot.Collections.Array<ItemTags> Tags { get; private set; } = [ItemTags.CRAFTING];
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
		[Export]
		private int MinimumFood { get; set; }
		[Export]
		private int MaximumFood { get; set; }
		[Export]
		private int MinimumWater { get; set; }
		[Export]
		private int MaximumWater { get; set; }
		[Export]
		private int MinimumAmmo { get; set; }
		[Export]
		private int MaximumAmmo { get; set; }

		
		public Godot.Collections.Array<Item> Loot { get; } = [];
		public int Food { get; private set; }
		public int Water { get; private set; }
		public int Ammo { get; private set; }
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
			
			GatherItemsByRarity();

			Random rng = new();
			int numberOfItems = rng.Next(MinimumItemAmount, MaximumItemAmount + 1);
			for (int i = 0; i < numberOfItems; i++)
			{
				ChooseRandomItem();
			}
			
			Food = rng.Next(MinimumFood, MaximumFood + 1);
			Water = rng.Next(MinimumWater, MaximumWater + 1);
			Ammo = rng.Next(MinimumAmmo, MaximumAmmo + 1);
			
			// Not really necessary, just saving memory (?)
			_commons.Clear();
			_uncommons.Clear();
			_rares.Clear();
			_uniques.Clear();
		}

		private void GatherItemsByRarity()
		{
			_commons = GatherItemsOfRarity(Rarity.COMMON);
			_uncommons = GatherItemsOfRarity(Rarity.UNCOMMON);
			_rares = GatherItemsOfRarity(Rarity.RARE);
			_uniques = GatherItemsOfRarity(Rarity.UNIQUE);
		}

		private List<Item> GatherItemsOfRarity(Rarity rarity)
		{
			return Databases.Craftables.Items
				.Where(item => item.Tags.Any(itemTag => Tags.Contains(itemTag)))
				.Where(item => item.Rarity == rarity)
				.ToList();
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
				case < (int)Rarity.UNIQUE:
					if (_uniques.Count > 0)
					{
						int uniqueIndex = rng.Next(0, _uniques.Count);
						Loot.Add(_uniques[uniqueIndex]);
						break;
					}
					// Falldown to rare case
					ChooseRandomItem(5);
					break;
				
				case < (int)Rarity.RARE:
					if (_rares.Count > 0)
					{
						int rareIndex = rng.Next(0, _rares.Count);
						Loot.Add(_rares[rareIndex]);
						break;
					}
					// Falldown to uncommon case
					ChooseRandomItem(15);
					break;
				
				case < (int)Rarity.UNCOMMON:
					if (_uncommons.Count > 0)
					{
						int uncommonIndex = rng.Next(0, _uncommons.Count);
						Loot.Add(_uncommons[uncommonIndex]);
						break;
					}
					// Falldown to common case
					ChooseRandomItem(30);
					break;
				
				case < (int)Rarity.COMMON:
					if (_commons.Count > 0)
					{
						int commonIndex = rng.Next(0, _commons.Count);
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
