/*

stuff I think needs work, or needs to be implemented, or whatever. just a to do list basically. THis list will likely just keep getting bigger and bigger

the way that BBallHoop, BBallHoop2, and grab all independently build a list of all tha balls in a level is redundant. should probably just do it once on the player then have everything else reference that. 

the gibs system for breaking props can be optimized a bit too to handle mass breaking better. maybe check to see how many gibs there currently are spawned already then set a cap to how many can be rendered at once. then the shatter script can vary how many gibs it spawns depending on that

if you press forward then backward very quickly afterwards youll continue moving forwards just really really slowly. also applied to climbing

need to implement climbing animations, use a 2d blend tree!

need to make a "landing" animation

switch the logic on stuff being climbable. by default everything should be unclimbable, then we make exceptions. 

make responding to gravity shifts work in first person too. check the UpdateRotation script

need to set up sneaking animation

consider a switch to cinemachine for the camera?

make wall climbing jumps contextual. climbing idle jump-> jump is perpendicular to wall, climbing left jump -> jump along wall to the left, etc

fall damage

little skid when you rapidly change directions?

interactable NPC's. Speaking, roaming, fighting, etc.

let me ragdoll any NPC and myself

platform that dissapears when you jump on it 

Make an ice area of the level

make a variety of different types of doors

SOUND EFFECTS! need to learn how to make an "array" of sounds that will be randomly chosen from to put a little variety in there, as well as finding the right settings to make stuff sound good. ie, when there are more than like 5 things all making the same noise we can probably mute a lot of them without much of a side effect. 

implement the "OVERFULL" mechanic, big physics ball character

Holographic Hamburger!

Food Dispensers

CONVEYOR BELTS!

make conveyor belts or doors or whatever dependent on a battery, very dishonored styled

MAKE SOME LEVELS

sometimes i just have to change sphere physics material from minimum to multiply and idk why it just like changes sometimes idk

figure out how to make a material togglable transparent (Poyomi may make this easier!)

need to make a death state

*/