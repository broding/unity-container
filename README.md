# Very Generic Container for Unity3D
A simple extension for Unity3D to make lists and collections of GameObjects easier to create and manage.

## Why you need this
You have a collection of data, for example a list of highscores, and you want to show them in a UI window. You'd have to create a seperate GameObject for every highscore entry and set the values like the player's name and the score. You'd wanna keep track of these GameObjects and maybe even pool them.

That's sounds easy (well, it is), but there are many more situations where you have a collection of data and show them in a list of GameObjects. And you don't want to write seperate code for each of these situations.

This is where Very Generic Container comes to save you. It handles all the instantiating, destroying, pooling and managing of your collection of GameObjects. You just feed it a collection of data and Very Generic Container will handle the rest.

## Installation
Copy the **Assets/VeryGenericContainer** folder into the Assets folder of your project.

## Usage
See the **Assets/VeryGenericContainerExamples** folder for some simple examples.
