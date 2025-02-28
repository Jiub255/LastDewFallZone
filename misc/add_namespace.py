import os

namespace = "Lastdew"
root_directory = "/home/jiub/godot/projects/lastdew-fallzone"

for dirpath, _, filenames in os.walk(root_directory):
    for filename in filenames:
        # TODO: Early continue if file is one of the two special 
        # cs files that shouldn't get a namespace.
        if filename.endswith(".cs"):
            file_path = os.path.join(dirpath, filename)

            # Read the contents of the file
            with open(file_path, "r") as file:
                lines = file.readlines()
            
            # Skip if the file already contains a namespace
            if any("namespace" in line for line in lines):
                print(f"Skipping {file_path}: Namespace already exists.")
                continue
            
            # Separate using statements from the rest of the content
            using_lines = [line for line in lines if line.strip().startswith("using ")]
            other_lines = [line for line in lines if not line.strip().startswith("using ")]
            
            # Move using statements inside the namespace
            new_content = (
                "".join(using_lines)
                + "\n"
                + f"namespace {namespace}\n{{"
                + "".join(["\t" + line for line in other_lines])
                + "}\n"
            )
            
            # Overwrite the file with the new content
            with open(file_path, "w") as file:
                file.write(new_content)
