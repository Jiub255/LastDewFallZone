using Godot;
using System.Threading.Tasks;

namespace Lastdew
{
	public partial class Mugshooter : Node3D
	{
		private PackedScene PcScene { get; } = GD.Load<PackedScene>(Uids.PC_BASE);
		private PackedScene MugshotCameraScene { get; } = GD.Load<PackedScene>(Uids.MUGSHOT_CAMERA);
		
		
		public override void _Ready()
		{
			GetMugshots();
		}

		private async Task GetMugshots()
		{
			foreach (PcData pcData in Databases.PcDatas)
			{
				PlayerCharacter pc = (PlayerCharacter)PcScene.Instantiate();
				this.AddChildDeferred(pc);
				pc.Initialize(new InventoryManager(), new PcSaveData(pcData.GetUid()), new ExperienceFormula());
				await GetMugshotIcon(pc);
			}
		}

		private async Task GetMugshotIcon(PlayerCharacter pc)
		{
			MugshotCamera camera = (MugshotCamera)MugshotCameraScene.Instantiate();
			pc.AddChildDeferred(camera);
			
			// TODO: Not sure if both of these are necessary.
			await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
			await GetTree().ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

			Image screenshot = camera.TakeMugshot();
			Image edited = ResizeAndCrop(screenshot);
			string path = SaveImage(pc, edited);
			SetIcon(pc, path);
			
			pc.QueueFree();
		}

		private static Image ResizeAndCrop(Image original)
		{
			Image edited = Image.CreateEmpty(648, 648, false, original.GetFormat());
			edited.BlitRect(
				original,
				new Rect2I(252, 0, 648, 648),
				Vector2I.Zero);
			edited.Resize(128, 128, Image.Interpolation.Trilinear);
			return edited;
		}

		private static string SaveImage(PlayerCharacter pc, Image image)
		{
			string path = $"res://characters/player-characters/management/pc_data/{pc.Data.Name.ToSnakeCase()}.png";
			Error error = image.SavePng(path);
			if (error != Error.Ok)
			{
				GD.PushError($"Error saving png: {error}");
			}

			return path;
		}

		private static void SetIcon(PlayerCharacter pc, string path)
		{
			// TODO: Might not work unless SaveImage() overwrites an already existing png.
			// So just running the scene twice should work. 
			Texture2D texture = GD.Load<Texture2D>(path);
			if (texture == null)
			{
				return;
			}
			pc.Data.Icon = texture;
			// TODO: Does this somehow change the PcData UID?
			ResourceSaver.Save(pc.Data);
		}
	}
}
