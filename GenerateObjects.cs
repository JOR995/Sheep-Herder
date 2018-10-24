using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GenerateObjects : MonoBehaviour
{
    /// <summary>
    /// Class handles creation of objects upon starting the game
    /// Creates instances of other classes to be used
    /// Generates an array of randomised float values using perlin noise
    /// Bakes the navmesh into the scene for AI use
    /// </summary>

    public static GenerateObjects generateObjectsInstance;

    [SerializeField]
    GameObject gridPlaceholder;

    [SerializeField]
    Transform groundParent, treeParent, bramblesParent;

    List<Transform> bramblesList;

    private SheepSpawner sheepSpawner;
    private GridObjects[,] noiseGrid;
    private GameObject newObject, ground, spawnZone;

    private float noiseScale;


    void Awake()
    {
        generateObjectsInstance = this;
        sheepSpawner = new SheepSpawner();
    }


    void Start()
    {
        noiseGrid = new GridObjects[15, 15];
        bramblesList = new List<Transform>();

        noiseScale = 2.5f;

        for (int x = 0; x < noiseGrid.GetLength(0); x++)
        {
            for (int z = 0; z < noiseGrid.GetLength(1); z++)
            {
                newObject = (Instantiate(gridPlaceholder, new Vector3((x), 0, (z)), Quaternion.identity) as GameObject);
                newObject.transform.parent = transform;
                noiseGrid[x, z] = new GridObjects(treeParent, bramblesParent, newObject, noiseGrid.GetLength(0), noiseGrid.GetLength(1), x, z);
            }
        }

        GenerateNoise();
        GenerateNavMesh();

        sheepSpawner.SpawnSheep(noiseGrid);

        for (int x = 0; x < noiseGrid.GetLength(0); x++)
        {
            for (int z = 0; z < noiseGrid.GetLength(1); z++)
            {
                noiseGrid[x, z].FillRemaining();
            }
        }

        foreach (Transform bramble in bramblesParent)
        {
            bramblesList.Add(bramble);
        }

        CheckNumActiveBrambles();
    }


    void GenerateNoise()
    {
        float offset = Random.Range(0, 50);

        for (int x = 0; x < noiseGrid.GetLength(0); x++)
        {
            for (int z = 0; z < noiseGrid.GetLength(1); z++)
            {
                float noise = Mathf.PerlinNoise(((noiseGrid[x, z].ThisObject.transform.position.x / noiseScale) + offset), ((noiseGrid[x, z].ThisObject.transform.position.z / noiseScale) + offset));

                noiseGrid[x, z].NoiseValue = noise;
            }
        }
    }


    void GenerateNavMesh()
    {
        float posX, posZ, scaleX, scaleZ;

        posX = (noiseGrid[noiseGrid.GetLength(0) - 1, 0].ThisObject.transform.position.x - noiseGrid[0, 0].ThisObject.transform.position.x) / 2;
        posZ = (noiseGrid[0, noiseGrid.GetLength(0) - 1].ThisObject.transform.position.z - noiseGrid[0, 0].ThisObject.transform.position.z) / 2;
        scaleX = 0.1f * noiseGrid.GetLength(0);
        scaleZ = 0.1f * noiseGrid.GetLength(1);

        ground = Instantiate(Resources.Load("Flora/GroundTile"), new Vector3(posX, 0, posZ), Quaternion.identity, groundParent) as GameObject;
        ground.gameObject.transform.localScale = new Vector3(scaleX, 1, scaleZ);

        NavMeshSurface[] navMeshes = ground.GetComponents<NavMeshSurface>();
        navMeshes[0].BuildNavMesh();
        navMeshes[1].BuildNavMesh();
        navMeshes[2].BuildNavMesh();

        spawnZone = Instantiate(Resources.Load("Flora/HelpSpawn"), new Vector3((posX - noiseGrid.GetLength(1)) + 3.5f, 0, posZ), Quaternion.Euler(0, 90, 0)) as GameObject;
    }


    public void CheckNumActiveBrambles()
    {
        int numActiveBrambles = 0;

        foreach (Transform bramble in bramblesList)
        {
            if (bramble.gameObject.activeSelf) numActiveBrambles++;
        }

        if (numActiveBrambles == 0)
        {
            StartCoroutine(SetActiveBrambles());
        }
    }
        

    private IEnumerator SetActiveBrambles()
    {
        int numEnabled = 0;

        do
        {
            yield return new WaitForSeconds(2.0f);
            bramblesList[Random.Range(0, bramblesList.Count)].gameObject.SetActive(true);
            numEnabled++;
        }
        while (numEnabled < 2);
    }


    public GridObjects[,] NoiseGrid { get { return noiseGrid; } }
}
