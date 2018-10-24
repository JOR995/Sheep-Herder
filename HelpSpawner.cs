using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpSpawner : MonoBehaviour
{
    /// <summary>
    /// Controls the spawning and despawning of helper characters; sheep dogs (ducks) and farmers (chickens)
    /// Instantiates the required objects at start time
    /// Spawns the objects within set intervals in the set location through use of pooling
    /// Tracks for when characters are to be despawned through a timer which is started on use
    /// </summary>

    public static HelpSpawner helpSpawnerInstance;

    private UIContoller uiController;
    private List<GameObject> dogList, farmerList, spawnPositions;
    private Transform dogPool, farmerPool;

    private int dogMax, farmerMax;
    private bool ableToSpawn;

    void OnEnable()
    {
        helpSpawnerInstance = this;

        uiController = UIContoller.uIContollerInstance;
        dogPool = GameObject.FindGameObjectWithTag("DogPool").transform;
        farmerPool = GameObject.FindGameObjectWithTag("FarmerPool").transform;

        dogList = new List<GameObject>();
        farmerList = new List<GameObject>();
        spawnPositions = new List<GameObject>()
        {
            transform.GetChild(0).gameObject,
            transform.GetChild(1).gameObject
        };

        dogMax = 2;
        farmerMax = 2;
        ableToSpawn = true;

        InstantiateObjects();
        StartCoroutine(SpawnTimer());
    }


    private void InstantiateObjects()
    {
        GameObject spawnedObject;

        for (int i = 0; i < dogMax; i++)
        {
            spawnedObject = Instantiate(Resources.Load("Fauna/DogTemp"), dogPool.position, Quaternion.identity, dogPool) as GameObject;
            spawnedObject.SetActive(false);
        }

        for (int i = 0; i < farmerMax; i++)
        {
            spawnedObject = Instantiate(Resources.Load("Fauna/FarmerTemp"), farmerPool.position, Quaternion.identity, farmerPool) as GameObject;
            spawnedObject.SetActive(false);
        }

        foreach (Transform dog in dogPool)
        {
            dogList.Add(dog.gameObject);
        }

        foreach (Transform farmer in farmerPool)
        {
            farmerList.Add(farmer.gameObject);
        }
    }


    private int SpawnObject(string type)
    {
        string objectType = type;
        int numInnactive = 0;

        switch (objectType)
        {
            case "Dog":
                foreach (GameObject dog in dogList)
                {
                    if (!dog.activeSelf)
                    {
                        Vector3 spawnPosition = transform.position + Random.insideUnitSphere * 3;
                        dog.transform.position = new Vector3(spawnPosition.x, 0, spawnPosition.z);
                        dog.SetActive(true);
                        uiController.HelpSpawned(dog);
                        break;
                    }
                }
                break;
            case "Farmer":
                foreach (GameObject farmer in farmerList)
                {
                    if (!farmer.activeSelf)
                    {
                        Vector3 spawnPosition = transform.position + Random.insideUnitSphere * 3;
                        farmer.transform.position = new Vector3(spawnPosition.x, 0, spawnPosition.z);
                        farmer.SetActive(true);
                        uiController.HelpSpawned(farmer);
                        break;
                    }
                }
                break;
            default:
                break;
        }

        foreach (GameObject dog in dogList)
        {
            if (!dog.activeSelf) numInnactive++;
        }
        foreach (GameObject farmer in farmerList)
        {
            if (!farmer.activeSelf) numInnactive++;
        }

        return numInnactive;
    }


    private IEnumerator SpawnTimer()
    {
        int numInnactive;
        string objectType;
        do
        {
            yield return new WaitForSeconds(Random.Range(20.0f, 25.0f));

            if (Random.Range(0, 1.0f) <= 0.5f)
            {
                objectType = "Dog";
            }
            else
            {
                objectType = "Farmer";
            }

            numInnactive = SpawnObject(objectType);
        }
        while (numInnactive > 0);
        ableToSpawn = false;
    }


    public void ObjectDespawned()
    {
        if (!ableToSpawn) StartCoroutine(SpawnTimer());
    }
        
}
