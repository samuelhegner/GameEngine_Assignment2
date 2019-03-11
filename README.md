# GameEngine_Assignment2: Battle over Coruscant
## The origin
The original spacebattle that will be depicted in this assignment, comes from the Star Wars universe and can be seen in "Star Wars: Episode III – Revenge of the Sith". The bellow video shows the battle in movie form.

[![](https://img.youtube.com/vi/ZZq53GloUhw/0.jpg)](https://www.youtube.com/watch?v=ZZq53GloUhw)

## The Goal
The goal of this assignment is to get a completely autonomous space battle to play out while two Jedi Interceptors to follow a long path through the carnage. One of the Jedi’s will be following a designated path whilst the other offset pursues the first. 

The space battle will have two separate parts. The one part is between Droid and Republican star destroyers. These will slowly fly past each other and shoot their space cannons. This should play out similarly to naval battles where the ships can only shoot each other if they are beside each other. 

The second part will be the republican and droid starfighters. A dog fighting system will be inplace where the ships will start chasing each other. The chaser will try to shoot its target whilst the target tries to evade the pursuer. Ships that have nothing to do will look for allies to help out.

To track the battle a fully autonomous cameraman ai will be inplace to create and place unity cameras where ever the action is. The cameras will also use steering behaviours to follow ships and show the action from many interesting angles.

## The Storyboard
<img src="Pictures/Storyboard_Cam.jpg">

## What is there now
The models that will be used can be found in the 3d art creation scene. They are sized to a somewhat accurate scale. Dogfighting will happen between the Arc-170s and the Vulture droids. Sadly I wasn't able to find a good model for the Tri-Droid fighters. Two separate star destroyers form the Republican and the Droid side will take part in a battle of their own. The two Jedi starfighters will be used as the main protagonists in the scene. The Providence model will be static and serve as scenery.

In the Main scene, you can see the start of the project with one starfighter following a basic path. This is trying to mimic the scene from the movie above.

The next step is to lay out the scene properly and then create a long path through it all for the Jedis to follow. Then the Ai dogfighting will take priority. This will be the hardest and most important thing to get right. Then the cameraman Ai will be put in place to capture the fight from different angles. Lastly, if there is time, I will look into seamlessly splice footage of the film into the scene to try to recreate the battle as accurately as possible.
