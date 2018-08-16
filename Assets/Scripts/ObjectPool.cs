using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPool : MonoBehaviour
{
    Dictionary<PoolingGameObjectData.PoolKey, List<PoolingGameObjectData>> m_pool;
    Vector3 InstanitiatePos;
    Transform InstantiateParent;

    StageController stageController;

    void Awake()
    {
        Init();
        stageController = FindObjectOfType<StageController>();
    }

    void Start()
    {
        OnSceneloaded(stageController.currentScene);
        stageController.SceneLoaded += OnSceneloaded;
        stageController.SceneUnLoaded += OnSceneUnloaded;
    }

    void OnSceneloaded(Scene scene)
    {
        InstanitiatePos = FindObjectOfType<EnemySpawner>().transform.position;
        InstantiateParent = GameObject.Find("SpawnAtRunTime").transform;
    }

    void OnSceneUnloaded(Scene scene)
    {
        ClearPool();
    }

    void Init()
    {
        if (FindObjectsOfType<ObjectPool>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
        m_pool = new Dictionary<PoolingGameObjectData.PoolKey, List<PoolingGameObjectData>>();
    }

    public void InstantiateGameObjectData(int iCount, PoolingGameObjectData.PoolKey key, Object o)
    {
        List<PoolingGameObjectData> pList;

        if (o == null) return;

        if (!m_pool.ContainsKey(key))
        {
            pList = new List<PoolingGameObjectData>();
            m_pool.Add(key, pList);
        }
        else
        {
            pList = m_pool[key];
        }


        for (int i = 0; i < iCount; i++)
        {
            PoolingGameObjectData goData = new PoolingGameObjectData();
            GameObject go = Instantiate(o,InstanitiatePos,Quaternion.identity,InstantiateParent) as GameObject;
            go.SetActive(false);
            goData.m_gameObject = go;
            goData.m_isUsing = false;
            pList.Add(goData);
        }
    }


    /// <summary>
    /// Activate a gameobject from pool. If you're not sure if you'll over activate than the pool's amounts, use the second overload.
    /// </summary>
    /// <returns>The gameobject from pool.</returns>
    /// <param name="key"> use to search in the pool</param>
    /// 
    public GameObject AccessGameObjectFromPool(PoolingGameObjectData.PoolKey key)
    {
        List<PoolingGameObjectData> pList;
        if (!m_pool.ContainsKey(key))
        {
            Debug.Log("Object not exist, Should have instantiate first.");
            return null;
        }
        else
        {
            pList = m_pool[key];
        }

        GameObject go = null;

        for (int i = 0; i < pList.Count; i++)
        {
            if (pList[i].m_isUsing == false)
            {
                pList[i].m_isUsing = true;
                go = pList[i].m_gameObject;
                go.SetActive(true);
                break;
            }
        }

        return go;
    }

    /// <summary>
    /// Activate the gameobject from pool.
    /// </summary>
    /// <returns>The game object from pool.</returns>
    /// <param name="key">Target's key in object pool.</param>
    /// <param name="o">Target gameobject.</param>
    /// <param name="InstantiateOptionIfUnUsingNotFound">If set to <c>true</c> , autometiclly instantiate 1 more GOData to the pool if there's no GO availible.</param>
    public GameObject AccessGameObjectFromPool(PoolingGameObjectData.PoolKey key, GameObject o, bool InstantiateOptionIfUnUsingNotFound = false)
    {
        List<PoolingGameObjectData> pList;
        if (!m_pool.ContainsKey(key))
        {
            Debug.Log("Object not exist, Should have instantiate first.");
            return null;
        }
        else
        {
            pList = m_pool[key];
        }

        GameObject go = null;
        while (go == null)
        {
            for (int i = 0; i < pList.Count; i++)
            {
                if (pList[i].m_isUsing == false)
                {
                    pList[i].m_isUsing = true;
                    go = pList[i].m_gameObject;
                    go.SetActive(true);
                    break;
                }
            }

            if (go == null && InstantiateOptionIfUnUsingNotFound == true)
            {
                InstantiateGameObjectData(1, key, o);
            }
            else break;
        }

        return go;
    }

    public void ReturnGameObjectToPool(GameObject go, PoolingGameObjectData.PoolKey key)
    {
        List<PoolingGameObjectData> pList;
        if (!m_pool.ContainsKey(key))
        {
            Debug.Log("Error: This type of object is not includded in the pool.");
            return;
        }
        else
        {
            pList = m_pool[key];
        }

        for (int i = 0; i < pList.Count; i++)
        {
            if (pList[i].m_isUsing == true && pList[i].m_gameObject == go)
            {
                go.SetActive(false);
                pList[i].m_isUsing = false;
                break;
            }
        }
    }

    void ClearPool()
    {
        m_pool.Clear();
        m_pool = null;
        Resources.UnloadUnusedAssets();
    }

}

