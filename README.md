# DevelopersDays2019
## Get prepared
To get started, you will need a Docker image with Telepresence. To build it, run the Dockerfile from this repository. You will also need to deploy all the services to your k8s cluster.
## Running kubectl
1. Open VS Code using workspace(`all.code-workspace`)
2. Ctrl + Shift + P 
3. Run Task(`K8S: .NET Core Attach` from `DevelopersDays.Web`)
4. Make a breakpoint in the `DevelopersDays.Web` project
5. Hit the page and see how it hits your breakpoint
## Running Azure Dev Spaces
1. Open VS Code using workspace(`all.code-workspace`)
2. Make sure you have Azure Dev Spaces installed
3. Run Task(`.NET Core Launch (AZDS)`)
4. Make a breakpoint in the selected project
5. Hit the page and see how it hits your breakpoint
## Running Telepresence
1. Open VS Code using workspace(`all.code-workspace`)
2. Change `{{T9S_TAG}}` in `run-telepresence.cmd` to the image name you created in the very beginning
3. Run Task(`Telepresence: .NET Core Lunch`)
4. Make a breakpoint in the selected project
5. Hit the page and see how it hits your breakpoint