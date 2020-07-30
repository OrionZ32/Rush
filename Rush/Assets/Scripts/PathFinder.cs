using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour {

    Dictionary<Vector2Int, Waypoint> grid = new Dictionary<Vector2Int, Waypoint>();
    Queue<Waypoint> queue = new Queue<Waypoint>();

    [SerializeField] Waypoint startWaypoint, endWaypoint;
    bool isRunning = true;
    Waypoint searchCenter;
    
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
            searchCenter = queue.Dequeue();
            HaltIfEndFound();
            ExploreNeighbors();
            searchCenter.isExplored = true;
        }
        print("Finished pathfinding?");
    }

    private void HaltIfEndFound() {
        if (searchCenter == endWaypoint) {
            isRunning = false;
        }
    }

    private void ExploreNeighbors() {
        if (!isRunning) {return;}

        foreach(Vector2Int direction in directions) {
            Vector2Int neighborCoordinates = searchCenter.GetGridPos() + direction;
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

        if (neighbor.isExplored || queue.Contains(neighbor)) {
            // do nothing
        }
        else {    
            queue.Enqueue(neighbor);
            neighbor.exploredFrom = searchCenter;
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
