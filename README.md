# Need for Drift

Need for Drift is a drift-racing game built in one week with Unity: pick your car, drift around the track, and lap it as fast as you can!

> This README is for **developers**. If you just want to play, grab a build from the
> [Releases](https://github.com/B0TAstro/NeedForDrift/releases/latest) page — each release ships with
> a [HOW_TO_PLAY.md](HOW_TO_PLAY.md) guide.

## Tech stack

- **Unity 2022.3.62f3** (LTS)
- C# game scripts in `Assets/Scripts/`
- **Git LFS** for binary assets (3D models, textures, audio)

## Getting started

1. Make sure Git LFS is installed, then clone the repo:
   ```bash
   git lfs install
   git clone https://github.com/B0TAstro/NeedForDrift.git
   ```
   > Without Git LFS, binary assets come down as small pointer files instead of the real models/textures.
2. Open the project in **Unity 2022.3** (via Unity Hub).
3. Open and play `Assets/Scenes/Intro.unity` to start the game.

## Project structure

| Path | Purpose |
|---|---|
| `Assets/Scripts/` | Custom game scripts (see below) |
| `Assets/Scenes/` | Game scenes: `Intro`, `SelectCars`, `CarGame`, `CarTest` |
| `Assets/Presets/` | Per-vehicle stat presets (`VehiclesPreset`) |
| `Assets/Import/` | Third-party assets (vehicles, track, skybox, Prometeo car controller) |

## Scenes & flow

1. **Intro** (`introSceneIndex = 0`) — intro sequence, the player drives the bus.
2. **SelectCars** (`selectCarSceneIndex = 2`) — choose one of 5 vehicles.
3. **CarGame** (`gameSceneIndex = 1`) — the main drift track.
4. **CarTest** — sandbox scene for testing.

> Scene indices are defined in `GameManager` and must match the Build Settings scene order.

## Scripts overview

- **`GameManager`** — singleton; persists the selected vehicle and day/night state across scenes (`DontDestroyOnLoad`).
- **`CarController`** — vehicle physics and driving input (W/A/S/D, Space to drift).
- **`VehiclesPreset`** — per-vehicle stat configuration.
- **`Checkpoint`** — lap / track checkpoint system.
- **`SelectController`** — vehicle selection screen.
- **`InputController`** — global input (Esc toggles the pause menu).
- **`UIController`**, **`GameController`**, **`GeneralController`**, **`VehicleActivator`**, **`BaseController`** — supporting systems.

## Building

Build from `File → Build Settings`:

- **Windows** — add the *Windows Build Support (Mono)* module in Unity Hub, switch platform, then build into `Builds/Windows/`.
- **macOS** — switch platform, then build into `Builds/macOS/`.

`Builds/` is git-ignored — **never commit build output**. Distribute builds through GitHub Releases by zipping the **whole** build folder (the executable needs its data folder and libraries), not just the executable.

## Releasing

Releases are published manually from local builds — the **git tag is the version** (no CI, no extra config).

1. Build both platforms into `Builds/Windows/` and `Builds/macOS/` (see above).
2. Make sure GitHub CLI is authenticated: `gh auth login`.
3. Run the release script with the new version:
   ```bash
   ./scripts/release.sh v1.0.0
   ```

The script zips each build (bundling `HOW_TO_PLAY.md`), creates the tag, publishes the GitHub Release, and uploads `NeedForDrift-Windows.zip` + `NeedForDrift-macOS.zip`.
