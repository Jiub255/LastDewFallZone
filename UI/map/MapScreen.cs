using Godot;

namespace Lastdew
{
	public partial class MapScreen : MarginContainer
	{
		public LocationData LocationData { get; private set; }
		private TextureRect Map { get; set; }
		private LocationInfoUI LocationInfo { get; set; }

		public override void _Ready()
		{
			base._Ready();
			
			Map = GetNode<TextureRect>("%Map");
			LocationInfo = GetNode<LocationInfoUI>("%LocationInfo");
			
			foreach (Node node in Map.GetChildren())
			{
				if (node is LocationButton button)
				{
					button.OnPressed += SetupLocationInfo;
				}
			}
			ClearLocationInfo();
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			
			foreach (Node node in Map.GetChildren())
			{
				if (node is LocationButton button)
				{
					button.OnPressed -= SetupLocationInfo;
				}
			}
		}
		
		private void SetupLocationInfo(LocationData data)
		{
			LocationData = data;
			LocationInfo.Setup(
				data.Name,
				data.Image,
				data.Description);
		}
		
		private void ClearLocationInfo()
		{
			LocationInfo.Setup("", null, "");
		}
	}
}
