Coding Weeks 2023-2024
=

# Introduction
This Unity project in C# is a 2D arcade platformer.

The player's objective is to pass through the door on the last _(2nd)_ level. To open the doors on each level, which lead to the next level, the player must kill a certain number of AIs on the map.

# Contents :

1. MVPs / Roadmap with role allocation
2. MVC 
3. Use cases
4. Functionality description (to be explained)	
5. Instructions for installing and launching the game	



# 1.MVPs / Roadmap 

Here's how we've prioritized the construction of our MVPs:

➤ MVP1: generate a 2D terrain (map) procedurally. We also want to integrate ambient elements into our map (trees, caves, hollows, slopes...) distributed semi-randomly across the map. This will help build the overall ambience of our game, while adding the major advantage of being attractive, since each time you play a new game, the map will be different. **(Erwan / Martin / Lucas)**

➤ MVP2: The main character must appear on the field. At the end of this stage, the user will be able to control the player's movements using the keyboard keys. Only character movement will be included in this step (right translation, left translation and vertical jump). The keys assigned will be the space bar for jumping and the arrow keys for vertical movement. **(Hammam / Yani)**

➤ MVP3: The camera will now follow the character's movement on the map. **(Lucas / Erwan)**

➤ MVP4: The main character will automatically appear at the far left of the map. The player's main quest will take shape at this stage. A door at the far right will be created, corresponding to where our character must go. Once through the door, the level is won. We plan to create several levels in a future MVP. In this way, the door will also be the means to access subsequent levels. **(Erwan / Tristan)**

➤ MVP5: We'll be adding two types of enemy next. On the ground, a category of monster will follow the player into a user-defined detection zone. When this monster is blocked by a block on the map, it will need to be able to jump by itself to clear this obstacle. In the air, a flying monster will follow the player continuously, but at a slower speed than the player.
When creating these enemies, we'll sketch out the monsters' first interactions with our character, the aim being for them to detect and attack the main character. Player and monster hit point losses will be taken into account in a later MVP. **(Tristan / Hammam / Martin)**

➤ MVP6: We'll create a feature that fills the role of 'Manager', allowing a user-defined number of monsters to appear on the map. **(Lucas / Tristan)**

➤ MVP7: We then add complementary interactions between our character and enemies.  During this phase, the player will be equipped with a life bar. He will then lose health points when colliding with one of the monsters presented in MVP4. Once hit, the character will have an immunity phase, the duration of which can be adjusted by the user. This invincibility will be characterized by the character blinking. **(Tristan / Martin)**

➤ MVP8: In this phase, we'll give our character the ability to inflict damage on enemies. To do this, we'll create weapons that can be assigned to the main character by user choice (with the option of swapping weapons). These weapons will enable the character to fight enemies. In this way, the character's quest is completed: in addition to reaching the door, he or she must kill a certain number of enemies for the door to the next level to open. **(Erwan / Yani)**

➤ MVP9: In this phase, we'll give the player the chance to mine blocks of earth using a pickaxe.  **(Erwan / Yani)**

➤ MVP10: When a character's life level (player or enemy) becomes zero, it will be automatically destroyed. **(Martin / Hammam / Tristan)**

➤ MVP11: Next, we'll add the interaction interface between the game and the user. This includes: 
Main menu (Play, Credits, Game buttons, Quit)
During the game, display of the number of monsters remaining to be killed before the door opens.
A “Level Passed” or “Lost” message is displayed, with the option of returning to the main menu if necessary. 
**(Lucas / Martin)**

➤ MVP12: In this step, we elaborate the different levels as desired from MVP4. For the difference in levels to be significant, several parameters will have to change. Firstly, the environments will be modified (snow, night, etc.). Secondly, to increase difficulty from one level to the next, the number of monsters will be increased. **(Erwan)**


➤ MVP13: We'll assign character and enemy animations: 
Setting characters in motion as they move.
When a character has no life left, it is destroyed by decomposing
**(Yani / Martin / Erwan)**

➤ MVP14: During this phase, we'll develop sound effects that will be assigned to certain stages of the game (growling when monsters move, sound effects when weapons are used, sound effects when a character is hit and a different sound when a character dies). **(Lucas / Hammam)**

➤ MVP15: Creation of background with parallax effect with cloud displacement. Particles added when an entity dies. (Erwan / Yani)



# 2.MVC 

An example of MVC architecture for the GameObject “Player” is available in the presentation “LOTEREO_CW_gp_17.pdf” available on this repository.

An MVC (Model View Controller) breaks down game logic into the game model (model), its graphical representation (view) and its control logic (controller). In the general case, the specifics of these three components are: 

➤ The model: it stores the values calculated by the controller in memory;

➤ The controller: performs real-time calculations of the game parameters it sends to the model. It governs all game operation, taking into account commands imposed by the user;

➤ The view: this is a graphical representation of the model, adapted to the user and contributing to the game's playfulness. This representation evolves as the game parameters change.
In our case, the very use of Unity imposes an MVC logic. Scripts are assigned to graphical objects. These scripts take into account parameters that are bound to evolve over the course of the game. The controller manages these calculations in real time. These objects participate in the visual representation of the game and interact with each other according to the parameter values stored in the model.



# 3.use case

The target audience for this game is individuals aged 16 to 50. No video game experience is particularly required. This game is ideal for people who enjoy pixel art. 

In this game, players must challenge themselves to pass the levels and become the LOTEREO champion. The game's ambition is to entertain its user and push him or her to the limit in the face of bloodthirsty enemies.

The game mechanics are relatively simple. The number of monsters increases with each level. You'll need more concentration and perseverance to win.

The game is complete with sound effects and animations that add to its beauty. It's ideal for delighting all the senses. 

LOTEREO has almost infinite replayability, thanks to procedural map generation. Players will never get bored.

Immerse yourself in a fantastic universe and enjoy a unique experience!



# 4.feature descriptions

Player movement: right ←, left →, jump “esp”, shoot “E”, display menu “escape” and change tools/weapons “tab”.

We can adjust many of our game's features through free parameters, namely: 

- Cellar size, vegetation density, water density and map width;
- The position on the map of the door to be reached;
- The number of monsters that appear on the map at each level;
- The player's jump height, movement speed, invincibility time when hit and maximum number of lives;
- The gravity imposed on shotgun bullets;
- The player's movement speed, jump height, detection distance, the amount of damage inflicted on the player, and the jump probability of land monsters;
- Monster movement speed and player damage.



# 5.Instructions for installing and launching the game

You have 3 options for launching the game: 

➤ Play the “Web page” version by clicking on the following link: https://play.unity.com/mg/other/webgl-6se

➤ Clone the repository to obtain the executable version, then click on the executable .exe file in the “Game” folder.

➤ Clone the repository, open the “2D tile map” project on Unity version “2002.3.13f1”, then press Play.
