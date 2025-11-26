Welcome to the MLX User's Guide!

This document contains documentation on how to utilize all of MLX's features.

Before using MLX, please read this document.

# Table of Contents
* 1: Basic Definitions
* 2: The Interace
  * 2.1: The Main Window
    * 2.1.1: The "Game" Side
    * 2.1.2: The "Modding" Side
  * 2.2: Dialog Menus
    * 2.2.1: Add Source Port Dialog
    * 2.2.2: Add Game Dialog
    * 2.2.3: New Preset Dialog

# Document Structure
* [x.x.x] signifies a link to another part of this document. This is usually when a part references something from another.

# 1: Basic Definitions
* `Game/IWAD`: The `.wad` or `.ipk3` file that contains the assets of the game. Examples include `DOOM.WAD` and `TNT.WAD`.
* `Source Port`: The engine that can run the game. Examples include UZDoom, DSDA-Doom, and *Woof!*.
* `External Files`: Files (like mods) that will be loaded alongside the game.
* `Extra Arguments`: Options given to the source port. Examples include `-complevel` and `-warp`.
* `RP`: The Discord Rich Presence integration present in this program.

# 2: The Interface
The interface of MLX has been designed to be easily used and understood.
However, for newcomers, this may still be complicated to use.

Below contains a description of each section of the UI.

## 2.1: The Main Window
This is the main place of interaction of MLX. 
Here you'll select your game, port, and optionally mods.
You can even create presets containing all of the above.

There are two main "sections" of this window: The "Game" side, and the "Modding" side.

### 2.1.1: The "Game" Side
This side contains drop-downs for what game you wish to play, as well as what port you wish to use.

You can also find the Extra Parameters textbox and the Launch button here.

The Extra Parameters textbox here allows you to define preset-specific parameters that can control certain aspects of the port.
Refer to your port's documentation for their specific parameters, or the Doom Wiki (at doomwiki.org) for id Tech 1 parameters.

You can add a game or source port by clicking their respective Add `+` buttons, or remove them with their Remove `-` buttons.
Either of the Add buttons will bring up a dialog [2.2] to add in the new game/port to their list.

You can then select the game and port by using their respective drop-down menus.

### 2.1.2: The "Modding" Side
This side contains the functionality of anything related to either mod files or presets.

The External Files list allows you to see what mods you currently have loaded and what order they are loaded in.

To add a mod to the list, you must click the Add `+` button. This will bring up a file dialog. 
Select the file(s) you wish to add click Open (or the equivalent on your OS). 
This will add the files to the very bottom of the list.

To remove a mod, you must click on it on the list and click on the Remove `-` button.
You can also clear the whole list by clicking the Clear List button.

To change the load order, you must click on a mod and use the Up `↑` and Down `↓` buttons to do so.

## 2.2: Dialog Menus
Dialog menus will sometimes pop up depending on what you're doing. 
This includes adding a game or port, creating a new preset, or even adding a mod to the External Files list.

Often they'll ask for a file path and visible name.

### 2.2.1: Add Source Port Dialog
This dialog pops up when you select the Add Source Port button [2.1.1].

Here it asks for
* The name (what you'll see in the port drop-down [2.1.1] and what others will see on the RP),
* The file path (where the port executable is located),
* And extra parameters.

You can either manually enter a file path, or use the Select File `...` button to bring up a file dialog.

The extra parameters seen here are similar to the ones seen in the Main Window [2.1.1].
However, the difference here is that they will always be applied as long as that port is selected in the drop-down.
This is particuarly useful when you must always have a certain option set, but don't want to clutter up your presets with it.

### 2.2.2: Add Game Dialog
This dialog pops up when you select the Add Game button [2.1.1].

Here it asks for
* The name (what you'll see in the port drop-down [2.1.1] and what others will see on the RP),
* The file path (where the port executable is located).

You can either manually enter a file path, or use the Select File `...` button to bring up a file dialog.
The name will generally be automatically filled in after selecting a file, but you can manually set one yourself as well.

### 2.2.3: New Preset Dialog
This dialog pops up when you select the New Preset button [2.1.2]

Here it asks for
* The name (what you'll see in the port drop-down [2.1.1] and what others will see on the RP),
* If you wish to display the preset name in the RP.

By default, MLX shows the first two loaded mod file names in the RP, but you can make it show the preset name instead by selecting that option.