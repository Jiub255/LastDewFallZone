import os

res_string = "uid://"
root_directory = "/home/jiub/godot/projects/lastdew-fallzone"

for dirpath, _, filenames in os.walk(root_directory):
    for filename in filenames:
        if filename.endswith(".cs"):
            file_path = os.path.join(dirpath, filename)

            # Read the contents of the file
            with open(file_path, "r") as file:
                lines = file.readlines()

            # TODO: Count number of res_string instances in file? Or just see if there's any.
            count = 0
            if any(res_string in line for line in lines):
                print(f"{file_path} contains a uid.")
