using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject enemySword;
    public int[] blocks;
    //public GameObject swordHolder;
    private static GameManager instance = null;
    public static GameManager Instance {
        get { return instance; }
    }

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        }
        else {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    //public Vector2Int GetTile(int x, int y) {
    //    if (!Tiles.ContainsKey(x) || !Tiles[x].ContainsKey(y))
    //        return new Vector2Int(0, 0);
    //    return Tiles[x][y];
    //}

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.G))
        //{
        //    SpawnEnemy(new Vector3(0, 2, 0));
        //}
    }

    /// <summary> SpawnEnemy spawns an Enemy at Origin. </summary>
    public void SpawnEnemy() {
        print("new");
        GameObject enemy = Instantiate(enemyPrefab, new Vector3(0,0,0), Quaternion.identity);
        //enemy.GetComponentInChildren<PlaceSword>().setSword(Instantiate(enemySword));

    }

    /// <summary> SpawnEnemy spawns an Enemy at spawnPoints. </summary>
    /// <param name="spawnPoints"> Points in worldSpace where EnemyPrefabs will be spawned. </param>
    public void SpawnEnemy(params Vector3[] spawnPoints) {
        foreach (Vector3 spawnPoint in spawnPoints) {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
            GameObject eSword = Instantiate(enemySword);
            Rigidbody rb = eSword.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            enemy.GetComponentInChildren<PlaceSword>().setSword(eSword, true);
        }
    }

    public PlaceSword getSwordHandler(GameObject unit)
    {
        return unit.GetComponentInChildren<PlaceSword>();
    }

}
