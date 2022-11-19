/*

swallowed Holl Teleporters neeed some work, NPCs cant tp through them and sometimes the player will clip through the wall behind them when using it repeatedly

stuff I think needs work, or needs to be implemented, or whatever. just a to do list basically. THis list will likely just keep getting bigger and bigger

NPC animation controller needs work, when theyre right between running and walking speed its this akward thing where they start and stop over and over, make it blend between the two instead

something is causing the face anim controller to fail, find it ( it seems like it was only blonde kaylas having this issue, i may have fixed it but hold off for now )

the NPCs get confused around balconies and edges of high places

small issue with physics objects being in a container scene, when i pick up and drop an objects it gets placed back in the default scene, should be an easy fix

the way that BBallHoop, BBallHoop2, and grab all independently build a list of all tha balls in a level is redundant. should probably just do it once on the player then have everything else reference that. 

the gibs system for breaking props can be optimized a bit too to handle mass breaking better. maybe check to see how many gibs there currently are spawned already then set a cap to how many can be rendered at once. then the shatter script can vary how many gibs it spawns depending on that

need to make a "landing" animation

switch the logic on stuff being climbable. by default everything should be unclimbable, then we make exceptions. 

make responding to gravity shifts work in first person too. check the UpdateRotation script

consider a switch to cinemachine for the camera?

make wall climbing jumps contextual. climbing idle jump-> jump is perpendicular to wall, climbing left jump -> jump along wall to the left, etc

fall damage

little skid when you rapidly change directions?

interactable NPC's. Speaking, roaming, fighting, etc.

let me ragdoll any NPC and myself

platform that dissapears when you jump on it 

Make an ice area of the level

make a variety of different types of doors 

implement the "OVERFULL" mechanic, big physics ball character

Holographic Hamburger!

Food Dispensers

make conveyor belts or doors or whatever dependent on a battery, very dishonored styled

MAKE SOME LEVELS

sometimes i just have to change sphere physics material from minimum to multiply and idk why it just like changes sometimes idk

figure out how to make a material togglable transparent (Poyomi may make this easier!)

*/