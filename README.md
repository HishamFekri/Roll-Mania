# Roll Mania

Roll Mania is a small 3D physics based racing prototype built with Unity and C#.

The player controls a ball and races through the track while dealing with gravity, obstacles, and time based gameplay.

## Gameplay

The goal is to reach the finish line as quickly as possible while staying on the track and avoiding hazards.

The game includes:

- Physics based ball movement
- Sprint / speed boost mechanic
- Increased gravity control
- Timer and speed display
- Pause and restart system
- Basic sound effects and visual effects

## How to Try the Game

A Windows build is available in the [**Releases**](https://github.com/HishamFekri/Roll-Mania/releases) section of this repository. This is the complete game, with all art and sound, and you do not need Unity to play it.

To play:

1. Open the [latest release](https://github.com/HishamFekri/Roll-Mania/releases/latest) on GitHub.
2. Download `Roll.Mainia.zip` (about 175 MB) from **Assets**.
3. Extract the whole ZIP file. The game will not run from inside the ZIP.
4. Open the extracted `Roll Mainia` folder.
5. Run `My project.exe`, which is the game's executable.

> Windows may show a security warning because the executable is not digitally signed. If this happens, choose **More info** → **Run anyway** only if you downloaded the file directly from this repository.

## Controls

- `W / A / S / D` — Move
- `Shift` — Speed boost
- `Space` — Increase downward gravity
- `Esc` — Pause / Resume

## Opening the Source Project in Unity

This repository contains the original Unity project, except for the third-party content listed below. Without that content the project still opens and its scripts compile, but the level will have missing models, pink materials and no sound.

1. Install **Unity 2022.3.49f1** through [Unity Hub](https://unity.com/download). Other versions may upgrade the project files.
2. Clone the repository:

   ```
   git clone https://github.com/HishamFekri/Roll-Mania.git
   ```

3. In Unity Hub, choose **Add** → **Add project from disk** and select the cloned `Roll-Mania` folder.
4. Open the project. The first import rebuilds the `Library` folder and can take several minutes.
5. Import the third-party content listed below.
6. Open `Assets/Scenes/SampleScene.unity` and press **Play**.

To make a Windows build, use **File** → **Build Settings**, select **Windows, Mac, Linux** and choose **Build**.

### Third-party assets

These items are used by the game but are not included here because this repository does not have the right to redistribute them in source form. They are only part of the compiled game in Releases.

| Expected location | Used for |
| --- | --- |
| `Assets/Low_Poly_Sci-fi_City` | City, road and building environment |
| `Assets/BEDRILL/Track_Environment_Free` | Lowpoly Track Environment Free: fences, signs, arrows and tires |
| `Assets/SpaceSkies Free` | Skybox |
| `Assets/TextureHaven` | Floor material (`floor_tiles_06`) |
| `Assets/SimpleFX` | Particle effect materials and models |
| `Assets/CreativeCore_UI` | UI font (Alfa Slab One) |
| `Assets/Audio` | Background music, pickup sound and win sound |

Import each pack so that it ends up in the location shown. Unity links assets by the IDs in their `.meta` files, so the original packs should reconnect automatically. Anything that stays missing, including replacement audio, can be assigned again in the Inspector.

## Built With

- Unity 2022.3.49f1
- C#
- Unity Physics / Rigidbody
- TextMeshPro
- Unity Input System

## Project Purpose

This project was created as a small game development prototype to practice:

- Object oriented programming
- Player movement and physics
- Game state management
- Unity components and collisions
- UI integration
- Input handling
- Basic gameplay mechanics

## Status

Prototype / learning project.

The game is playable, but some mechanics and level elements may still require additional polish and refinement.
