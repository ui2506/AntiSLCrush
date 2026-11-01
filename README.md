# AntiSLCrush

![SCP:SL Plugin](https://img.shields.io/badge/SCP--SL%20Plugin-blue?style=for-the-badge)
![Language](https://img.shields.io/badge/Language-C%23-blueviolet?style=for-the-badge)
![Downloads](https://img.shields.io/github/downloads/angelseraphim/AntiSLCrush/total?label=Downloads&color=333333&style=for-the-badge)

---
AntiSLCrush is a plugin for SCP: Secret Laboratory using LabApi, designed to prevent server crashes caused by game bugs, developer errors.
It adds additional protection and stability to your server.
---

## 📦 Installation

1. Download the latest release from the [Releases](../../releases) page.
2. Place the `.dll` file into your server's **LabApi** plugin directory.
3. Configure `config.yml` as needed.

## Building

The SDK-style project targets .NET Framework 4.8. Install a .NET SDK with C# 12 or newer support and the .NET Framework 4.8 targeting pack.
NuGet dependencies are restored through `PackageReference`.

Set the `SL_REFERENCES` environment variable to the directory containing the game assemblies from your server version. The project uses `$(SL_REFERENCES)` to resolve its game DLL references.

For a build in the current PowerShell session:

```powershell
$env:SL_REFERENCES = "C:\path\to\game-assemblies"
dotnet build AntiSLCrush.sln -c Release
```

The assembly directory must contain `Assembly-CSharp-Publicized.dll`, `Mirror-Publicized.dll`, `UnityEngine.CoreModule-Publicized.dll`, `NorthwoodLib.dll`, and `Pooling.dll`.

For Visual Studio, add `SL_REFERENCES` to your Windows user environment variables and restart Visual Studio so it picks up the value.

The plugin is written to `AntiSLCrush/bin/Release/AntiSLCrush.dll`.
