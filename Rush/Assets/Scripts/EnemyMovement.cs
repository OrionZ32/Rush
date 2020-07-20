using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMovement : MonoBehaviour {

[SerializeField] List<Block> path;

    void Start() {

        foreach (Block waypoint in path) {
            print(waypoint.name);
        }
        
    }

    void Update() {
        
    }
}
