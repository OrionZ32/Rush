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
        // ExploreNeighbours();
    }

    private void PathFind() {
        queue.Enqueue(startWaypoint);
        while(queue.Count > 0) {
            var searchCenter = queue.Dequeue();
            print("Searching from: " + searchCenter);
            HaltIfEndFound(searchCenter);
        }
        print("Finished pathfinding?");
    }

    private void HaltIfEndFound(Waypoint searchCenter) {
        if (searchCenter == endWaypoint) {
            print("Searching from end node, stopping");
            isRunning = false;
        }
    }

    private void ExploreNeighbours() {
        foreach(Vector2Int direction in directions) {
            Vector2Int explorationCoordinates = startWaypoint.GetGridPos() + direction;

            try {
                grid[explorationCoordinates].SetTopColor(Color.magenta);
            } catch {
                Debug.LogError("no blocks");
            }
            
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
            } else {
                grid.Add(gridPos, waypoint);
            }

        }
        // print("Loaded " + grid.Count + " blocks");
    }

}
