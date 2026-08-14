# CyberEvolution

## Preview

<img width="800" height="451" alt="Cyber-ezgif com-video-to-gif-converter" src="https://github.com/user-attachments/assets/0363cea3-083d-467b-a674-236b2cc9a432" />

## Project Description

CyberEvolution is a simulation project based on a genetic algorithm. It simulates a world populated by mobs that follow a set of simple commands written in their genomes.

The following rules apply:

+ Each tick, a mob must perform a command.
+ Mobs can perform `DoNothing`, `Turn`, `MoveForward`, `Consume`, and `Attack` commands.
+ Whenever a mob performs any kind of command, its energy is depleted.
+ Different commands have different energy costs.
+ If a mob's energy reaches 0 at any point during the simulation, it dies.
+ Mobs can replenish their energy by eating food.
+ Different types of food restore different amounts of energy.
+ Whenever a mob accumulates a sufficient amount of energy, it reproduces.
+ When a mob reproduces, half of its energy is passed to its offspring.
+ Every time a new offspring is spawned, it has a chance to mutate.
+ When a mob mutates, a new genome is created that is identical to its current genome, and one random command is then replaced with another random command.
