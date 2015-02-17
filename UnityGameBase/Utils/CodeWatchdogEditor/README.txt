README for CodeWatchdog Unity Editor Package
============================================

CodeWatchdog is a coding convention compliance checker written in C#.

See https://bitbucket.org/flberger/codewatchdog

This package integrates CodeWatchdog into the Unity3D Editor.


## Usage

There are several ways of using the tool:

- Use the menu UGB > Run CodeWatchdog. This will check the entire
  'scripts'-folder and display a summary in a dedicated window.

- Select a C# file in the project view. The Unity Inspector will
  display the CodeWatchdog output, along with the file's content
  and a detailled log.

- If you change C# files outside of Unity, CodeWatchdog will run
  automatically and display a summary for the changed files in
  a dedicated window.


## Files

*CodeWatchdogInspector.cs*: Customises the inspector for C# files.

*CodeWatchdogMenu.cs*: Add the UGB > Run CodeWatchdog menu.

*CodeWatchdogUpdateCheck.cs*: Runs CodeWatchdog on changed C# files.

*WatchdogEditorWindow.cs*: The custom editor output window for CodeWatchdog.


*CamelCaseCSharpWatchdog.cs*
*Logging.cs*
*Watchdog.cs*

The actual CodeWatchdog business logic files.



End of README.
