using System;
using System.Collections;
using Godot;

namespace Lastdew
{	
	[GlobalClass, Tool]
	public partial class AllPcDatas : Resource, IEnumerable
	{
		private const string DIRECTORY = "res://characters/player-characters/management/pc_datas/";
		private const string PATH = "res://characters/player-characters/management/pc_datas/all_pc_datas.tres";
        
		[Export]
		public Godot.Collections.Dictionary<string, PcData> PcDatas { get; set; } = new();
		
        public PcData this[string name] => PcDatas[name];
        
		public void PopulateDictionary()
		{
			PcDatas.Clear();
			
			PopulateDictionary(DIRECTORY);
			
			Error error = ResourceSaver.Save(this, PATH);
			if (error != Error.Ok)
			{
				this.PrintDebug($"Error saving resource: {error}");
			}
			
			this.PrintDebug($"Number of PcDatas: {PcDatas.Count}");
		}
		
		private void PopulateDictionary(string directory)
		{
			DirAccess dirAccess = DirAccess.Open(directory);
			if (dirAccess == null)
			{
				GD.PushError($"Failed to open directory: {directory}");
				return;
			}

			dirAccess.ListDirBegin();
			string fileName;
			while ((fileName = dirAccess.GetNext()) != "")
			{
				if (dirAccess.CurrentIsDir())
				{
					if (fileName == "." || fileName == "..")
					{
						continue;
					}

					// Recursively search subfolders
					string subFolder = directory + "/" + fileName;
					PopulateDictionary(subFolder);
				}
				else
				{
					string filePath = directory + "/" + fileName;
					Resource resource = GD.Load<Resource>(filePath);
					if (resource != null && resource is PcData data)
					{
						PcDatas[data.Name] = data;
					}
				}
			}
			dirAccess.ListDirEnd();
		}

		public IEnumerator GetEnumerator()
		{
			return PcDatas.Values.GetEnumerator();
		}
	}
}
