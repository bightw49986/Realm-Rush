using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour 
{
    Player player;
    StageController stageController;

    void Awake()
    {
        player = FindObjectOfType<Player>();
        stageController = FindObjectOfType<StageController>();
    }

    void OnMouseDown()
    {
        if (stageController.CurrentStage == StageController.Stage.Build) 
        DestroyTower();
        // todo show tower info.
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
