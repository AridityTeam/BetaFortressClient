# Steam static class
```cs
/// <summary>
/// Beta Fortress Team's own implementation of the Steam class like from TF2CLauncher
/// DO NOT CHANGE UNLESS YOU KNOW WHAT YOUR DOING
/// </summary>
public static class Steam
```

## Strings
### GetSteamPath
```cs
/// <summary>
/// Returns a value where the Steam client was installed
/// </summary>
public static string GetSteamPath
```

### GetSourceModsPath
```cs
/// <summary>
/// Returns a value where the "sourcemods" directory is
/// </summary>
public static string GetSourceModsPath
```

## Booleans
### IsSteamInstalled
```cs
/// <summary>
/// Checks if Steam is installed by checking if the registry keys for Steam exists or checking if the
/// Steam installation directory exists
/// </summary>
public static bool IsSteamInstalled
```
### IsAppInstalled
```cs
/// <summary>
/// Checks if the specific app ID is installed
/// (ex.: 220 is HL2, 240 is CS:S, 440 is TF2)
/// </summary>
/// <param name="appId"></param>
/// <returns></returns>
public static bool IsAppInstalled( int appId )
```

| Exceptions | Parameters | Description |
| ---------- | ---------- | ----------- |
|            |   appId    | The Steam App ID to check if it is installed on the user's machine |

### IsAppUpdating
```cs
/// <summary>
/// Checks if the specific app ID is updating
/// Only useful in some cases
/// </summary>
/// <param name="appId"></param>
/// <returns></returns>
public static bool IsAppUpdating( int appId )
```

| Exceptions | Parameters | Description |
| ---------- | ---------- | ----------- |
|            |   appId    | The Steam App ID to check if it is updating |

## IsAppRunning
```cs
/// <summary>
/// Checks if the specific app ID is running
/// NOTE: If the game/software has a mutex, you can use the Mutex class as an better alternative.
/// </summary>
/// <param name="appId"></param>
/// <returns></returns>
public static bool IsAppRunning( int appId )
```
| Exceptions | Parameters | Description |
| ---------- | ---------- | ----------- |
|            |   appId    | self-explanatory |

## Void/Functions
## RunApp
```cs
/// <summary>
/// Runs a specific app ID
/// </summary>
/// <param name="appId"></param>
public static void RunApp( int appId )
{
    Process p = new Process();
    p.StartInfo.FileName = GetSteamPath + "\\steam.exe";
    p.StartInfo.UseShellExecute = false;
    p.StartInfo.Arguments = "-applaunch " + appId;
    p.Start();
}
```

| Exceptions | Parameters | Description |
| ---------- | ---------- | ----------- |
|            |   appId    | self-explanatory |

## RunApp

| Exceptions | Parameters | Description |
| ---------- | ---------- | ----------- |
|            |   appId    | self-explanatory |
|            |   args     | extra launch options |
```cs
/// <summary>
/// Runs a specific app ID with extra launch options
/// </summary>
/// <param name="appId"></param>
/// <param name="args"></param>
public static void RunApp( int appId, string args )
{
    Process p = new Process();
    p.StartInfo.FileName = GetSteamPath + "\\steam.exe";
    p.StartInfo.UseShellExecute = false;
    p.StartInfo.Arguments = "-applaunch " + appId + " " + args;
    p.Start();
}
```
