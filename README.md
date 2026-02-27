This branch contains **compiled desktop builds** of the game for:

- Windows  
- macOS  
- Linux
- Web

# Play Online (Web Version)

You can play the WebGL version directly in your browser:

**https://matej987.itch.io/mksafenet**

No installation required.

---

# 📁 Project Structure (Master Branch)

```

Game/
│
├── windows version/        # Windows build
│   ├── MkSafeNetGame.exe
│   └── MkSafeNetGame_Data/
│
├── macos version/          # macOS build
│   ├── mac game.app
│   └── (Unity runtime files)
│
├── linux game/             # Linux build
│   ├── linux game.x86_64
│   └── linux game_Data/
│
└── README.md

```

---

# How To Run The Game

## Windows

1. Open the folder:
```

windows version/

```
2. Double-click:
```

MkSafeNetGame.exe

```
3. If Windows shows a security warning:
- Click **More Info**
- Click **Run Anyway**

---

## macOS

1. Open:
```

macos version/

```
2. Right-click on:
```

mac game.app

```
3. Click **Open**
4. If macOS blocks the app:
- Go to **System Settings → Privacy & Security**
- Click **Open Anyway**

Since the app is not signed with an Apple Developer certificate, macOS may show a warning. This is normal for unsigned builds.

---

## 🐧 Linux

1. Open:
```

linux game/

````

2. Make the executable runnable:

```bash
chmod +x "linux game.x86_64"
````

3. Run the game:

   ```bash
   ./linux\ game.x86_64
   ```

---

# Built With

* Unity Engine
* C#
* Unity Addressables
* Unity Localization System
* WebGL & Standalone Builds

---

# Notes

* This repository contains compiled builds, not the Unity source project.
* Large files are included due to Unity runtime dependencies.
* For the full Unity project source files, please contact the author.

---
