# SkyEscape - A Vertical Escape Platformer 🌌

SkyEscape is a **fast-paced vertical platformer** where players must escape rising dangers by jumping between platforms, avoiding obstacles, and using special mechanics like wall jumps and fans to survive. As platforms continuously move upwards, the player must climb higher while avoiding deadly spikes and other hazards. With each level, survival time increases, making the challenge even harder.

- Game Engine: **Unity** 
- Programming Language: **C#**
- Supported Platforms: **Web**, **Android**, **Windows**

## 🚀 New Features and Improvements
1. **Jump and Double Jump**: Players can now perform double jumps, allowing greater control and maneuverability.
2. **Revival Mechanic**: Upon failure, players have a chance to revive. The revived state shows a brief "awakening" period, during which level and time are temporarily hidden.
3. **Touchscreen Support (Android)**:
   - **Movement**: Tap the left or right side of the screen to move.
   - **Jumping**: Double-tap to perform a jump.
4. **Multi-Platform Builds**:
   - **Web Version**: Can be played directly in the browser.
   - **Android Version**: APK available for touchscreen devices.
   - **Windows Version**: Executable file for PC gameplay.

---

## 🎮 Controls

### PC Version
| **Action**  | **Key**          |
|------------|------------------|
| Move Left  | A / Left Arrow    |
| Move Right | D / Right Arrow   |
| Jump       | Space             |
| Wall Jump  | Press Space while touching a wall |

### Android Version
| **Action**    | **Input Method**                          |
|--------------|--------------------------------------------|
| Move Left     | Tap left side of the screen                 |
| Move Right    | Tap right side of the screen                |
| Jump / Double Jump | Double-tap anywhere on the screen |

---

## 📝 Game Overview
- **Game Type:** 2D Platformer (Vertical Escape)
- **Objective:** Escape rising dangers by moving left and right while avoiding hazards.
- **Levels:** 5 progressively challenging levels.
- **Winning Condition:** Survive for the required time and reach Level 5.
- **Losing Condition:** Touching spikes or falling off-screen.

---

## 🔧 Future Improvements
- **Enhance Revival Visual Feedback**: Display time and level immediately after revival.
- **Improved Touch Control**: Fine-tune touch sensitivity and response.
- **New Obstacle Types**: Add moving spikes and falling hazards.
- **Power-Ups**: Temporary invincibility, speed boosts.
- **Sound Effects & Music**: Background music and jump effects.
- **More Levels**: Expand beyond 5 levels with unique mechanics.
- **Multiplayer Mode**: Competitive or cooperative gameplay.

---

## 📂 Project Structure

📁 **SkyEscape**
- **`📂 Assets/`** → *Main Unity asset folder (contains all game resources)*
  - **`📂 Animation/`** → *Stores animations (Player, Platforms, etc.)*
  - **`📂 Prefabs/`** → *Reusable objects (platforms, traps, player, UI elements)*
  - **`📂 Scenes/`** → *Unity scene files (game levels, menus, UI screens)*
  - **`📂 Scripts/`** → *All C# scripts responsible for gameplay mechanics*
  - **`📂 UI/`** → *HUD and UI components*
  - **`📂 TextMeshPro/`** → *Font and text settings for UI elements*
- **`📂 Packages/`** → *Unity package dependencies*
- **`📂 ProjectSettings/`** → *Unity project settings (input, rendering, physics)*
- **`📂 UserSettings/`** → *Editor-specific settings (not essential for version control)*
- **`📄 README.md`** → *Project documentation and setup guide*
- **`📄 .gitignore`** → *Specifies files to be ignored in version control*

---

## 💻 Installation and Running Guide

### Web Version
- Can launch in localhost.

### Android Version
- Download and install the APK on your Android device.
- Ensure you allow installation from unknown sources.
- Launch the game from your app drawer.

### Windows Version
- Download the executable and run the game.
- No installation required.
