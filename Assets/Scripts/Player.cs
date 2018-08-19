using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StageController))]
public class Player : MonoBehaviour
{
    public GameObject TowerPrefab;
    public int HP { get { return m_hp; } set { m_hp = value;OnHPChanged(m_hp); } }
    int m_hp;
    public int Tower { get;set; }
    public int EnemyKilled { get; set; }
    public bool isAlive;
    public bool hasInit;

    void Awake()
    {
        if (FindObjectsOfType<Player>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public event Action<int> HPChanged;
    protected virtual void OnHPChanged(int hp)
    {
        if(HPChanged!=null)
        {
            HPChanged(hp);
        }
        if (hp <=0 && isAlive == true)
        {
            OnPlayerDied();
        }
    }

    public event Action Died;
    protected virtual void OnPlayerDied()
    {
        if(Died != null)
        {
            Died();
        }
        isAlive = false;
    }

    public List<int> GetPlayerStates()
    {
        List<int> pList = new List<int>{HP,Tower,EnemyKilled};
        return pList;
    }

    public void SetPlayerStates(List<int> pList)
    {
        if (pList==null || pList.Count!=3)
        {
            return;
        }
        HP = pList[0];
        Tower = pList[1];
        EnemyKilled = pList[2];

    }
}
