using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGrid : MonoBehaviour 
{
    StageController stageController;
    const int GridSize = 10;
    Vector2Int gridPos;
    MeshRenderer topMeshRenderer;

    [Header("Properties")]
    public bool passable = true;
    public bool isStartOrEnd;
    public bool isExplored;
    public MyGrid searchFrom;

    [Header("Materials")] 
    [SerializeField] Material Normal;
    [SerializeField] Material TowerPlace;
    [SerializeField] Material Forbidden;
    [SerializeField] bool DisableMeshInRunTime;


    private void Start()
    {
        stageController = FindObjectOfType<StageController>();
        topMeshRenderer = transform.Find("Top").GetComponent<MeshRenderer>();
        SetMaterial();
        if (!isStartOrEnd)
        topMeshRenderer.enabled = false;
    }

    private void OnMouseOver()
    {
        if (!isStartOrEnd)
        ShowGridImage(true);

    }

    private void OnMouseExit()
    {
        if (!isStartOrEnd)
        ShowGridImage(false);
    }

    private void OnMouseDown()
    {
        if(stageController.isBuildingMode)
        {
            if(!passable||isStartOrEnd)
            {
                print("Invalid grid.");
            }
            else
            {
                if (stageController.TowerCount > 0)
                {
                    PlaceTower();
                    return;
                }
                print("Out of tower,please delete one first(left click on tower)");
            }

        }
    }

    private void PlaceTower()
    {
        print("Placed a tower.");
        Instantiate(stageController.TowerPrefab, transform.position, Quaternion.identity,transform.Find("Top"));
        stageController.TowerCount -= 1;
        passable = false;
        SetMaterial();
    }

    private void ShowGridImage(bool b)
    {
        topMeshRenderer.enabled = b;
    }

    public void SetMaterial()
    {
        if (!isStartOrEnd)
        {
            if(stageController.isBuildingMode)
            {
                if(passable)
                {
                    topMeshRenderer.material = TowerPlace;
                }
                else
                {
                    topMeshRenderer.material = Forbidden;
                }
            }
            else
            {
                if(passable)
                {
                    topMeshRenderer.material = Normal;
                }
                else
                {
                    topMeshRenderer.material = Forbidden;
                }
            }
        }
    }

    public int GetGridSize()
    {
        return GridSize;
    }

    public Vector2Int GetGridPos()
    {
        return new Vector2Int
            (
                Mathf.RoundToInt(transform.position.x / GridSize) ,
                Mathf.RoundToInt(transform.position.z / GridSize) 
            );
    }
}
