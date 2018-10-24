using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridObjects
{
    /// <summary>
    /// Uses perlin noise grid created in the GenerateObjects class to instantiate objects to the game scene in start time
    /// Different objects are instantiated dependant on the noise value at the current position in the grid
    /// Positions of objects will be randomised and different with each playthrough
    /// Objects are added to their resepctive lists to keep track on current numbers within the scene
    /// </summary>

    private Transform treeParentTrans, brambleParentTrans, groundParent;
    private GameObject thisObject, spawnedObject;

    private string objectType;
    private float noiseValue;
    private int gridMaxX, gridMaxZ, gridPosX, gridPosZ;


    public GridObjects(Transform treeParent, Transform brambleParent, GameObject gameObject, int maxX, int maxZ, int x, int z)
    {
        groundParent = GameObject.Find("Field").transform;
        treeParentTrans = treeParent;
        brambleParentTrans = brambleParent;
        thisObject = gameObject;
        gridMaxX = maxX;
        gridMaxZ = maxZ;
        gridPosX = x;
        gridPosZ = z;
        objectType = "Empty";
    }


    private void SpawnObject()
    {
        if (gridPosX == 0 || gridPosX == gridMaxX - 1)
        {
            if (gridPosZ % 3 == 0)
            {
                spawnedObject = MonoBehaviour.Instantiate(Resources.Load("Flora/FirTree_02"),
                    thisObject.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity, treeParentTrans) as GameObject;
                spawnedObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                spawnedObject.GetComponent<Renderer>().material.color = new Color(noiseValue, noiseValue, noiseValue);
                objectType = "Tree";
            }
        }
        else if (gridPosZ == 0 || gridPosZ == gridMaxZ - 1)
        {
            if (gridPosX % 3 == 0)
            {
                spawnedObject = MonoBehaviour.Instantiate(Resources.Load("Flora/FirTree_02"),
                    thisObject.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity, treeParentTrans) as GameObject;
                spawnedObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                spawnedObject.GetComponent<Renderer>().material.color = new Color(noiseValue, noiseValue, noiseValue);
                objectType = "Tree";
            }
        }
        else if (noiseValue >= 0 && noiseValue < 0.05f)
        {
            if (gridPosX > 1 && gridPosX < gridMaxX - 2 && gridPosZ > 1 && gridPosZ < gridMaxZ - 2)
            {
                spawnedObject = MonoBehaviour.Instantiate(Resources.Load("Flora/Spikes"),
                    thisObject.transform.position, Quaternion.Euler(0, Random.Range(0.0f, 180.0f), 0), thisObject.transform) as GameObject;
                objectType = "Spikes";
            }
        }
        else if (noiseValue >= 0.05f && noiseValue < 0.18f)
        {
            if (gridPosX > 1 && gridPosX < gridMaxX - 2 && gridPosZ > 1 && gridPosZ < gridMaxZ - 2)
            {
                spawnedObject = MonoBehaviour.Instantiate(Resources.Load("Flora/Shrub_18"),
                    thisObject.transform.position + new Vector3(0, 0.4f, 0), Random.rotation, brambleParentTrans) as GameObject;
                spawnedObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                spawnedObject.GetComponent<BrambleGrowth>().SetCoords(gridPosX, gridPosZ);
                objectType = "Brambles";
            }
        }
        else if (noiseValue >= 0.25f && noiseValue < 0.29f)
        {
            if (gridPosX > 1 && gridPosX < gridMaxX - 2 && gridPosZ > 1 && gridPosZ < gridMaxZ - 2)
            {
                spawnedObject = MonoBehaviour.Instantiate(Resources.Load("Flora/Grass3D"),
                    thisObject.transform.position, Quaternion.Euler(0, Random.Range(0.0f, 180.0f), 0), thisObject.transform) as GameObject;
                spawnedObject.GetComponent<Renderer>().material.color = new Color(noiseValue, noiseValue, noiseValue);
                spawnedObject.transform.localScale = new Vector3(0.2f, 0.1f, 0.2f);
                objectType = "Grass";
            }
        }
    }


    public void FillRemaining()
    {
        if (objectType == "Empty" || objectType == "Sheep") 
        {
            if (gridPosX > 1 && gridPosX < gridMaxX - 2 && gridPosZ > 1 && gridPosZ < gridMaxZ - 2)
            {
                spawnedObject = MonoBehaviour.Instantiate(Resources.Load("Flora/Shrub_18"),
                    thisObject.transform.position + new Vector3(0, 0.4f, 0), Random.rotation, brambleParentTrans) as GameObject;
                spawnedObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                spawnedObject.GetComponent<BrambleGrowth>().SetCoords(gridPosX, gridPosZ);
                spawnedObject.SetActive(false);
                objectType = "Brambles";
            }
        }
    }


    public float NoiseValue { get { return noiseValue; } set { noiseValue = value; SpawnObject(); } }

    public string ObjectType { get { return objectType; } set { objectType = value; } }

    public GameObject ThisObject { get { return thisObject; } }

    public GameObject SpawnedObject { get { return spawnedObject; } }
}
