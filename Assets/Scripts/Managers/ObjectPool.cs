using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool : MonoSingleton<ObjectPool>
{
    [Serializable]
    public struct Pool
    {
        public Queue<GameObject> pooledObjects;
        public GameObject objectPrefab;
        public int poolSize;
    }

    [SerializeField] private Pool[] pools = null;

    [SerializeField] Transform AllCubesTransform;

    private void Awake()
    {
        for (int j = 0; j < pools.Length; j++)
        {
            pools[j].pooledObjects = new Queue<GameObject>();

            for (int i = 0; i < pools[j].poolSize; i++)
            {
                GameObject obj = Instantiate(pools[j].objectPrefab, AllCubesTransform);
                obj.SetActive(false);

                pools[j].pooledObjects.Enqueue(obj);
            }
        }
    }

    public void ClearPooledObject()
    {
        for (int i = 0; i < pools.Length; i++)
        {
            for (int j = 0; j < pools[i].poolSize; j++)
            {
                GameObject obj = pools[i].pooledObjects.Dequeue();
                obj.SetActive(false);

                pools[i].pooledObjects.Enqueue(obj);
            }
        }
    }

    public GameObject GetPooledObject(int objectType)
    {
        if (objectType >= pools.Length)
        {
            return null;
        }

        GameObject obj = pools[objectType].pooledObjects.Dequeue();
        while (obj.activeSelf == true)
        {
            pools[objectType].pooledObjects.Enqueue(obj);
            obj = pools[objectType].pooledObjects.Dequeue();
        }


        pools[objectType].pooledObjects.Enqueue(obj);

        return obj;
    }
}
