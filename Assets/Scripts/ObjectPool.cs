using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPool : MonoBehaviour
{
    Dictionary<PoolingGameObjectData.m_eType, List<PoolingGameObjectData>> m_pool;

    StageController stageController;

    void Awake()
    {
        Init();
        stageController = FindObjectOfType<StageController>();
    }

    void Start()
    {
        stageController.SceneUnLoaded += OnSceneUnloaded;
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
        m_pool = new Dictionary<PoolingGameObjectData.m_eType, List<PoolingGameObjectData>>();
    }

    public void InstantiateGameObjectData(int iCount, PoolingGameObjectData.m_eType eType, Object o)
    {
        List<PoolingGameObjectData> pList;

        if (o == null) return;

        if (!m_pool.ContainsKey(eType))
        {
            pList = new List<PoolingGameObjectData>();
            m_pool.Add(eType, pList);
        }
        else
        {
            pList = m_pool[eType];
        }


        for (int i = 0; i < iCount; i++)
        {
            PoolingGameObjectData goData = new PoolingGameObjectData();
            GameObject go = Instantiate(o) as GameObject;
            go.SetActive(false);
            goData.m_gameObject = go;
            goData.m_isUsing = false;
            pList.Add(goData);
        }
    }

    public GameObject AccessGameObjectFromPool(PoolingGameObjectData.m_eType eType, bool InstantiateOptionIfUnUsingNotFound = false)
    {
        List<PoolingGameObjectData> pList;
        if (!m_pool.ContainsKey(eType))
        {
            Debug.Log("Object not exist, Should have instantiate first.");
            return null;
        }
        else
        {
            pList = m_pool[eType];
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
                InstantiateGameObjectData(1, eType, pList[0].m_gameObject);
            }
            else break;
        }

        return go;

    }

    public void ReturnGameObjectToPool(GameObject go, PoolingGameObjectData.m_eType eType)
    {
        List<PoolingGameObjectData> pList;
        if (!m_pool.ContainsKey(eType))
        {
            Debug.Log("Error: This type of object is not includded in the pool.");
            return;
        }
        else
        {
            pList = m_pool[eType];
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

