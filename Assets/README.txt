this project is for educational purposes only and is not to be released in the public domain.

As this is a prototype game, you are likely to encounter visual and mechanical bugs (errors not ants).

You will not be penalised for these but please do contact me at s.sturtivant@bathspa.ac.uk and I can hopefully fix the issue.

Because  this project uses a scripted FMOD Studio Listener you are likely to see the following error:

'[FMOD] Please add an 'FMOD Studio Listener' component to your a camera in the scene for correct 3D positioning of sounds.'

you can ignore this. The custom listener is set up so that the spatialised audio sources are panned relative to the camera but the volume of the source is attenuated based on the players position.

**********************************************************************************************************************
***YOU ARE NOT TO CHANGE ANY OF THE VISUAL OR MECHANICAL ELEMENTS IN THE PROJECT AND DOING SO WILL IMPACT YOUR GRADE***
*********************************************************************************************************************

Examples of this would be:

"I want the character to jump so I will edit the code to make the character jump". 
or "
I want the characters outfit to be bright pink so I am going to change the chars material".

The only case where project amendements can be made are if they relate to the games audio 

Examples of this would be:

"I feel that the rain SOUND is programmed to come in too early, I am going to make changes to the c sharp script to make it come in later"
or

"I want to add a new FMOD Sound Event with a parameter that allows me to hear the players clothes rustle when they move".

"I want to add more than one studio listener as I don't like the way the listener is set up in this project"

Controls: 

Controlling the droid:

Right Click - remote control droid - the droid will follow the crosshair (cursor).
MouseScroll - Select tool type (laser, plantScanner, seedPlanter)
Left Click (whilst controlling droid) - depending on the tool that is selected - fire a projectile (laser), scan a cactus(plantScanner), deposit a seed (seedPlanter)

Moving the Character:

WASD 
Mouse for camera orbit 

Enemies:

There is only one enemy in this example project (Goldworm Sandworm) - the enemy can be damaged by the laser tool.

Ants:

There is an ant colony in the scene. 10 % of the ant colony go further and when they encounter a foodsource (1 of the four cactus') they will bring the
food back to the hive and leave a trail. When other ants discover this trail they will follow it to the foodsource and then bring it back to the hive.

When the hives foodcount goes up, a procedural tree grows above the hive.


The assets used are from the following sources:

Unity Asset Store:

https://assetstore.unity.com/packages/3d/characters/humanoids/sci-fi/stylized-astronaut-114298
https://assetstore.unity.com/packages/vfx/particles/polygonal-s-low-poly-particle-pack-118355
https://assetstore.unity.com/packages/vfx/particles/environment/rain-maker-2d-and-3d-rain-particle-system-for-unity-34938
https://assetstore.unity.com/packages/vfx/particles/cartoon-fx-remaster-free-109565

Standard Unity Asset Store EULA

Github:

https://github.com/SlightlyMad/VolumetricLights
https://roystan.net/articles/toon-shader.html

Sketchfab:

https://sketchfab.com/3d-models/backpack-low-poly-d7225bdd1d6c42d992485d72438cab28
https://sketchfab.com/3d-models/360-sphere-robot-a0bd28b7133648848427a5c27975611b
https://sketchfab.com/3d-models/gold-sandworm-41f03702289940b29831dcb771fb5f3a
https://sketchfab.com/3d-models/low-poly-grain-pile-e267b67bb983452fb12cbd3d0478a8ae
https://sketchfab.com/3d-models/scifi-station-low-poly-3d-art-construction-kit-f907311bd4744e97a577d5637454ac2a
https://sketchfab.com/3d-models/medievalfantasy-watchtower-on-an-island-c2337e347d8c4909aa24674629deb81c
https://sketchfab.com/3d-models/gp-5-gas-mask-d46e51606d4447f3b2127b3386799460
https://sketchfab.com/3d-models/lowpoly-stylized-3d-icons-in-the-jar-8682e434fb5748309e7dc9ca453ac69c

renderhub:
https://www.renderhub.com/m4rios/cactus-set-free-low-poly-3d-model

Sprites:

https://dribbble.com/shots/4740095-Low-Poly-Book
https://www.turbosquid.com/3d-models/3d-sunflower-seed-1337345

Github

https://github.com/keijiro/PerlinNoise