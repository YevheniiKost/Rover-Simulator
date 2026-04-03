#  Rover Simulator

A physics-based rover driving simulation built with Unity (URP).
Explore terrain, switch camera views, and feel real engine response — with full keyboard and gamepad support.

---

##  Preview

> *GIF — Overview / Third-Person view*
>
>![Overview 1](Docs/Gifs/overview_1.gif)
>![Overview 2](Docs/Gifs/overview_2.gif)

> *GIF — First-Person cockpit view*
>
>![First-Person](Docs/Gifs/first_person.gif)

---

##  Controls

| Action | Keyboard | Gamepad |
|---|---|---|
| Drive / Steer | `W A S D` or Arrow Keys | Left Stick |
| Switch Camera | `Space` | South Button (A / Cross) |
| Pause | `Escape` | Start |

> UI navigation (main menu, settings) uses standard pointer / gamepad UI bindings via Unity Input System.

---

##  Features

- **Physics-driven rover** — Rigidbody + WheelColliders with configurable mass and motor torque
- **Dual camera modes** — seamlessly toggle between **Third-Person** and **First-Person** views (Cinemachine)
- **Configurable rover** — motor torque, steering coefficient, and mass exposed via ScriptableObject config
- **Full input support** — keyboard & mouse and gamepad handled through Unity Input System

---

## Project Structure

```
Assets/RoverSimulator/
├── Scripts/
│   ├── Data/               # Input & config layer (IInputProvider, RoverConfig)
│   ├── Domain/             # Pure logic (EngineModel, SimulationModel)
│   ├── Presentation/       # Unity MonoBehaviours (RoverView, CameraController, UI)
│   └── Utilities/          # Shared tools (DI container, logger)
├── Input/                  # Unity Input Actions asset
└── ...
```

The codebase follows a **clean layered architecture** across four assemblies:

| Assembly | Responsibility |
|---|---|
| `RoverSimulator.Data` | Input reading, config loading |
| `RoverSimulator.Domain` | Engine physics model, simulation state |
| `RoverSimulator.Presentation` | MonoBehaviours, cameras, UI windows |
| `RoverSimulator.Utilities` | DI container, logging |

Dependencies flow strictly **inward** — Presentation → Domain ← Data, with Utilities available to all.

---

## Tech Stack

- **Unity** (URP)
- **Unity Cinemachine** — camera rigs
- **Unity Input System** — unified keyboard & gamepad input
- **Custom lightweight DI container** — service wiring at bootstrap

---

## Getting Started

1. Clone the repository.
2. Open the project in **Unity 6** or later.
3. Open the main scene in `Assets/RoverSimulator/Scenes/`.
4. Press **Play** — use the Main Menu to start the simulation.
