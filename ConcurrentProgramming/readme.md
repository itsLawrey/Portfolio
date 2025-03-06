# Concurrent programming
Some practice in Java programming in the form of a small simulation game. See the text below for rules.

## Concurrent Programming Assignment: Sheep Farm
This project simulates a farm where shepherd dogs and sheep move around. The goal is to implement a program that models their movement, allowing a sheep to escape if it reaches a gate.

## Farm Class
* The farm is a rectangular area enclosed by walls, except for a few gates where sheep can escape. The area is represented as a matrix, where each cell can be of different types: empty space, wall, gate, shepherd dog, or sheep.

* The farm's width and height are each a multiple of three plus two, with a default size of 14×14. The area is divided into nine equal zones in both dimensions.

* There is one gate on each of the four walls, positioned randomly but never in a corner.

* By default, the simulation starts with 10 sheep and 5 dogs.

## Sheep Class
* Each sheep runs in its own thread.
The thread name (and the toString() method) should return an uppercase letter.
* All sheep start in the central ninth of the farm.
* Sheep pause for 200 ms between moves.
* Sheep detect nearby dogs (within adjacent cells) and move in the opposite direction:
If a dog is detected in a neighboring horizontal cell, the sheep moves in the opposite horizontal direction.
The same rule applies to the vertical dimension.
If no dogs are detected, the sheep chooses a random movement direction.
The sheep must move (its movement cannot be (0,0)) unless it is blocked by an obstacle.
* If a sheep steps onto a gate, it escapes, and the simulation ends.
## Dog Class
* Each dog runs in its own thread.
The thread name (and the toString() method) should return a number.
* Dogs start in one of the outer eight ninths of the farm and continuously patrol their area.
* Dogs pause for 200 ms between moves.
* Dogs choose their movement direction randomly.
The movement cannot be (0,0); the dog must move unless it encounters an obstacle or would enter the central ninth.
## Other Objects on the Farm
* Empty spaces and gates should return a " " (space) in their toString() method.
* Walls should return a "#" character.
* These objects have no other role in the simulation.
## Program Execution
* The program runs until a sheep escapes through a gate.
* The farm’s state is continuously updated in the console every 200 ms to show the movement of sheep and dogs.
* If a sheep successfully escapes, the program should display a message before terminating.
## Thread Safety
* The farm is considered a shared resource, and the moving objects (sheep and dogs) must use proper synchronization techniques to ensure thread-safe operations.

This project demonstrates concurrent programming, thread management, and synchronization techniques in a simulated environment.