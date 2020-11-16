using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectGenerator : MonoBehaviour
{
    [SerializeField]
    private float minWaitingTime = default;

    [SerializeField]
    private float maxWaitingTime = default;

    [SerializeField]
    private Transform positionOnTerrain = default;

    [SerializeField]
    private float offset = default;

    private List<MovingObject> movingObjects;
    private void Start()
    {
        movingObjects = new List<MovingObject>();
        StartCoroutine("ActivateMovingObject");
    }

    private IEnumerator ActivateMovingObject()
    {
        while (true)
        {
            var timeToWait = Random.Range(minWaitingTime, maxWaitingTime);
            yield return new WaitForSeconds(timeToWait);

            var poolType = gameObject.name.Contains("Water") ? "log" : "transport";

            var movingObject = MovingObjectPool.instances[poolType].GetPooledObject();
            if (movingObject != null)
            {
                movingObjects.Add(movingObject);
                movingObject.gameObject.SetActive(true);
                movingObject.offset = offset;
                movingObject.transform.position = positionOnTerrain.position;
                movingObject.transform.eulerAngles = new Vector3(0, positionOnTerrain.localEulerAngles.y, 0);
                movingObject.direction = positionOnTerrain.localEulerAngles.y == 90 ? -1 : 1;
                movingObject.transform.DOMoveX(transform.position.x, 0);
                movingObject.transform.DOComplete();
                movingObject.StartMoving();
            }
        }
    }
    public void StopMoving()
    {
        foreach (var movingObject in movingObjects)
        {
            movingObject.StopMoving();
        }
        movingObjects.Clear();
    }
}
