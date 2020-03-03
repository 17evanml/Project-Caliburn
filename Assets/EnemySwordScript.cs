using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwordScript : MonoBehaviour
{

    public string enemy;
    public GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        toggleSnapSpots(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.root.tag.Equals(enemy))
        {
            if (parent.GetComponent<EnemyController>().isAttacking == true)
            {
                other.transform.root.GetComponent<playerProperties>().takeDamage(2);
            }
        }
    }

    public void toggleSnapSpots(bool ac)
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetChild(0).GetComponent<swordHierarchyNode>().toggleSnapspots(ac);
        }
    }

    public void setParent(GameObject nParent)
    {
        parent = nParent;
    }
}
