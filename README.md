# Very Generic Container for Unity3D
A simple extension for Unity3D to make collections of data easier to represent through GameObjects.

## Why you want this
You have a collection of data, for example a list of highscores, and you want to show them in a UI window. You'd have to create a seperate GameObject for every highscore entry and set the values like the player's name and the score. You'd wanna keep track of these GameObjects and maybe even pool them.

That's sounds easy (well, it is), but there are many more situations where you have a collection of data and show them in a list of GameObjects. And you don't want to write seperate code for each of these situations.

This is where Very Generic Container comes to save you. It handles all the instantiating, destroying and pooling of GameObjects. You just feed it a collection of data and Very Generic Container will handle the rest.

## Installation
Copy the **Assets/VeryGenericContainer** folder into the Assets folder of your project.

## Examples
See the **Assets/VeryGenericContainerExamples** folder for some simple examples.

## Usage
For our example we have a collection of ```Food```'s we'd like to show. We need a few things:
* The data class, which holds the data
* The ```ContainerItem``` class, which represents a single item in the container
* The ```Container``` class, which is the parent GameObject and holds all the items as childs
### The data class
Create the class which holds the data you want to show.
```c#
public class Food {

    public readonly string Name;

    public Food(string name) {
        Name = name;
    }

}
```

### The ContainerItem class
Now we will create the class that will serve as a component on the items of our container.
```c#
public class FoodItem : ContainerItem<Food> {

    [SerializeField] private Text labelText;

    public override void OnSetup(Food data) {
        labelText.text = data.Name;
    }

    public override void OnDispose() {
    }
}
```

### The ContainerClass
Now we will create the parent class which will hold all the individual items. It derives from `Container`.
```c#
public class FoodContainer : Container<FoodItem, Food> {
}
```

### Setting up the GameObjects
#### Create the container GameObject
Create the parent GameObject and add the ```FoodContainer``` script. 
![Create the container GameObject](https://user-images.githubusercontent.com/2270398/47284143-8bd53400-d5e6-11e8-9c65-8b767bc1b13e.png)
#### Create the container item GameObject
Create the item GameObject as a child of the container GameObject and ad the ```FoodItem``` script. This GameObject will serve as a template for all the items. In our example we also attached a Text to it so show the name of the food.
![Create the container GameObject](https://user-images.githubusercontent.com/2270398/47284144-8ed02480-d5e6-11e8-9a9d-c07b8662121a.png)
### Assign the template item
Assign the ```FoodItem``` component to the ```ItemTemplate``` field on the ```FoodContainer``` script. 

### Using the container
That's all! All we need to do is feed some data to the container. For this we use the ```UpdateContainer()``` method. For this example we call the method in the ```Start()``` method the of ```FoodContainer``` itself, but it can be done from everywhere.
```c#
public class FoodContainer : Container<FoodItem, Food> {
    private void Start() {
        List<Food> foods = new List<Food>();

        foods.Add(new Food("Apple"));
        foods.Add(new Food("Bread"));
        foods.Add(new Food("Pasta"));
        foods.Add(new Food("Chocolate"));
        foods.Add(new Food("Yoghurt"));

        UpdateContainer(foods);
    }
}
```
### Result
![Result](https://user-images.githubusercontent.com/2270398/47287408-6c440880-d5f2-11e8-80f1-e5e0925eab86.png)
