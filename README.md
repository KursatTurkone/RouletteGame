🎲 RouletteGame
A Modern & Modular Roulette Simulator built with Unity


📌 About
This is a feature-rich Roulette Game Simulator developed in Unity (C#), supporting all classic bet types, live animated wheel and ball, persistent statistics, and a modern event-driven architecture.
Built for code maintainability, modularity, and a great user experience.

📁 Folder Structure
Assets/
├── Scripts/
│   ├── BetSystems/          # Bet logic and interfaces
│   ├── Camera/              # Camera effects
│   ├── Managers/            # GameManager, BetManager, etc.
│   ├── RouletteSystem/      # Wheel, animation, helpers
│   ├── SaveSystem/          # Game state persistence
│   ├── UISystems/           # UI panels and statistics
│   └── Utils/               # Helpers, events
├── ScriptableObjects/       # Game configs (RouletteConfig)
└── Scenes/                  # Main and demo scenes

🚩 Key Features
✔️ All Classic Bet Types: (straight, split, corner, dozens, red/black, even/odd, etc)

✔️ Animated Wheel & Ball: Realistic visuals and motion

✔️ Bankroll & Statistics: Track money, wins/losses, profit, and more

✔️ Persistent Progress: Auto-save and instant resume

✔️ Modern UI: Responsive, interactive interface and notifications

✔️ Event-driven Architecture: Loosely coupled and easily testable modules

🚀 Getting Started
Clone this repo

git clone https://github.com/KursatTurkone/RouletteGame.git
Open in Unity Hub (Recommended: Unity 2021.3 LTS or newer)

Install dependencies via Unity Package Manager
(TextMeshPro is required; others are optional)

Open the main scene:
Assets/Scenes/GamePlayScene.unity

Press Play! 🎮

🕹️ How to Play
Interface Overview

Top-Left: Statistics — View all your play history and stats.

Top-Center: Current Balance — Your available money.

Top-Right: Number Dropdown — Pick a specific outcome or set to Random for normal play.

Center: Roulette Table — Click any number or bet area to place a bet.

Left: Roulette Wheel — Spins visually, animating the ball.

Bottom-Center: Bet Amount Selector — Adjust your bet with arrows.

Bottom-Left: SPIN — Start the spin!

Bottom-Right:

Clear Bets: Remove all bets

Give Money 10k: Add 10,000 for testing/demo

Steps to Play

Adjust bet amount using the arrows.

Click any table cell, color, or special area to place a bet.

(Optional) Use the dropdown (top-right) to select a deterministic result.

Press SPIN to play!

The wheel spins, results show instantly, and your stats are updated.

Tips

Auto-save and resume — continue your game anytime.

Use the dropdown for test cases or demo purposes.

Track your progress in the Statistics panel.

👨‍💻 Code Guidelines
Each ScriptableObject lives in /ScriptableObjects

Every interface/class has its own descriptive file

No DI frameworks — all dependencies are manually and clearly set

English naming and clean, maintainable code

Event-based, decoupled design for flexibility

📈 Statistics & Save System
Auto-save: Progress, bets, stats, and preferences saved on every update

Resume: Return right where you left off — instantly

Statistics UI: Tracks all spins, wins, losses, profit/loss, and more

📜 License
This project is for demonstration purposes only.
Not intended for commercial use.

Author:
Ömer Kürşat Türköne
Unity Game Developer

