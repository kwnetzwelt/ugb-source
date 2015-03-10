README for CodeWatchdog Unity Editor Package
============================================

[CodeWatchdog](https://bitbucket.org/flberger/codewatchdog) is a simple
coding convention compliance checker written in C#.

This package integrates CodeWatchdog into the Unity3D Editor.

**NOTE:** This is an alpha release. Expect a bumpy ride, and please look
after your data.


## Installation

There are several possibilities of getting CodeWatchdog into your project:

1. Using a recent [Unity Game Base](https://git.exozet.com/exozet/ugb-source).
2. As a buildstep of the [Unity Build System](https://bitbucket.org/kaiwegner/unity-build-system).
   The buildstep is called *UBS > WatchdogBuildStep*.
3. As a [Standalone Unity Package in GitLab](https://git.exozet.com/florian.berger/codewatchdog-unity-package).
   Move the folder "CodeWatchdogEditor" to *Assets/packages/* in your project.


## Usage

There are several ways of using the tool:

- **Menu**: Select *UGB > CodeWatchdog > Open Window* to open the CodeWatchdog report window.
  *UGB > CodeWatchdog > Run* will check all scripts in the *Assets/scripts/* folder and
  report a summary in the report window if it has been opened.
- As a **buildstep**, see above.
- If you select a C# script, its **inspector** will display a CodeWatchdog summary for that file.
  Below that it displays the file's content, followed by a detailled CodeWatchdog report.
- C# files **changed outside of Unity** will be checked automatically. If the report window
  has been opened (see above), a summary will be displayed there.


## Ideas, Suggestions, and Problems

If you miss a feature in CodeWatchdog or have a problem to report, please open a
[ticket in GitLab](https://git.exozet.com/florian.berger/codewatchdog-unity-package/issues).


## Files

*CodeWatchdogInspector.cs*: Customises the inspector for C# files.

*CodeWatchdogMenu.cs*: Add the UGB > CodeWatchdog submenu.

*CodeWatchdogUpdateCheck.cs*: Runs CodeWatchdog on changed C# files.

*WatchdogEditorWindow.cs*: The custom editor output window for CodeWatchdog.


*CamelCaseCSharpWatchdog.cs*
*Logging.cs*
*Watchdog.cs*

The actual CodeWatchdog business logic files.



End of README.
