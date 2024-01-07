# Configure Development Environment
This document is for developers who want to contribute.

## Prerequisites

## Install .NET SDK

Before you can run a .NET application, you need to install the .NET SDK. To install the latest .NET 7 SDK, follow these steps:

- Visit the [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0) download page.
- Choose the SDK that corresponds to your operating system.
- Click on the download link and run the installer.
- Follow the instructions in the installer.

You can verify the installation by opening a new command prompt or terminal window and running the command `dotnet --version`. This should display the version of the .NET SDK that you installed. If you installed it correctly, it should show a version number starting with `7.X.X`.

### Development Environment
You have the flexibility to develop this project using one of the recommended IDEs below, or feel free to use your preferred IDE:

## Visual Studio Code

- **Visual Studio Code**: This is a lightweight but powerful source code editor which runs on your desktop and is available for Windows, macOS, and Linux. You can download it from the official [Visual Studio Code](https://code.visualstudio.com/download) page.
- After installing Visual Studio Code, open the project folder by clicking on `File -> Open Folder`.

## Visual Studio

- **Visual Studio**: This is a full-featured IDE that runs on Windows and macOS. You can download it from the official [Visual Studio](https://visualstudio.microsoft.com/downloads/) page.
- After installing Visual Studio, open the project by clicking on `File -> Open -> Project/Solution` and selecting the `.sln` file of your project.

## JetBrains Rider

- **JetBrains Rider**: This is a fast and powerful .NET IDE developed by JetBrains. It runs on Windows, macOS, and Linux. You can download it from the official [JetBrains Rider](https://www.jetbrains.com/rider/download/) page.
- After installing Rider, open the project by clicking on `File -> Open` and selecting the `.sln` file of your project.


## Steps

> :warning: **Important!** Before running the application, make sure you have set up your environment correctly. You must have read the [Managing Secrets with User-Secrets](#managing-secrets-with-user-secrets) and [Generating a GitHub Access Token](#generating-a-github-access-token) sections.

1. **Clone the repository**: Use the `git clone` command followed by the URL of the repository. This will create a local copy of the project on your machine.

2. **Navigate to the project directory**: Use the `cd` command followed by the path of the directory. For example, if the project is in a directory named `src/CrystallineSociety`, you would use the command `cd src/CrystallineSociety`.

3. **Run the web app**: Navigate to the `Client/App` directory within the project directory. Use the `dotnet run` command to start the web app. This command will build the project and start running the web server.

4. **Run the API app**: Navigate to the `Server` directory within the project directory. Use the `dotnet run` command to start the API app. This command will build the project and start running the API server.

# Managing Secrets with User-Secrets

.NET provides a secrets manager tool that can be used for storing sensitive data during the development of a project. This tool stores sensitive data in a separate secrets.json file that is not checked into source control, unlike the `appsettings.json` file.

Here's how you can use the secrets manager:

1. **Install the Secret Manager tool globally**: Run the following command in your terminal:

```bash
dotnet tool install --global dotnet-user-secrets
```

2. **Navigate to your API project directory**: Use the `cd` command to navigate to the directory of your API project. If your API project is located in a folder named `Server` inside the `src/CrystallineSociety` directory, you would use the following command:

```bash
cd src/CrystallineSociety/Server
```

3. **Initialize the secrets storage**: Run the following command:

```bash
dotnet user-secrets init
```

4. **Set the secrets**: Use the `dotnet user-secrets set` command to set your secrets. For example:

> :warning: **Important!** You will need to generate your own GitHub Access Token. Instructions on how to do this will be provided in the [next section](#generating-a-github-access-token). For the SQL Server Connection String, you can request access to the Telegram group that contains the connection string. You can join the group using this [link](https://t.me/+VLs-FTg5nLRmMGY0).

```bash
dotnet user-secrets set "ConnectionStrings:SqlServerConnectionString" "Your SQL Server Connection String"
dotnet user-secrets set "GitHub:GitHubAccessToken" "Your GitHub Access Token"
```

Remember to replace `"Your SQL Server Connection String"` and `"Your GitHub Access Token"` with your actual connection string and access token.

This way, your secrets are kept out of your project files and won't be checked into source control. For more information on managing application secrets during development with User Secrets, you can visit the official Microsoft documentation [here](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets).


# Generating a GitHub Access Token

A GitHub access token is a way to authenticate with GitHub without using a password. You can generate a new token as follows:

1. **Go to your GitHub settings**: Click on your profile picture in the top right corner of the GitHub homepage, then click on `Settings`.

2. **Navigate to `Developer settings`**: This is located at the bottom of the settings sidebar.

3. **Click on `Personal access tokens`**: This will take you to a page where you can see a list of your existing tokens.

4. **Generate a new token**: Click on the `Generate new token` button. You will be asked to enter your password.

5. **Name your token**: Enter a descriptive name in the `Note` field to help you remember what the token is for.

6. **Set the scopes**: Check the boxes for the scopes, or permissions, you want to grant this token. For example, if you want the token to be able to access your repositories, you would check the `repo` box.

7. **Generate the token**: Click the `Generate token` button at the bottom of the page. Your new token will be created.

8. **Copy the token**: After generating the token, make sure to copy it. You won't be able to see it again. Use the .NET user-secrets tool to store your new token. Run the following command in your terminal, replacing `"Your GitHub Access Token"` with your new token:

```bash
dotnet user-secrets set "GitHub:GitHubAccessToken" "Your GitHub Access Token"
```

For more detailed information, you can visit the official GitHub documentation on [Managing Your Personal Access Tokens](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens).
