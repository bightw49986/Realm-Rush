using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour 
{
    StageController stageController;
	// Use this for initialization
	void Start () 
    {
        stageController = FindObjectOfType<StageController>();	
	}
    private void OnMouseDown()
    {
        DestroyTower();
}

    private void DestroyTower()
    {
        var grid = transform.GetComponentInParent<MyGrid>();
        grid.passable = true;
        grid.SetMaterial();
        stageController.TowerCount += 1;
        Destroy(gameObject);
    }
}
