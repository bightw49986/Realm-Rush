using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGrid : MonoBehaviour 
{
    Player player;
    StageController stageController;

    [Header("Properties")]
    const int GridSize = 10;
    Vector2Int gridPos;
    MeshRenderer topMeshRenderer;
    public bool passable = true;
    public bool isStartOrEnd;
    public bool isExplored;
    public MyGrid searchFrom;

    [Header("Materials")] 
    [SerializeField] Material Normal;
    [SerializeField] Material TowerPlace;
    [SerializeField] Material Forbidden;



    void Awake()
    {
        stageController = FindObjectOfType<StageController>();
        player = FindObjectOfType<Player>();
        topMeshRenderer = transform.Find("Top").GetComponent<MeshRenderer>();
    }

    void Start()
    {
        
        if(!isStartOrEnd)
        {
            SetMaterial(stageController.CurrentStage);
            stageController.StageChanged += SetMaterial;
            topMeshRenderer.enabled = false;
        }
    }

    void OnMouseOver()
    {
        if (!isStartOrEnd)
            topMeshRenderer.enabled = true;

    }

    void OnMouseExit()
    {
        if (!isStartOrEnd)
            topMeshRenderer.enabled = false;
    }

    void OnMouseDown()
    {
        if (stageController.CurrentStage == StageController.Stage.Build)
        {
            if (!passable||isStartOrEnd)
            {
                print("Invalid grid.");
            }
            else
            {
                if (player.Tower > 0)
                {
                    PlaceTower();
                    return;
                }
                print("Out of tower,please delete one first(left click on tower)");
            }
        }
    }

    void PlaceTower()
    {
        print("Placed a tower.");
        Instantiate(player.TowerPrefab, transform.position, Quaternion.identity, transform.Find("Top"));
        player.Tower -= 1;
        passable = false;
        SetMaterial(stageController.CurrentStage);
    }

    public void SetMaterial(StageController.Stage stage)
    {
        switch(stage)
        {
            case (StageController.Stage.Build):
                {
                    if (passable&& !isStartOrEnd )
                    {
                        topMeshRenderer.material = TowerPlace;
                        return;
                    }
                        topMeshRenderer.material = Forbidden;
                        return;
                }
            default:
                {
                    if(passable && !isStartOrEnd)
                    {
                        topMeshRenderer.material = Normal;
                        return;
                    }
                    topMeshRenderer.material = Forbidden;
                    return;
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
