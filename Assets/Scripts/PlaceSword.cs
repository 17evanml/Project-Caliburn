using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceSword : MonoBehaviour
{
    GameObject sword;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        

    }

    void snap()
    {
        sword.transform.parent = this.transform.parent;
        sword.transform.position = this.transform.position;
        sword.transform.rotation = this.transform.rotation;
        sword.GetComponent<swordScript>().setParent(this.transform.root.gameObject);
    }

    void snapEnemy()
    {
        sword.transform.parent = this.transform.parent;
        sword.transform.position = this.transform.position;
        sword.transform.rotation = this.transform.rotation;
        sword.GetComponent<EnemySwordScript>().setParent(this.transform.root.gameObject);
    }

    public void setSword(GameObject nSword)
    {
        sword = nSword;
        snap();
    }
    public void setSword(GameObject nSword, bool t)
    {
        sword = nSword;
        snapEnemy();
    }
}
