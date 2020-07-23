using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour {

    [SerializeField] Waypoint startWaypoint, endWaypoint;
    [SerializeField] bool isRunning = true;
    Dictionary<Vector2Int, Waypoint> grid = new Dictionary<Vector2Int, Waypoint>();
    Queue<Waypoint> queue = new Queue<Waypoint>();
    
    Vector2Int[] directions = {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

    void Start() {
        LoadBlocks();
        ColorStartAndEnd();
        PathFind();
        // Exploreneighbors();
    }

    private void PathFind() { 
        queue.Enqueue(startWaypoint);
        while(queue.Count > 0 && isRunning) {
            var searchCenter = queue.Dequeue();
            print("Searching from: " + searchCenter);
            HaltIfEndFound(searchCenter);
            ExploreNeighbors(searchCenter);
            searchCenter.isExplored = true;
        }
        print("Finished pathfinding?");
    }

    private void HaltIfEndFound(Waypoint searchCenter) {
        if (searchCenter == endWaypoint) {
            print("Searching from end node, stopping");
            isRunning = false;
        }
    }

    private void ExploreNeighbors(Waypoint from) {
        if (!isRunning) {return;}

        foreach(Vector2Int direction in directions) {
            Vector2Int neighborCoordinates = from.GetGridPos() + direction;
            try {
                QueueNewNeighbors(neighborCoordinates);
            } 
            catch {
                // Debug.LogError("no blocks");
            }
        }
    }

    private void QueueNewNeighbors(Vector2Int neighborCoordinates) {
        Waypoint neighbor = grid[neighborCoordinates];

        if (neighbor.isExplored) {

        }
        else {    
            neighbor.SetTopColor(Color.magenta);
            queue.Enqueue(neighbor);
            print("Queuing " + neighbor);
        }
    }

    private void ColorStartAndEnd() {
        startWaypoint.SetTopColor(Color.green);
        endWaypoint.SetTopColor(Color.red);
    }

    private void LoadBlocks() {
        var waypoints = FindObjectsOfType<Waypoint>();
        foreach (Waypoint waypoint in waypoints) {
            var gridPos = waypoint.GetGridPos();

            if (grid.ContainsKey(gridPos)) {
                Debug.LogWarning("Skipping overlapping block " + waypoint);
            } 
            else {
                grid.Add(gridPos, waypoint);
            }
        }
        // print("Loaded " + grid.Count + " blocks");
    }

}
