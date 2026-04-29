# Forgotten People – Complete Game Description

## 1. Game Identity

**Title:** *Forgotten People*  
**Genre:** Online PvP 2.5D precision platformer (one‑button auto‑runner)  
**Core Theme:** Memory, identity loss, and the cost of recovering your past

**One‑line pitch:**  
*Steal liquid memory gems, merge them into purer recollections, and trade them to a mysterious doctor to earn back the faces and names of everyone you once loved – but every gem you hoard can be taken by rivals, slowing your journey to remember who you are.*

---

## 2. Story & Setting

### The Premise
You are **Subject 047**. You voluntarily entered a memory clinic hoping to recover lost childhood memories. Instead, the doctor – a brilliant but amoral neuroscientist – **erased every personal memory you had**: your family, your lovers, your friends, even your own name. You are a blank slate.

Now you are forced to raid the **Dreaming Vaults** – a shared unconscious space where people’s memories manifest as **liquid memory gems**. By collecting these gems, you can **buy back your own forgotten people**, one by one, from the doctor.

### The Doctor’s Deal
The doctor sits in a shadowy chamber. He does not explain why he erased you. He simply offers a transaction:

> *“Bring me gems. A certain score of gems. In exchange, I will return one person to your memory. First, a friend. Then a lover. Then a parent. The price doubles each time. When you have remembered enough… perhaps I will tell you why I chose you.”*

The player never truly trusts the doctor, but has no choice – his gems are the only currency that works.

### The Whisperer (for the Dark Ending)
After recovering a large number of memories (total gem score ≥ 1024), the player begins receiving garbled messages from a **second figure** – another erased patient who calls themselves **The Whisperer**.  
The Whisperer claims:
- The doctor is **planting fake memories** in your mind.
- Your real past may be completely different.
- The only way to know the truth is to **confront and kill the doctor** – and then silence the Whisperer, who also cannot be trusted.

This leads to an **optional dark ending** (triggered via a special menu choice). In that ending, the player kills the doctor, then the Whisperer, then turns the weapon on themselves – breaking the cycle of manipulated memory.  
After this ending, the player can reload their save and continue the open‑ended PvP game (the dark ending is a narrative branch, not a forced game over).

---

## 3. Characters

| Character | Role | Description |
|-----------|------|-------------|
| **The Player (Subject 047)** | Protagonist | A hooded, faceless figure. Runs automatically. No dialogue – a vessel for the player’s own search for identity. |
| **The Doctor** | Antagonist / Trader | Tall, wearing a white coat and a mirrored visor. Never enters the vaults. Calm, clinical, and utterly transactional. |
| **The Whisperer** | Optional third party | Appears only via text logs after very high score. Casts doubt on the doctor. |
| **Memory Guardian** | AI Enemy | A ghostly humanoid shape that patrols certain vaults. Cannot be killed – only avoided. Represents the emotional weight of stolen memories. |

---

## 4. Core Mechanics

### 4.1 One‑Button Gameplay
- **Jump** (Spacebar / tap) – the only manual input.
- Auto‑run, wall‑bounce on contact, wall‑slide when falling.

### 4.2 Memory Gems (Liquid Purity System)
- Gems are **viscous, glowing droplets**.
- Three purity levels:
  - **Impure** (Tier 1) – dark, slow, few particles. Score value = **1**.
  - **Clarifying** (Tier 2) – semi‑translucent, soft glow. Score value = **2**.
  - **Pure** (Tier 3) – bright, fast‑flowing, emits light motes. Score value = **3**.
- Visuals directly communicate purity: colour saturation, particle density, movement speed.

### **4.3 Distillation (Merging Gems)**  
- Combine **3 Impure gems** → **1 Clarifying gem**. 
- Combine **3 Clarifying gems** → **1 Pure gem**. 
- Distillation **reduces total score** (e.g., 3 Impure score 3 → 1 Clarifying score 2) but increases **purity**. Higher purity is required to purchase later memories from the doctor, creating a strategic trade‑off between score quantity and gem quality.
> Gems can be stolen only during the merging process.

### 4.4 Total Recollection Score
- Sum of the **score values** of all gems you currently possess (in your inventory, not yet spent).
- Displayed prominently in HUD.

### 4.5 Buying Memories from the Doctor
- In the **Doctor’s Chamber** (menu), you can spend a **gem** (or a combination of gems) to unlock a **memory vignette** – a still image + short text of a forgotten person.
- Each memory has a **price** (total score value required). The gem(s) you spend are **consumed** (removed from inventory).
- Once a memory is unlocked, it is **permanent** – you never forget that person again.
- Price scaling (exponential, but using **score thresholds** rather than strict gem count):
  - Memory 1 (friend): requires **total spent gems score** = 1 (i.e., one Impure gem)
  - Memory 2 (lover): requires additional 2 (cumulative 3)
  - Memory 3 (parent): additional 4 (cumulative 7)
  - Memory 4 (sibling): additional 8 (cumulative 15)
  - Memory 5 (childhood pet): additional 16 (cumulative 31)
  - … continues exponentially.
- The doctor shows your next required **total spent** score.

### 4.6 Online PvP: Raiding & Defending
- **Raid:** Attack another player’s vault (asynchronous). Success = steal **one gem** from their inventory (randomly selected from their lowest purity gems first).
- **Defense:** Design your own vault with traps. Other players will raid it. If they succeed, you lose a gem.
- When you **lose a gem** to a raider, your **Total Recollection Score** decreases by that gem’s score value.
- **Important – No forgetting:** Lost gems only slow your progress toward the *next* memory. Already unlocked memories remain intact. The doctor comments: *“You have been set back. Bring me more gems.”*

### 4.7 Three Trials per Vault
- Each vault (whether raiding or defending) gives you **3 attempts**.
- Failure = you lose any gems you collected during that run and are returned to the hub.

### 4.8 Shadow Merchant (The Doctor’s Courier)
- In the hub, you can trade **1 Impure gem** for **1 extra trial** (limited to 2 trades per day in the design, but you can set any limit).
- The doctor’s dialogue: *“A small sample for a second chance. Good business.”*

### 4.9 AI Traps: The "Echo Turrets"
To satisfy the AI requirement and enhance the PvP vault-building mechanics, players can place **Echo Turrets** in their vaults. 
* **Mechanic:** These static traps fire a slow-moving, glowing projectile that uses **Steering Behaviors (Seek/Pursuit AI)** to actively track the player's movement arc. 
* **Theme:** Visually, they look like floating, shattered mirrors. The projectiles represent "Intrusive Thoughts"—fragments of bad memories that actively home in on the player's consciousness.
* **Player Action:** The player must use early/late jump timing to alter their airborne trajectory and outmaneuver the homing AI.

---

## 5. AI Implementation – Memory Guardian

To satisfy the project’s AI requirement, a non‑player enemy **Memory Guardian** patrols specially marked vaults (you can have one in your demo level).  

**Finite State Machine:**

| State | Behaviour |
|-------|-----------|
| **Idle** | Stands still, slowly pulsing light. |
| **Patrol** | Moves along a predefined path (linear or circular). |
| **Hear** | If player jumps within a certain radius, guardian turns and moves toward the sound location. |
| **Chase** | If guardian sees the player directly (line of sight), it accelerates and homes in. Contact = lose 1 trial, player resets to start of current mission segment. |
| **Leash** | After losing sight of player for 3 seconds, returns to patrol route. |

**Why this fits the Memory theme:** The Guardian represents the **guilt and sorrow** of taking others’ gems (which symbolise their experiences). It is not evil – it is a mournful echo.

---

## 6. Level & Environment (Demo Level)

- **Single, continuous vault** combining both precision and stress sections (formerly “The Sawmill” and “The Citadel”).
- **2.5D perspective:** 3D models, 2D movement.
- **Lighting:** Dynamic – use coloured spotlights to suggest emotional tone (blue = melancholy, amber = nostalgia, desaturated = loss).
- **Memory symbols:** Floating fragments of photographs, broken clocks, shattered mirrors – all hinting at forgotten people and time.
- **Checkpoint:** After the first section, a glowing “memory anchor” acts as a respawn point.

---

## 7. User Interface & Menus

| Screen | Features |
|--------|----------|
| **Main Menu** | Play (raid random opponent) / Defend (test your own vault) / Doctor’s Chamber / Evolution (distill gems) / Leaderboard |
| **Doctor’s Chamber** | Shows unlocked memories (portraits), next required total spent score, button to pay gems (choose gem from inventory). |
| **Evolution Menu** | Drag‑and‑drop 3 gems of same tier to convert to 1 gem of next tier. |
| **In‑Game HUD** | Total Recollection Score, trials left, current gem purity (small icon). |
| **Doctor Pop‑ups** | After each raid, after memory unlock, after gem loss: one line of flavour text. |

---

## 8. Endings & Replayability

- **Default open‑ended:** Leaderboard continues forever. You can keep collecting gems, distilling them, and buying back memories – after the initial set of specific people, the doctor can offer generic memories (e.g., “a forgotten neighbour”, “a childhood toy”) with increasing exponential costs.
- **Dark ending (optional):** Once your **total spent score reaches 1024** (cumulative gems given to doctor), a new dialogue option appears: *“Confront the truth.”*  
  Choosing it triggers a short sequence (text + static images) where the player kills the doctor, then the Whisperer, then themselves.  
  The game then returns to the main menu with a “The End” message, but the save file can be reloaded before the confrontation, allowing players to continue PvP. This makes the dark ending a **bonus narrative** rather than a dead end.

---

## 9. Innovation & Creativity (For Your Final Report)

1. **Liquid purity system** – gems are not static; their visual and mechanical properties change with merging, reflecting the fluid nature of memory.
2. **Permanent memory unlocks** – once you remember a person, they stay. This creates a positive, additive progression loop, contrasting with typical PvP punishment.
3. **Doctor as both antagonist and trader** – he is never fought; he is negotiated with. This keeps the focus on platforming and resource management.
4. **Dark ending as optional lore** – players who invest enough time are rewarded with a thematically bold conclusion without alienating casual players.
5. **2.5D perspective + memory‑themed environmental storytelling** – floating photographs, mirrors, clocks.

---

## 10. Summary of Key Changes from “Get Gems”

| Aspect | Old (Get Gems) | New (Forgotten People) |
|--------|----------------|------------------------|
| Name | Get Gems | Forgotten People |
| Collectable | Gems | Liquid memory gems |
| Progression | Merge for higher score | Pay gems to doctor to remember people |
| PvP consequence | Loss of score, possible forgetting (old version) | Only loss of progress toward *next* person |
| Memories | Abstract | Specific people (friend, lover, parent, etc.) |
| Endgame | Leaderboard only | Leaderboard + optional dark ending |
