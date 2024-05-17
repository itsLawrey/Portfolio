# Explo-Ducks

## Contributors
* Molnár Eszter
* Börzsei Marcell
* Kerényi Gergő
* Dallos Loránd (me)

## About
This is a personal take on the Bomberman game, made by four people, including myself. This was our first work in Unity, so it's not perfect. 

The project was made following the Scrum method, keeping weekly meetings, developed on GitLab.

## The Game
### Game Description
The game is played on a 2-dimensional map composed of square tiles. It is a 2-player game where each player controls a bomberman figure with the objective of being the last one standing. The game map contains wall elements, boxes, monsters, and the player figures. Players can place bombs to blow up boxes, monsters, and even other players (including themselves). A player loses (and their opponent wins) if they explode or are caught by a monster.

### Controls
| | Player 1 | Player 2 |
|---------|-----------|-----------|
| Movement | Arrow keys | WASD |
| Place Bomb | Enter | Space |

### Walls
Wall elements are indestructible and, with some exceptions (detailed in optional tasks), cannot be traversed by players or monsters. The game map is surrounded by walls, preventing players from leaving the map.

### Players
Each player controls a bomberman figure, which can move left, right, up, and down. Players can also place a bomb on the tile they are currently on. A player can only have one placed bomb at a time (see the Boxes section for further details). Players control their characters using the keyboard.

### Bombs
Placed bombs explode after a short period, affecting the current tile and extending in all four directions (left, right, up, down) by 2 tiles. Walls are indestructible and block the blast. Boxes can be destroyed but will stop the explosion from spreading further. Bombs can trigger each other, creating a chain reaction. The explosion spreads quickly but not instantly, so the blast effects spread with a small delay proportional to the distance from the bomb. The explosion destroys players and monsters on the affected tiles. With a few exceptions (detailed in optional tasks), neither players nor monsters can step on bombs. Players can place a bomb under themselves and then move off the tile, but cannot move back onto it while the bomb is still there.

### Boxes
Boxes are destructible, and with some exceptions (detailed in optional tasks), neither players nor monsters can step on them. When a box is destroyed, it may reveal a power-up, which grants a bonus to the player who picks it up. Possible bonuses include:
* **Number of Bombs**: The number of bombs the player can place increases by 1.
* **Explosion Range**: The range of the player's bombs increases by 1 tile in all 4 directions.

### Monsters
Monsters roam the game map and kill any player they touch. In the basic task, monsters use simple heuristics: they change direction randomly when they encounter an obstacle. Occasionally, they change direction unpredictably to make their movement less predictable.

### End of the Game
If a player explodes or is caught by a monster, the game continues for a short period (e.g., the remaining time of the bomb's timer). If the other player survives during this time, they win the game. If the other player also dies, the game ends in a draw.

### Starting the Game
At the start of a new game, players can choose from at least 3 pre-made maps. The positions of walls and boxes are predefined, but the power-ups (and their types) within the boxes are determined dynamically at runtime. The maps can be rectangular or other shapes. Players can also set the number of wins needed to end the game. After each game, the current score and the number of wins for each player are displayed. (A draw counts as a loss for both players.) The final result is shown after the last game.

### Additional Features
#### Intelligent Monsters
The game can include different types of monsters, each with unique speed and heuristics. At least 4 different types of monsters should be implemented:
1. The monster from the basic task.
2. A similar monster that can pass through walls and boxes (but not bombs). If it encounters an obstacle and there is a clear path ahead, it may continue without changing direction. It moves slower than the basic monster.
3. A similar monster that changes direction when encountering an obstacle and heads towards the nearest player based on the shortest path. It moves faster than the basic monster.
4. A monster that can change direction at intersections using the described heuristic. It may make wrong decisions and take a wrong path. It moves at the same speed as the basic monster.

#### Advanced Power-Ups
In addition to the 2 power-ups defined in the basic task, there should be more complex bonuses available from destroyed boxes:
* **Detonator**: The player's bombs do not explode due to a timer but can be detonated by pressing the bomb placement key again.
* **Roller Skates**: The player's movement speed increases. This is not stackable; additional roller skates have no effect.
* **Invincibility**: The player's character becomes invincible for a short time. This should be visually indicated, and also show when it is about to end.
* **Ghost**: The player's character can pass through walls, boxes, and bombs for a short time. This should be visually indicated and show when it is about to end. If the player is on a wall or box tile when the ability ends, they die. (They can move off bombs.)
* **Barrier**: The player can place obstacles that act like boxes but never contain power-ups. A maximum of 3 barriers can be placed by a player. (If destroyed, new ones can be placed.) This ability is stackable, allowing for up to 6 barriers if picked up twice, etc.

#### Hindering Power-Ups
Boxes can also contain hindering "power-ups". These should not be distinguishable from beneficial power-ups before being picked up. Possible hindering effects include:
* The player's movement speed decreases for a time.
* The range of the player's bombs is reduced to 1 tile for a time.
* The player cannot place bombs for a time.
* The player automatically places bombs as soon as they can (if standing on a free tile and able to place a bomb). This effect lasts for a limited time.

#### Power-Up Interactions

| | Plus Bomb | Plus Radius | Detonator | Speed Bonus | Invincibility | Ghost | Barrier | Slow Power-Up | Reduced Radius | No Bomb | Place Bomb |
|-------------|-------------|-----------|-------------|---------------|-------------|-------------|-------------|---------------|---------------|-----------|-------------|
| **Plus Bomb** | - | - | Detonator works normally | - | - | - | extra bomb but barrier count remains | - | can't place bombs | place more bombs |
| **Plus Radius** | - | - | - | - | - | - | - | radius = 1 | - | - |
| **Detonator** | place extra bombs | - | no extra effect | - | - | - | first placement detonates | - | sucks | can detonate bombs |
| **Speed Bonus** | - | - | - | no further increase | - | faster ghosting | - | revert to normal speed | - | - | - |
| **Invincibility** | - | - | - | - | timed | very OP | - | - | - | - | - |
| **Ghost** | - | - | - | faster ghosting | very OP | timed | can place barriers | slower ghosting | - | can place barriers | |
| **Barrier** | - | - | - | - | - | can place barriers | barrier count increases | - | can place barriers | can place barriers |
| **Slow Power-Up** | - | - | - | no effect | slower ghosting | - | timed | - | - | - |
| **Reduced Radius** | - | no effect | - | - | - | - | - | timed | radius = 1 | no bomb | - |
| **No Bomb** | no effect | picks up | remains until detonated | - | - | - | can place barriers | - | no bomb | timed | no bomb placement |
| **Place Bomb** | place more bombs | - | can detonate | - | don't die to own bombs | can place barriers | - | - | - | timed |

#### Continuous Movement
Players and monsters should move smoothly between tiles on the game map. Logically, characters can still be on a single tile at a time, but moving between tiles should require multiple key presses (or holding down the key).

#### Customizable Controls
The game should allow players to customize the key mappings for their controls. These settings should be saved and persist across game sessions.
