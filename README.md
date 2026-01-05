# Rock · Paper · Scissors · Lizard · Spock (Unity 6.3LTS)

A round-based, retro-styled implementation of **Rock–Paper–Scissors–Lizard–Spock** built in **Unity (C#)**.  
The project focuses on **clean game-loop architecture**, **event-driven UI**, and **strict separation of game logic and presentation**.

apk - https://drive.google.com/file/d/1dB8ATc0xC93xoP2Lb3ZrnmrRLdAmena8/view?usp=drive_link

<img width="421" height="358" alt="Screenshot 2026-01-05 at 9 48 05 AM" src="https://github.com/user-attachments/assets/f85c4051-72c0-4b2c-ae3e-02efa93a65c7" />

---

## 🎮 Gameplay

Each round follows a deterministic flow:
→ Round Start
→ Timer starts
→ Player selects a hand
→ AI selects a hand
→ Result is shown (timer paused)
→ Next round starts after delay

### Rules
- Player wins → streak increases, next round starts
- Player loses or timer expires → lose 1 heart
- Game ends when all **3 hearts** are lost
- Score = number of **consecutive wins**
- High score is stored locally using `PlayerPrefs`

---

## ⏱️ Timer Rules

- Timer runs **only during an active round**
- Timer pauses immediately when:
  - player makes a selection
  - result UI is displayed
- Timer fully resets at the start of every round
- Final **25% of time** triggers a flashing warning UI

Timer ownership lives entirely in **GameManager**.

---

## 🤖 AI Logic

- Uses a **rigged throw sequence** for early rounds
- Rigged data is consumed **once per lifetime**
- Falls back to random throws afterward
- Lifetime rounds tracked via `PlayerPrefs`

---

## 🖥️ UI System

### UI States
Implemented using a simple state pattern:

- `UiStartState`
- `UiGameplayState`
- `UiGameEndState`

Each state:
- Manages its own lifecycle
- Handles initialization and cleanup
- Contains no game-rule logic

### Gameplay UI Features
- Cycling “thinking” animations for player & AI hands
- Retro-style Play and Home buttons (PNG, transparent)
- Heart-based life display
- Round counter
- Result banner (`YOU WIN` / `AI WINS`) shown for a fixed delay

---

## 🧱 Architecture Overview

### Core
- **GameManager**
  - Single source of truth
  - Owns timer, rounds, streaks, hearts, AI decisions
  - Emits events for UI

### UI
- **UiManager**
  - Switches between UI states
- **UiGameplayState**
  - Displays gameplay data
  - Locks input during result display
  - Handles visual-only timing (animations, cycling)

### Data
- **ScriptableObjects**
  - `ThrowData` – throw relationships
  - `RiggedData` – AI rigged sequence
  - `GameConfig` – timer duration & settings

---

## 🛠️ Tech Stack

- Unity
- C#
- TextMeshPro
- ScriptableObjects
- PlayerPrefs

---

## ✅ Design Principles

- Single Responsibility Principle
- Clear ownership (logic vs UI)
- Event-driven updates
- Deterministic round lifecycle
- No UI-driven game rules
- Safe coroutine usage

---

## 🚀 Possible Improvements

- Difficulty scaling per round
- Sound effects & animations
- Pre-round countdown
- Online multiplayer
- Analytics hooks
- Possible StarTrek threamed UI
- Online Leaderboard
- AI to use an actual LLM like gemini to give out throws. 

---

## 📌 Notes

This project is intended as:
- A clean Unity architecture example
- A solid round-based game loop reference
- A foundation for further extension

No third-party assets required.
