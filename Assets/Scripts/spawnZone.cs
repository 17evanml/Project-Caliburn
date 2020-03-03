using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class spawnZone
{
    public SpawnBehavior behavior;
    public SpawnBehavior2 style;
    public List<GameObject> spawnpoints = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();
}
