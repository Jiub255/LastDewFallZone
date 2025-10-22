using System.Collections.Generic;
using System.IO;
using Godot;

namespace Lastdew
{
    [Tool]
    public partial class NamespaceContextMenu : EditorContextMenuPlugin
    {
        static string Namespace => "Lastdew";
        Texture2D Icon { get; } = GD.Load<Texture2D>("uid://ci1ycdtuh12k7");

        public override void _PopupMenu(string[] paths)
        {
            base._PopupMenu(paths);
            
            Callable namespaceCallable = new(this, MethodName.AddNamespaces);
            AddContextMenuItem("Add Namespace", namespaceCallable, Icon);
            
            Callable toolCallable = new(this, MethodName.AddTools);
            AddContextMenuItem("Add Tool Attribute", toolCallable, Icon);
        }

#region NAMESPACE
        private void AddNamespaces(string[] paths)
        {
            foreach (string path in paths)
            {
                string absolutePath = ProjectSettings.GlobalizePath(path);
                if (absolutePath.EndsWith(".cs"))
                {
                    AddNamespace(absolutePath);
                }
            }
        }

        private void AddNamespace(string path)
        {
            string[] lines = GetLines(path);

            bool alreadyHasNamespace = SeparateLines(
                lines,
                out List<string> usingLines,
                out List<string> otherLines,
                out string end);
            if (alreadyHasNamespace)
            {
                return;
            }

            string newScript = InsertNamespace(usingLines, otherLines, end);

            OverwriteScript(path, newScript);
        }

        private static string[] GetLines(string path)
        {
            string text = File.ReadAllText(path);
            string[] lines = text.Split("\n");
            return lines;
        }

        /// <returns>true if file already has a namespace</returns>
        private bool SeparateLines(string[] lines, out List<string> usingLines, out List<string> otherLines, out string end)
        {
            usingLines = [];
            otherLines = [];
            end = "";
            foreach (string line in lines)
            {
                string stripped = line.StripEdges();

                if (stripped.StartsWith("namespace "))
                {
                    this.PrintDebug($"file already has a namespace");
                    return true;
                }

                if (stripped.StartsWith("using") || stripped.StartsWith("#if"))
                {
                    usingLines.Add(line);
                }
                else if (stripped.StartsWith("#endif"))
                {
                    end = line + "\n";
                }
                else
                {
                    otherLines.Add(line);
                }
            }

            return false;
        }

        private string InsertNamespace(List<string> usingLines, List<string> otherLines, string end)
        {
            return string.Join("\n", usingLines)
                + $"\n\nnamespace {Namespace}\n{{"
                + string.Join("\n\t", otherLines[..^1])
                + "\n}\n"
                + end;
        }

        private static void OverwriteScript(string path, string newScript)
        {
            string absolutePath = ProjectSettings.GlobalizePath(path);
            File.WriteAllText(absolutePath, newScript);
        }
#endregion
        
#region TOOL
        private void AddTools(string[] paths)
        {
            foreach (string path in paths)
            {
                string absolutePath = ProjectSettings.GlobalizePath(path);
                if (absolutePath.EndsWith(".cs"))
                {
                    AddTool(absolutePath);
                }
            }
        }

        private static void AddTool(string path)
        {
            string[] lines = GetLines(path);
            
            if (lines[0].StripEdges().StartsWith("#if"))
            {
                return;
            }

            List<string> beforeClass = [];
            List<string> classAndAfter = [];
            for (int i = 0; i < lines.Length - 1; i++)
            {
                string line = lines[i];
                if (line.Contains("public ") && line.Contains(" class "))
                {
                    classAndAfter.AddRange(lines[i..]);
                    break;
                }
                else
                {
                    beforeClass.Add(line);
                }
            }

            bool hasNamespace = false;
            foreach (string line in beforeClass)
            {
                if (line.Contains("namespace "))
                {
                    hasNamespace = true;
                }
            }

            string maybeTab = hasNamespace ? "\t" : "";

            string newScript =
                "#if TOOLS\n"
                + string.Join("\n", beforeClass)
                + "\n"
                + maybeTab
                + "[Tool]\n"
                + string.Join("\n", classAndAfter)
                + "#endif\n";
            
            OverwriteScript(path, newScript);
        }
#endregion
    
    }
}
