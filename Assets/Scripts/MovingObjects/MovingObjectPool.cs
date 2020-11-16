using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectPool : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> movingObjectsPrefabs = default;

    [SerializeField]
    private Transform positionOnTerrain = default;

    [SerializeField]
    private float minWaitingTime = default;

    [SerializeField]
    private float maxWaitingTime = default;

    [SerializeField]
    private int poolSize = default;

    private List<MovingObject> pool;

    private List<MovingObject> movingObjects;

    private void Start()
    {
        pool = new List<MovingObject>(poolSize);
        movingObjects = new List<MovingObject>();
        for (int i = 0; i < poolSize; i++)
        {
            pool.Add(CreateMovingObject());
        }
        StartCoroutine("ActivateMovingObject");
    }
    private IEnumerator ActivateMovingObject()
    {
        while (true)
        {
            var timeToWait = Random.Range(minWaitingTime, maxWaitingTime);
            yield return new WaitForSeconds(timeToWait);

            var movingObject = GetPooledObject();
            if(movingObject != null) 
            {
                movingObjects.Add(movingObject);
                movingObject.gameObject.SetActive(true);
                if (movingObject.isUsed)
                {
                    var newPosition = new Vector3(transform.position.x,
                        movingObject.transform.position.y, movingObject.initialPosition.z);
                    movingObject.transform.DOMove(newPosition, 0);
                    movingObject.initialPosition = newPosition;
                    movingObject.transform.DOComplete();
                    movingObject.StartMoving();
                }
            }
        } 
    }

    private MovingObject GetPooledObject()
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
        var newObject = Instantiate(movingObjectsPrefabs[objectType], positionOnTerrain.position,
           Quaternion.Euler(0, positionOnTerrain.localEulerAngles.y, 0)).GetComponent<MovingObject>();
        newObject.direction = positionOnTerrain.localEulerAngles.y == 90 ? -1 : 1;
        newObject.gameObject.SetActive(false);
        return newObject;
    }

    public void StopMoving()
    {
        foreach (var movingObject in movingObjects)
        {
            movingObject.StopMoving();
            movingObject.transform.DOComplete();
        }
        movingObjects.Clear();
    }
}