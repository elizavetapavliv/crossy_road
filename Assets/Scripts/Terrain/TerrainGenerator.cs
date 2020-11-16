using DG.Tweening;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{ 
    [SerializeField]
    private float distanceWithPlayer = default;

    private Vector3 currentPosition;

    private int currentIndex;

    private int terrainPoolSize;

    private void Start()
    {
        terrainPoolSize = TerrainPool.instance.terrainPoolSize;
        currentPosition = TerrainPool.instance.currentPosition;
        currentIndex = 0;
    }

    public void GenerateTerrain(Vector3 playerPosition)
    {
        if (currentPosition.x - playerPosition.x < distanceWithPlayer)
        {
            var terrain = TerrainPool.instance.GetTerrain(currentIndex);
            terrain.transform.DOMove(currentPosition, 0f);
            terrain.transform.DOComplete();

            var coinGenerator = terrain.GetComponent<CoinGenerator>();
            if (coinGenerator)
            {
                coinGenerator.RegenerateCoins();
            }

            var movingObjectGenerator = terrain.GetComponent<MovingObjectGenerator>();
            if (movingObjectGenerator)
            {
                movingObjectGenerator.StopMoving();
            }

            currentPosition.x++;
            currentIndex++;
            currentIndex %= terrainPoolSize;
        }
    }
}
