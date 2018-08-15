using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour 
{
    Player player;

    void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    void OnMouseDown()
    {
        DestroyTower();
}

    void DestroyTower()
    {
        var grid = transform.GetComponentInParent<MyGrid>();
        grid.passable = true;
        grid.SetMaterial(StageController.Stage.Build);
        player.Tower += 1;
        Destroy(gameObject);
    }
}
