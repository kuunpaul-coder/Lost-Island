# Lost Island

Android-first island expansion for Stardew Valley 1.6.15.3 / SMAPI 4.3.2.5.

## Current build: 0.1.0

The repository now includes a first Content Patcher test pack:
- `manifest.json` — pack metadata and Content Patcher dependency.
- `content.json` — loads the custom map and registers its location in `Data/Locations`.
- `assets/main-island.tmx` — initial 48×32 map using the game's outdoor tilesheet.

This is a **technical test build**, not the finished island. It does not yet include a boat entrance, custom tileset/art, the other planned zones, NPCs, quests, or treasure systems. The location registration and map still need an in-game SMAPI test on Android.

## Planned regions

1. Port and boat landing
2. Abandoned town
3. Jungle
4. Central square
5. Underground tunnels
6. Old mine
7. Lighthouse

## Target environment

- Stardew Valley 1.6.15.3
- SMAPI 4.3.2.5 (Android)
- Content Patcher 2.x

Android-safe assets and a simple ZIP installation workflow are priorities.
