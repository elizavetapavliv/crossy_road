using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainPool : MonoBehaviour
{
    [SerializeField]
    private List<TerrainData> terrainData = default;

    [SerializeField]
    private int terrainPoolSize = default;

    [SerializeField]
    private Transform terrainTransform = default;

    [SerializeField]
    private int startGrassCount = default;

    [SerializeField]
    private int countToSeePlayer = default;

    [SerializeField]
    private float distanceWithPlayer = default;

    private Vector3 currentPosition;

    private List<GameObject> terrain;

    private int currentIndex;

    private void Start()
    {
        currentPosition = Vector3.zero;
        terrain = new List<GameObject>(terrainPoolSize);

        var range = CreateTerrains(0, startGrassCount - countToSeePlayer, false);
        terrain.AddRange(range);
        range = CreateTerrains(0, countToSeePlayer, true);
        terrain.AddRange(range);
        while (terrain.Count < terrainPoolSize)
        {
            AddTerrains();
        }
        currentIndex = 0;
    }


    private void GenerateRandomTerrainParameters(out int terrainType, out int terrainCount)
    {
        terrainType = Random.Range(0, terrainData.Count);
        terrainCount = Random.Range(1, terrainData[terrainType].maxCount);
    }

    public void AddTerrains()
    {
        GenerateRandomTerrainParameters(out var terrainType, out var terrainCount);

        var range = CreateTerrains(terrainType, terrainCount, false);
        var maxCanAdd = terrainPoolSize - terrain.Count;

        if (range.Count > maxCanAdd)
        {
            range = range.Take(maxCanAdd).ToList();
        }
        terrain.AddRange(range);
    }

    public void GenerateTerrain(Vector3 playerPosition)
    {
        if (currentPosition.x - playerPosition.x < distanceWithPlayer)
        {
            terrain[currentIndex].transform.DOMove(currentPosition, 0f);
            terrain[currentIndex].transform.DOComplete();

            var coinGenerator = terrain[currentIndex].GetComponent<CoinGenerator>();
            if (coinGenerator)
            {
                coinGenerator.RegenerateCoins();
            }

            var movingObjectPool = terrain[currentIndex].GetComponent<MovingObjectPool>();
            if (movingObjectPool) 
            {
                movingObjectPool.StopMoving();
            }

            currentPosition.x++;
            currentIndex++;
            currentIndex %= terrainPoolSize;
        }
    }

    private List<GameObject> CreateTerrains(int terrainType, int terrainCount, bool canBePlayer)
    {
        var terrains = new List<GameObject>();
        for(int i = 0; i < terrainCount; i++)
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
           
            terrains.Add(Instantiate(terrainData[terrainType].terrainTypes[terrainDataType], 
                currentPosition, Quaternion.identity, terrainTransform));
            currentPosition.x++;
        }
        return terrains;
    }
}
