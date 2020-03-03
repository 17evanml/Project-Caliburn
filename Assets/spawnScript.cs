using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnBehavior { Random, Sequential, All, }
public enum SpawnBehavior2 { Zone, Point }
public class spawnScript : MonoBehaviour
{
    int counter = 0;
    public List<spawnZone> spawnZones;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            triggerspawn(spawnZones[0]);
        }
    }


    void triggerspawn(spawnZone s)
    {
        print(s.behavior);

        if (s.behavior.Equals(SpawnBehavior.Random))
        {
            int point = UnityEngine.Random.Range(0, s.spawnpoints.Count);
            int unit = UnityEngine.Random.Range(0, s.enemies.Count);
            spawnAt(s.enemies[unit], s.spawnpoints[point], s);

        } else if(s.behavior.Equals(SpawnBehavior.Sequential))
        {
            if (s.enemies.Count == s.spawnpoints.Count)
            {
                spawnAt(s.enemies[counter], s.spawnpoints[counter], s);
            } else 
            {
                spawnAt(s.enemies[0], s.spawnpoints[counter], s);
            }
            counter++;
            if(counter >= s.spawnpoints.Count)
            {
                counter = 0;
            }
        } else if(s.behavior.Equals(SpawnBehavior.All))
        {
            for(int i = 0; i < s.spawnpoints.Count; i++)
            {
                if (s.enemies.Count == s.spawnpoints.Count)
                {
                    spawnAt(s.enemies[i], s.spawnpoints[i], s);
                }
                else
                {
                    spawnAt(s.enemies[0], s.spawnpoints[i], s);
                }
            }
        }
    }


    void spawnAt(GameObject unit, GameObject point, spawnZone s)
    {
        if (s.style.Equals(SpawnBehavior2.Point))
        {
            Instantiate(unit, point.transform.position, point.transform.rotation);
        }
        else if (s.style.Equals(SpawnBehavior2.Zone))
        {
            Bounds b = point.GetComponent<Collider>().bounds;
            Vector3 spawnpoint = new Vector3(Random.Range(b.min.x, b.max.x), Random.Range(b.min.y, b.max.y), Random.Range(b.min.z, b.max.z));
            Instantiate(unit, spawnpoint, point.transform.rotation);
        }
        else
        {

        }
    }
}
