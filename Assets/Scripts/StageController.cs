using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class StageController : MonoBehaviour 
{
    public bool isBuildingMode;
    public GameObject TowerPrefab;
    public int TowerCount = 2;
    PathCreator pathCreator;
    EnemySpawner enemySpawner;


	void Start () 
    {
        pathCreator = FindObjectOfType<PathCreator>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        pathCreator.ClearPath();
        isBuildingMode = true;
        BroadcastMessage("SetMaterial");
	}

    public void TransToPlayingMode()
    {
        isBuildingMode = false;
        BroadcastMessage("SetMaterial");
        pathCreator.GeneratePath();
    }
    public void TransToBuildingMode()
    {
        pathCreator.ClearPath();
        isBuildingMode = true;
        BroadcastMessage("SetMaterial");
    }
}
