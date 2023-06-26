# Amanote Case Study

## Overview

- [Amanote Case Study](#amanote-case-study)
  - [Overview](#overview)
  - [Introduction](#introduction)
    - [``Frameworks & Technologies:``](#frameworks--technologies)
  - [Folder Structure](#folder-structure)
  - [Main Components](#main-components)
    - [``UI components``](#ui-components)
    - [``Scripts``](#scripts)
  - [Problems](#problems)
  - [**Future Improvements**](#future-improvements)

[//]: # (- [Key Features]&#40;#key-features&#41;)

## Introduction

This is my sbmission for Amanote Playable Ad Dev Interview Case Study

- [Final Output](https://asdqwe02.github.io/Case-Study-Hosting/)

### ``Frameworks & Technologies:``

- [Extenject](https://github.com/Mathijs-Bakker/Extenject): Dependency injection framework
- [DryWetMidi](https://github.com/melanchall/drywetmidi): Framework to read and access midi file data
- [Unity](https://unity.com): Game Engine

[//]: # (## Key Features)

[//]: # ()
[//]: # (- Highlight the key features or functionalities of the project.)

[//]: # (- Explain how these features are implemented within the project structure.)

[//]: # (- Mention any important algorithms, data structures, or design patterns utilized.)

## Folder Structure

For the folder and file structures of this project first I create a folder name [`CaseStudy`](./Assets/CaseStudy) inside
my `Assets` folder to hold all the assets being used.

Inside the [`CaseStudy`](./Assets/CaseStudy) folder:

```
├───Animation                       
├───Audio
├───Materials
├───Prefabs
├───Scenes
│   └───MusicNightBattle
│       ├───Scenes
│       └───Scripts
├───Scripts
│   ├───Configs
│   ├───GameLogicControllers
│   ├───Signals
│   └───VisualControllers
├───Settings
└───Sprites
    ├───arrow
    ├───characters
    ├───fx-score
    ├───long-note
    ├───others
    └───ui
```

- The [`Scenes`](./Assets/CaseStudy/Scenes) folder will be consist of the scene asset file and scripts that control the
  main logic for that particular scene
- The [`Scripts`](./Assets/CaseStudy/Scripts) is splitted into multiple sub folder:
  - [`Configs`](./Assets/CaseStudy/Scripts/Configs): contains all the scriptable object scripts to parameterize assets
      reference and game settings like note duration, input delay, note tap and despawn position etc
  - [`GameLogicControllers`](./Assets/CaseStudy/Scripts/GameLogicControllers): contains scripts that control the game logic like
      input and process input, spawn note, read and process Midi file data etc
  - [`Signals`](./Assets/CaseStudy/Scripts/Signals): Contains class/struct which then will be send to scripts that subscribed to an event being called by [`SignalBus`](https://github.com/modesttree/Zenject/blob/master/Documentation/Signals.md)
  - [`VisualsControllers`](./Assets/CaseStudy/Scripts/VisualControllers): Contains script that control visual elements like visual effects, animation etc

## Main Components

### ``UI components``

- "Start" and "Try Again" Buttons:

    ![Alt text](./README%20images/image-2.png)  ![Alt text](./README%20images/image-3.png)

- Arrow Buttons:

    ![Arrow Buttons](./README%20images/image.png)

    To receive input from player on a particular note lane

- Countdown:

    ![Alt text](./README%20images/image-1.png)

    Display countdown time when click start button

### ``Scripts``

- [`MusicNightBattleLogic`](./Assets/CaseStudy/Scenes/MusicNightBattle/Scripts/MusicNightBattleLogic.cs): This script is the main logic of the game, it will process logic like calculate the healthbar when note hit or miss, send start or end game signal, send countdown signal etc.

- [`MusicNightBattleInstaller`](./Assets/CaseStudy/Scenes/MusicNightBattle/Scripts/MusicNightBattleInstaller.cs): This script is used to install dependencies like `MusicNightBattleLogic`, `SongController`, `SongConfig` etc, for other scripts to inject into.

- [`MusicNightBattleInstaller`](./Assets/CaseStudy/Scenes/MusicNightBattle/Scripts/MusicNightBattleController.cs):
  
    This script will receive signals event like start/end game and display the main game UI or the title screen respectively. It will also receive signals event when a note hit/miss and play hit SFX and miss SFX respectively  
  
- [`Lane`](./Assets/CaseStudy/Scripts/GameLogicControllers/Lane.cs):

    This script control when the note of a particular lane will be spawn and when that note will need tap input to register a note hit.
  
    There will be 4 note `Lane` and these lanes will change its position when the game start depend on the position get from converting screen position of the button represent the lane's note
  
- [`SongController`](./Assets/CaseStudy/Scripts/GameLogicControllers/SongController.cs):
  
    This script will read the midi file and process it data then send it to `Lane` script, start song and calculate the current song time to sync note tap input and song together.
  
## Problems

- The game UI can scale relatively well with any resolution but the game objects can't scale well on any aspect ratio other than `16:9` or `9:16`
- Some of the components in game are still tightly couple together.
- Some of the core logics like processing input and when to spawn note is heavily depend on the `Lane` script
- A lot of assets refernces and game setting values haven't been parameterized yet which make it harder to maintain or add new features to the project in the future  
- It might be hard for other people to work on this project if they don't know about the `Extenject` dependency injection frameowrk

## **Future Improvements**

- Write logic to detect screen resolution of different aspect ratio to scale game object accordingly or lock camera aspect ratio to `16:9` or `9:16`
- Decoupling game logic in the project and modularize scripts so that it can be easier to implement new feature and maintain the project in the future.
- Refactor the spawn note logic in `Lane` script and move input processing outside to another sript. Create an input dispatcher script to send input signal to scripts that subscribe to an input event to modularize input processing logic.
- Paremeterize all the asset references and game setting values in the game for easier maintainence, changing assets and tweaking game object values in game.
- Can migrate from `Extenject` DI framework if needed to make it easier for when other people who don't know about `Extenject` to work on the project in the future. Instead of `Extenject` should use ScriptableObject architecture and design patterns to modularize the game logic.
- Imrpove the VFX when hitting a note and missing a note, make the visual effect look more vibrant and snappy
- Replace current note hit SFX with different SFX for each note type.
- Update the character visual to use animation instead of just changing sprite when hitting a note and missing a note.
- Refactor the game loop logic to make it more efficient and reduce tight coupling.
