using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PathCreator : MonoBehaviour 
{
    StageController stageController;

    [Header("PathFinding")]
    public MyGrid StartPoint,EndPoint;
    Dictionary<Vector2, MyGrid> BFSWayPoints = new Dictionary<Vector2, MyGrid>();
    Queue<MyGrid> queue = new Queue<MyGrid>(); 
    Vector2Int[] directions = 
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };
    bool isRunning = true;
    MyGrid searchCenter;
    List<MyGrid> Path = new List<MyGrid>();

    void Awake()
    {
        stageController = FindObjectOfType<StageController>();
    }

    void Start()
    {
        ClearPath();
        stageController.StageChanged += OnStageChanged;
    }

    void OnStageChanged(StageController.Stage stage)
    {
        if (Path.Count==0 && stage == StageController.Stage.Defense)
        {
            GeneratePath();

        }
            
    }


    public List<MyGrid> GetPath()
    {
        return Path;
    }

    void ClearPath()
    {
        if(Path!=null)
        Path.Clear();
    }

    public void GeneratePath()
    {
        LoadBlocks();
        BreadthFirstSearch();
        CreatePath();
        OnPathGenerated();
    }

    public event Action PathGenerated;
    protected virtual void OnPathGenerated()
    {
        if (PathGenerated != null)
        {
            PathGenerated();
            print("Path generated.");
        }
    }


    void CreatePath()
    {
        Path.Add(EndPoint);
        MyGrid previous = EndPoint.searchFrom;
        while (previous != StartPoint)
        {
            Path.Add(previous);
            previous = previous.searchFrom;
        }
        Path.Add(StartPoint);
        Path.Reverse();
    }

    void BreadthFirstSearch()
    {
        queue.Enqueue(StartPoint);
        while (queue.Count > 0 && isRunning)
        {
            searchCenter = queue.Dequeue();
            searchCenter.isExplored = true;
            HaltIfEndFound();
            ExploreNeighours();
        }
    }
    void HaltIfEndFound()
    {
        if (searchCenter == EndPoint)
        {
            isRunning = false;
        }
    }

    void ExploreNeighours()
    {
        if (!isRunning)
        {
            return;
        }
        foreach (Vector2Int deriction in directions)
        {
            Vector2Int neighbourCoordinates = searchCenter.GetGridPos() + deriction;
            if (BFSWayPoints.ContainsKey(neighbourCoordinates))
            {
                QueNewNeighbours(neighbourCoordinates);
            }
        }
    }

    void QueNewNeighbours(Vector2Int neighbourCoordinates)
    {
        var neighbour = BFSWayPoints[neighbourCoordinates];
        if (!neighbour.isExplored && !queue.Contains(neighbour))
        {
            queue.Enqueue(neighbour);
            neighbour.searchFrom = searchCenter;
        }
    }

    void LoadBlocks()
    {
        MyGrid[] blocks = FindObjectsOfType<MyGrid>();
        foreach (MyGrid waypoint in blocks)
        {
            Vector2Int gridpos = waypoint.GetGridPos();
            if (BFSWayPoints.ContainsKey(gridpos))
            {
                Debug.LogWarning("Skipping overlappong block on: " + waypoint);
            }
            else
            {
                if (waypoint.passable)
                {
                    BFSWayPoints.Add(gridpos, waypoint);
                }
            }
        }
        print("Loaded " + BFSWayPoints.Count + " blocks.");
    }
}
