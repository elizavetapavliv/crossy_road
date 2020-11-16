using System.Collections.Generic;
using UnityEngine;

public class MovingObjectPool : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> movingObjectsPrefabs = default;

    [SerializeField]
    private int poolSize = default;

    [SerializeField]
    private string type = default;

    private List<MovingObject> pool;

    public static Dictionary<string, MovingObjectPool> instances 
        = new Dictionary<string, MovingObjectPool>();

    private void Start()
    {
        instances[type] = this;
        pool = new List<MovingObject>(poolSize);
        
        for (int i = 0; i < poolSize; i++)
        {
            pool.Add(CreateMovingObject());
        }
    }
    
    public MovingObject GetPooledObject()
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (!pool[i].gameObject.activeInHierarchy)
            {
                return pool[i];
            }
        }
        return null;
    }

    private MovingObject CreateMovingObject()
    {
        int objectType = 0;
        if (movingObjectsPrefabs.Count > 1)
        {
            objectType = Random.Range(0, movingObjectsPrefabs.Count);
        }
        var newObject = Instantiate(movingObjectsPrefabs[objectType]).GetComponent<MovingObject>();
        newObject.gameObject.SetActive(false);
        return newObject;
    }
}