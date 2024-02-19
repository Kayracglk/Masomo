using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;
    public Dictionary<string, Stack<GameObject>> poolingObjects = new Dictionary<string, Stack<GameObject>>();

    [Serializable]
    public struct Pool
    {
        public GameObject particlePrefab;
        public int size;
    }
    public Pool[] pool;

    private void Awake()
    {
        instance = this;
        for (int i = 0; i < pool.Length; i++)
        {
            string key = pool[i].particlePrefab.name;
            int size = pool[i].size;
            for (int j = 0; j < size; j++)
            {
                GameObject obj = InstantiateObjects(key);
                if (!poolingObjects.ContainsKey(key))
                {
                    Stack<GameObject> stack = new Stack<GameObject>();
                    poolingObjects.Add(key, stack);
                }
                poolingObjects[key].Push(obj);
                obj.SetActive(false);
            }
        }
    }
    public GameObject InstantiateObjects(string name)
    {
        GameObject obj = Instantiate(Resources.Load("Pooling/" + name) as GameObject);
        obj.transform.parent = this.transform;
        return obj;
    }
    public GameObject OpenObject(string key, Vector3 pos)
    {
        GameObject particleObj = GetObject(key);
        particleObj.transform.position = pos;
        particleObj.SetActive(true);
        return particleObj;
    }

    public GameObject OpenObject(string key, Vector3 pos, Vector3 rotation)
    {
        GameObject particleObj = GetObject(key);
        particleObj.transform.position = pos;
        particleObj.transform.eulerAngles = rotation;
        particleObj.SetActive(true);
        return particleObj;
    }
    public GameObject OpenObject(string key, Vector3 pos, Transform parent)
    {
        GameObject particleObj = GetObject(key);
        particleObj.transform.position = pos;
        particleObj.transform.parent = parent;
        particleObj.SetActive(true);
        return particleObj;
    }
    public GameObject OpenObject(string key, Vector3 pos, float time)
    {
        GameObject particleObj = GetObject(key);
        particleObj.transform.position = pos;
        particleObj.SetActive(true);
        StartCoroutine(SetObject(particleObj, key, time));
        return particleObj;
    }
    private IEnumerator SetObject(GameObject obj, string key, float time)
    {
        if (!poolingObjects.ContainsKey(key)) yield break;
        yield return new WaitForSeconds(time);
        poolingObjects[key].Push(obj);
        obj.transform.parent = this.transform;
        obj.SetActive(false);
    }
    public GameObject GetObject(string key)
    {
        if (!poolingObjects.ContainsKey(key) || poolingObjects[key].Count <= 0) { Debug.LogError("Nesne Bulunamadi : " + key); return null; }
        if (poolingObjects[key].Count <= 2)
            for (int i = 0; i < 5; i++)
            {
                GameObject obj = InstantiateObjects(key);
                poolingObjects[key].Push(obj);
                obj.SetActive(false);
            }

        GameObject particleObj = poolingObjects[key].Pop();
        return particleObj;
    }
    public void SetObject(GameObject obj, string key)
    {
        if (!poolingObjects.ContainsKey(key)) return;
        poolingObjects[key].Push(obj);
        obj.transform.parent = this.transform;
        obj.SetActive(false);
    }
}

