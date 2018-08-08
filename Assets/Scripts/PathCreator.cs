using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PathCreator : MonoBehaviour 
{
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

    private void Start()
    {
        
    }

    public List<MyGrid> GetPath()
    {
        return Path;
    }

    public void ClearPath()
    {
        Path = null;
    }

    public void GeneratePath()
    {
        LoadBlocks();
        BreadthFirstSearch();
        CreatePath();
    }

    private void CreatePath()
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

    private void BreadthFirstSearch()
    {
        queue.Enqueue(StartPoint);
        while (queue.Count> 0 && isRunning)
        {
            searchCenter = queue.Dequeue();
            searchCenter.isExplored = true;
            HaltIfEndFound();
            ExploreNeighours();
        }
    }
    private void HaltIfEndFound()
    {
        if (searchCenter == EndPoint)
        {
            isRunning = false;  
        }
    }

    private void ExploreNeighours()
    {
        if (!isRunning)
        {
            return;
        }
        foreach(Vector2Int deriction in directions)
        {
            Vector2Int neighbourCoordinates = searchCenter.GetGridPos() + deriction;
            if (BFSWayPoints.ContainsKey(neighbourCoordinates))
            {
                QueNewNeighbours(neighbourCoordinates);
            }
        }
    }

    private void QueNewNeighbours(Vector2Int neighbourCoordinates)
    {
        var neighbour = BFSWayPoints[neighbourCoordinates];
        if (!neighbour.isExplored && !queue.Contains(neighbour))
        {
            queue.Enqueue(neighbour);
            neighbour.searchFrom = searchCenter;
        }
    }

    private void LoadBlocks()
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
