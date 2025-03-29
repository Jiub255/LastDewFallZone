using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lastdew
{
    public partial class UidsFiller : EditorPlugin
    {
		private const string DIRECTORY = "res://";
		
		[Export]
		public Godot.Collections.Dictionary<string, string> UidsByFilename { get; set; } = new();
		private Button GatherUidsButton { get; set; }
		
		
        public string this[string name] => UidsByFilename[name];

        public override void _EnterTree()
        {
            GatherUidsButton = new Button
            {
                Text = "Gather UIDs"
            };
            GatherUidsButton.Pressed += PopulateDictionary;
            AddControlToContainer(CustomControlContainer.Toolbar, GatherUidsButton);
        }

        public override void _ExitTree()
        {
            GatherUidsButton.Pressed -= PopulateDictionary;
            RemoveControlFromContainer(CustomControlContainer.Toolbar, GatherUidsButton);
        }

        private void PopulateDictionary()
		{
			UidsByFilename.Clear();
			
			PopulateDictionary(DIRECTORY);
			
			// TODO: Format dictionary as part of .cs file
			// insert into Uids.cs at correct spot (test with different file first)
			// Save file? 
		}
		
		// TODO: Want to get .tscn files too.
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
					// TODO: Switch for resource type here? How to check which files to add?
					PackedScene scene = GD.Load<PackedScene>(filePath);
					if (scene != null)
					{
                        string constName = FormatFilenameAsConst(fileName);
						UidsByFilename[constName] = scene.ResourceSceneUniqueId;
					}
				}
			}
			dirAccess.ListDirEnd();
		}
		
		private static string FormatFilenameAsConst(string filename)
		{
            if (char.IsLower(filename[0]))
            {
                return filename.ToUpper();
            }
            else
            {
                // Separate words by upper case letters
                List<string> words = [];
                string word = "";
                foreach (char c in filename)
                {
                    if (char.IsUpper(c) && word != "")
                    {
                        words.Add(word);
                        word = c.ToString();
                    }
                    else if (char.IsLower(c))
                    {
                        word += c;
                    }
                }
                // Join with "_"
                string joined = string.Join("_", words);
                // ToUpper()
                return joined.ToUpper();
            }
		}
    }
}
