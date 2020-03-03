using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    GameObject player;
    GameManager gm;
    int delay = 20;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (delay <= 0)
        {
            Vector3 toPlayer = player.transform.position - transform.position;
            float distToPlayer = toPlayer.magnitude;
            if (distToPlayer < 10)
            {
                transform.Translate(toPlayer.normalized * 0.1f);
            }
            if (distToPlayer < 1)
            {
                gm.blocks[0]++;
                Destroy(gameObject);
            }
        }
        else
        {
            delay--;
        }
        //transform.Rotate(new Vector3(0, 1, 0));
    }
}
