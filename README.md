<p align="center">
  <img width="50%" alt="prostir" src="https://github.com/paundra0217/paundra0217/blob/main/images/Peak%202024-09-18%2022-07-35.gif">
  </br>
</p>

## 🔴About
**Peak** is a 2D platformer game that tells a story about a young man who is trying to climb to the top of a mountain. Your task is to try to climb to the top of the mountain while explore many different places and environments through the mountain and avoid obstacles while maintaining stamina and health.

## ⬇️Download Game
Itch.io : https://bgdc.itch.io/peak

## 🕹️Game controls
| Key Binding       | Function          |
| ----------------- | ----------------- |
| W, A, S, D        | Player Movement   |
| Space        | Jump             |
| Q       | Soar   |

## 💼My Responsilibites
As the sole programmer on this 2D game project, I was responsible for the entire development pipeline. This included implementing game mechanics from the designer's perspective, creating interactive environments, and developing the player character's movement and abilities. I integrated various assets, such as sprites and sound effects, to bring the game world to life. Through meticulous testing and debugging, I delivered a polished and enjoyable gaming experience.

## 📋 Project Info and Developers
This project was made using Unity 2022.3.12f1

Developers:
- Vincent Pho Wijaya (Team Manager, Design, and Story)
- Ariq Bimo Nurputro (Supervisor)
- **Paundra Amirtha Tanto (Programming)**
- Bagas Hidayat (Art)
- Nathania Joscelind (Art and Animation)
- Nicholas Van Lukman (Sound)

##  📜Scripts
|  Script       | Description                                                  |
| ------------------- | ------------------------------------------------------------ |
| `GameManager.cs` | Manages the game flow such as checkpoints, current state, timer, etc. |
| `DialogueManager.cs` | Manages the dialogue system in the game. |
| `InteractableManager.cs` | Manages the interactables inside the game and calls the function based on each interactable. |
| `AudioController.cs` | Manages all kind of audios existed in the project, and handles play, pause, and stop function calls based on events happening in the game. |
| `etc`  | |

## 📂Files description
```
├── batavia-outbreak                    # In this Folder, containing all the Unity project files, to be opened by a Unity Editor
   ├── ...
   ├── Assets                           # In this Folder, it contains all of our code, assets, scenes, etc.
      ├── ...
      ├── Scenes                        # In this folder, there are scenes. You can open these scenes to play the game via Unity.
         ├── MainMenu.unity             # The entry scene for this game is this scene, which is MainMenu.unity
      ├── Scripts                       # In this Folder, there are scripts that manage various game mechanics and functionalities.
         ├── Object                     # In this Folder, there are scripts that handles the behaviour of the objects, like obstacles and traps.
         ├── Player                     # In this Folder, there are scripts that handles the behaviour and mechanics of the Player.
         ├── System                     # In this folder, there are scripts that handles the system or backend of the game.
         ├── UI                         # In this folder, there are scripts that handles the UI of the game.
      ├── ....
   ├── ...
      
```

