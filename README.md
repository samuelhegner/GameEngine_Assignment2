# Final Submission

## Instructions
This repo uses Git LFS. If you don't have Git LFS you wont clone the occlusion culling data and might need to bake that file again, which takes forever. Clone the repo and run the unity files. If you want to just get a build of the project, use this link (https://drive.google.com/drive/folders/14-rAFAB9LGmUJ0SiW835FgkhwPP0SM9v?usp=sharing) to google drive for a mac and windows build.

## How it works
  This project attempts to replicate the Battle over Coruscant in Star Wars III in Unity using Boid Steering Behaviours and Ai Statemachines. The project plays like a movie that you can't interact with, however because of the procedural nature of it you will never get the same experience more than once. The scene starts like the movie does with two jedi starfighter flying over a Republican star destroyer. The yellow ship is following a basic path that starts at the republican destroyer and ends in the hangar bay of General Grievous' ship. The red ship just follows the yellow one with the Arrive behaviour. As the two ship nose dive into the ongoing battle. A large group of republican Arc-170 ship can be seen flying towards the enemy control ship. Out of all the ship, one starts of as the "leader" whilst the others aren't. The leader assigns all others a number which decides their position in the approach formation. The first two non leaders are assigned the numbers 1 and -1 the second two are assigned 2 and -2 and so on. The numbers are then used to calculate an offset position from the leader. This ends up looking like a large spread out V formation. These ship fly towards the number of approaching vulture droids. These droids just choose a random Arc 170 ship to approach. Once the ships are close enough, the ai state machine kicks in, which is essentially the same for both the Arc 170 ships and the vulture droids.

  The logic of the statemachines is fairly straightforward, however it looks complex when run. Essentially, the statemachine has 4 states each ship can be in, as well as one state specifically designed to allocate the next state of the ship. The first state is the ChaseDown state, which makes the ship chase down an assigned ship. It does this with a combination of the arrive and pursue behaviour. It enables pursue if the target is far away and switches to arrive when it is close enough. This means that the ship will get close to the target but never reach it. If its target is destroyed, the ship checks if it is currently being chased itself. If so it switches its current state into the EscapeEnemy state. Else it switches to the allocateState state which will assign a new objective.

  The second behaviour is the ShakeEnemy state lets the ship attempt to flee from his chaser. This behaviour works with a combination of two NoiseWander behaviours. One decides the vertical axis of movement and one decides the horizontal axis wander. I used two wonders to be able to control the intensity of the vertical which should be relatively smooth, whilst the horizontal wander should be more sporadic. If the chasing enemy is destroyed the state changes into the allocateState state.

  The third state is the helpAlly state which lets ships help allies in need. This behaviour is very similar to the first behaviour where the ship just chases down the the chasing enemy of an ally in need. If the ally the ship is destroyed, the state switches to Chase down the enemy. If the enemy chasing the ally is destroyed the ship is allocated a new state.

  The last state is the Wait wonder state, which is just a buffer state that is used if all other ship are busy and no allies need help. This state is rarely used, however is crucial to have the ship working in an infinite loop. This behaviour is the same and the Shake enemy behaviour in essence. The only difference is that the state automatically changes into allocateState after 5 seconds to attempt to assign a new role.

  The allocate State state is the state that cause me the most grieve. This state only has an state Enter function which switches to a new State by the end of the function. To start with the state create to lists of all ally and enemy ships. It also has to null gameObjects called NewEnemy and NewAlly. The function loops through all enemies to check if anyof them are doing nothing. If that's the case the new enemy is assigned to that enemy. IT then loops through all allies to check if any of them need help and that no one is helping them. If that is the case it assigns NewAlly to that ally. It then checks if NewEnemy has been assigned. If so, it changes the state to chase down that enemy and changes that enemies state to shake enemy. If NewEnemy hasn't been assigned, it then checks if the NewAlly has been assigned. If so, it changes the ship's state to HelpAlly state and lets the enemy chasing the NewAlly know that it is now being chased as well. If both NewEnemy and NewAlly remain unassigned, it changes the state to waitwander, where the ship will try again in 5 seconds.

 If all of that made no sense, hopefully this diagram will clear things up:
 
<img src="Pictures/StateMachine_Diagram.png">

  The Cameras are semi-procedural. There are 5 key events that override the dog fight camera spawner. The first event is the start approach of the two jedis. The second is the shot of the group formation approach of the Arc-170 ships. The third is the wide shot of the two groups of fighters clashing and starting their battle. The fourth shot is the missile being launched and the buzzdroids attaching themselves to the jedi starfighters. The last is the jedis landing in the hangar bay and the door closing. Between shots 3&4 and shots 4&5 the procedural camera spawner is active.
  
  The camera spawner works in simple way that can go on forever. It keeps track of the number of switches. If the number of switches is odd, the next camera will spawn on a droid ship. If it is even the next camera will spawn on a republican ship. Each ship in the scene has a front and a back camera position childed to itself. The camera manager will check all ally or enemy ships, if they are chasing someone or they are being chased. This roots out useless ships. It then checks if the Distance from the ship to the ship chasing or the ship being chased is smaller than the previous smallest amount. This roots out ships that are far away from each other and aren't close enough to be interesting to watch. Once it check all ships it will have the most ideal target to follow. It will then check if the ship is being chased or if the ship is chasing. If the ship is chasing something. It will spawn a camera on the backCamera Position of that ship. If the ship is being chased, it will spawn a camera on the front of the ship. The camera it spawn lerps its position to the rotation to the camera attachment points rotation and its position is equals to the position of the camera attachment point. If the ship being tracked is destroyed the camera will stop moving and look at the nearest ship of the opposite faction to the one it was just recently attached to. This gives the camera a nice flyby effect of the ship that just shot down the initial ship.
  
  The shooting of the ships is done in a way that it looks realistic i regardless of the state the ship is in. The shooting works off of coroutines that periodically shoot out sphere casts to check if any enemy ship is in front of the ship. If that is the case a bool shooting is switched on. Whilst this bool is true, the ship will shoot. This allows the ship to not only shoot things that it is chasing but to be aware of its surroundings and shoot at any viable target. This effect works quite well, however on the Arc170 ship it doesn't work as well as the vulture droids since the Arc 170 have an aiming element to their shooting.
  
  The last thing that isn't sort of self explanatory would be the missile key event. The missile has a little state machine of its own. After the missile is launched it Uses the SeekFront behaviour the seek a point in world space in front of a gameObject (In this case the yellow jedi ship). Once it gets close enough, it explodes and releases a number of Buzz droids. These will hover is space until they find something to attach to. The jedi ships will fly through the droids and if they collide with one of them, the droids get attached to the jedi ship.
  
## What I am most proud of in the assignment
The project as a whole was quite a challenge as a whole. I am proud of the fact that I got the dogfighting consistently well. This is the part that took by far the longest and required multiple rewrites and changes in planning. There are still occationally issues with more than one ship chasing another, but these are few and far between and don't change the experience. Even though the code behind the ai is quite straight forward, it still looks complex when watching it and in combination with the dynamic camera changer, the action is quite exciting. The camera changer and the set pieces like the missile bit and the final hangar section are cool aswell. It does feel like a movie. Since the battle is contrained to an area infornt of the jedi ships, there is always action to be seen and even 25 ships pers side feels like a big battle. Since the spawner continuously spawns new ships, the battle is as long as you want it to be and feels and looks nice. The whole scene looks great, thanks to excelent free to use models which can be found in the credits section as well as some basic post processing. The performance is a little disapointing but I did my best to optimise the scene with occlusion culling and some other changes. I'm also proud of the audio which required lots of editing to make them sound bareable to listen to on repeat. Here is a video of one simulated battle incase you want to see it in action without having to clone or download a build:


[![YouTube](http://img.youtube.com/vi/K9vNCZKBZ1Q/0.jpg)](https://www.youtube.com/watch?v=K9vNCZKBZ1Q)

## Credits

Models: https://sketchfab.com/doctor10?fbclid=IwAR33shoYLUgNjPiY1UD9cnE2rDpOR-l4gG68tH4dlXV9JG8CmZ0gr7M_Zc4
        https://sketchfab.com/3d-models/aim-120c-amraam-62b79b0f76e44684ad43adcc2ae3cdb9
        https://3dwarehouse.sketchup.com/model/71ce6a93-c5ac-462b-a24d-9ba19ef7ba0d/arc-170
        https://3dwarehouse.sketchup.com/model/92aad811b7f6bb6de616498194bbba1/vulture-droid
        https://3dwarehouse.sketchup.com/model/462c4dd4470c6eb039e51e642ae550d3/Jedi-Starfighter
        https://3dwarehouse.sketchup.com/model/cc84982281443c9439e51e642ae550d3/Jedi-Starfighter
