using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainPool : MonoBehaviour
{
    [SerializeField]
    private Transform terrainTransform = default;

    [SerializeField]
    private List<TerrainData> terrainData = default;

    private List<GameObject> pool;

    public Vector3 currentPosition;

    public int terrainPoolSize = default;

    public int startGrassCount = default;

    public int countToSeePlayer = default;

    public static TerrainPool instance;

    private void Start()
    {
        instance = this;
        pool = new List<GameObject>(terrainPoolSize);
        currentPosition = Vector3.zero;

        var range = CreateTerrains(0, startGrassCount - countToSeePlayer, false);
        pool.AddRange(range);
        range = CreateTerrains(0, countToSeePlayer, true);
        pool.AddRange(range);
        while (pool.Count < terrainPoolSize)
        {
            AddTerrains();
        }
        currentPosition.x = terrainPoolSize;
    }


    private void GenerateRandomTerrainParameters(out int terrainType, out int terrainCount)
    {
        terrainType = Random.Range(0, terrainData.Count);
        terrainCount = Random.Range(1, terrainData[terrainType].maxCount);
    }

    public void AddTerrains()
    {
        GenerateRandomTerrainParameters(out var terrainType, out var terrainCount);
        terrainCount = Mathf.Min(terrainCount, terrainPoolSize - pool.Count);

        var range = CreateTerrains(terrainType, terrainCount, false);
        pool.AddRange(range);
    }

    private List<GameObject> CreateTerrains(int terrainType, int terrainCount, bool canBePlayer)
    {
        var terrains = new List<GameObject>();
        for (int i = 0; i < terrainCount; i++)
        {
            int terrainDataType;
            if (canBePlayer)
            {
                terrainDataType = Random.Range(0, terrainData[terrainType].terrainTypes.Count - 1);
            }
            else
            {
                terrainDataType = Random.Range(0, terrainData[terrainType].terrainTypes.Count);
            }
            var newTerrain = Instantiate(terrainData[terrainType].terrainTypes[terrainDataType], 
                currentPosition, Quaternion.identity, terrainTransform);
            terrains.Add(newTerrain);
            currentPosition.x++;
        }
        return terrains;
    }

    public GameObject GetTerrain(int index)
    {
        return pool[index];
    }
}
