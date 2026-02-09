using UnityEngine;
using NUnit.Framework;

public class StarSpawnerTest
{
    private StarSpawner spawner;

    private Star mainStar;
    private Star neighbour1;
    private Star neighbour2;

    [SetUp]
    public void Setup() {
        var gameObject = new GameObject();
        spawner = gameObject.AddComponent<StarSpawner>();

        //Stars
        var mainStarGO = new GameObject();
        var neighbour1GO = new GameObject();
        var neighbour2GO = new GameObject();

        mainStar = mainStarGO.AddComponent<Star>();
        neighbour1 = neighbour1GO.AddComponent<Star>();
        neighbour2 = neighbour2GO.AddComponent<Star>();
    }

    [Test]
    public void GalaxyShape_UpdateShape() {
        //Arrange
        StarSpawner.GalaxyShape expectedShape = StarSpawner.GalaxyShape.Concave;
        spawner.galaxyShape = expectedShape;
        //Assign
        StarSpawner.GalaxyShape actualShape = spawner.galaxyShape;
        //Assert
        Assert.AreEqual(expectedShape, actualShape);
    }

    [Test]
    public void DropDown_UpdateShape() {
        //Arrange
        StarSpawner.GalaxyShape expectedShape = StarSpawner.GalaxyShape.Bowl;
        spawner.DropDownMenuChoice(3);
        //Assign
        StarSpawner.GalaxyShape actualShape = spawner.galaxyShape;
        //Assert
        Assert.AreEqual(expectedShape, actualShape);
    }

    [Test]
    public void SwitchRadius_ChangesRadius() {
        //Arrange
        float expectedRadius = 15f;
        spawner.galaxyShape = StarSpawner.GalaxyShape.DoubleHelix;
        //Assign
        float actualRadius = spawner.CheckRadius();
        //Assert
        Assert.AreEqual(expectedRadius, actualRadius);
    }

    [Test]
    public void GenerateNeighbours_FindNeighbours() {
        //Arrange
        neighbour1.transform.position = new Vector3(0, 10, 0);
        neighbour2.transform.position = new Vector3(10, 10, 10);
        ////Assign
        spawner.GenerateNeighbours(mainStar);
        ////Assert
        Assert.Greater(mainStar.starNeighbours.Count, 0);
    }

    [Test]
    public void GalaxyShape_Default() {
        //Arrange
        StarSpawner.GalaxyShape expectedShape = StarSpawner.GalaxyShape.Concave;
        //Assign
        StarSpawner.GalaxyShape actualShape = spawner.galaxyShape;
        //Assert
        Assert.AreEqual(expectedShape, actualShape);
    }
}
