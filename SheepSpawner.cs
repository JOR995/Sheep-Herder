using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepSpawner
{
    /// <summary>
    /// Creates sheep objects within the field space through instantiation during start time
    /// Sheep positions are assigned using the grid of perlin noise
    /// </summary>

    public static SheepSpawner sheepSpawnerInstance;

    private GameObject sheepParent;
    private GridObjects[,] gridSpaces;

    private int sheepMax, sheepCount, gridMaxX, gridMaxZ;
    private int rndX, rndZ;


    public SheepSpawner()
    {
        sheepSpawnerInstance = this;
        sheepParent = GameObject.FindGameObjectWithTag("SheepPool");
        sheepMax = Random.Range(4, 6);
        sheepCount = 0;
    }


    public void SpawnSheep(GridObjects[,] gridArray)
    {
        gridSpaces = gridArray;
        gridMaxX = gridSpaces.GetLength(0);
        gridMaxZ = gridSpaces.GetLength(1);

        do
        {
            rndX = Random.Range(2, gridMaxX - 2);
            rndZ = Random.Range(2, gridMaxZ - 2);

            if (gridSpaces[rndX, rndZ].ObjectType == "Empty")
            {
                GameObject spawnedSheep = MonoBehaviour.Instantiate(Resources.Load("Fauna/SheepWhite"),
                    gridSpaces[rndX, rndZ].ThisObject.transform.position, Quaternion.identity, sheepParent.transform) as GameObject;
                spawnedSheep.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                gridSpaces[rndX, rndZ].ObjectType = "Sheep";
                sheepCount++;
            }
        }
        while (sheepCount < sheepMax);
    }


    public int SheepMax { get { return sheepMax; } }
}
