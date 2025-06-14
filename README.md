<h1>README</h1>

<h2>The Mod</h2>

Replaces the REPO Head Man Enemy with bombadiro crocodilo including some blessed audio to go alongside

<h2>How To Build</h2>

In order to build this project simply have dotnet installed, (created with version 9.0.200 however I have not checked if it's compatible with earlier versions).
Put all the files in the same folder and run the <i>dotnet build</i> command.

This will generate BombadiroCrocodilo.dll in the bin folder for netstandard2.1 which is the plugin dll used in the mod up on thunderstore

<h2>How to Run as a Plugin</h2>

Either install the mod though thundertore (although if you built it thats probably not what you care about) or if you built the dll file, you will need to make a folder
in you plugins, titled really whatever you would like.

Inside this folder put the dll file alongside the contents from the assets folder provided through this link
<u>https://drive.google.com/drive/folders/1hVHRYpzlHBOPMb3btf3OnebkAJO3Y9Z-?usp=sharing</u>

<b>DO NOT</b> copy the extracted folder directly, copy all of its components seperately.
This will ensure that the mod will be able to find its resources

<h3>Expected Structure</h3>
  - Bombadiro Head Man Mod Folder <br>
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- BombadiroCrocodilo.dll <br>
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- All of the Assets, NOT in there own subfolder <br>
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-... <br>
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-... <br>
