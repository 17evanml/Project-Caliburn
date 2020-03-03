using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordHierarchyNode : MonoBehaviour
{
    SwordHierarchyTree self;
    public List<swordHierarchyNode> children = new List<swordHierarchyNode>();

    public GameObject[] snapSpots;
    public GameObject[] misc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void add(SwordHierarchyTree s)
    {
        self = s;
    }

    public SwordHierarchyTree getSelf()
    {
        return self;
    }

    public void toggleSnapspots(bool ac)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if (i < snapSpots.Length)
            {
                snapSpots[i].SetActive(ac);
            }
            else if (i < snapSpots.Length + misc.Length)
            {

            }
            else
            {
                transform.GetChild(i).GetChild(0).GetComponent<swordHierarchyNode>().toggleSnapspots(ac);
            }
        }

    }

    public int getSnapSpots()
    {
        return snapSpots.Length;
    }

    public void addChild(swordHierarchyNode g)
    {
        children.Add(g);
    }
    public void removeChild(swordHierarchyNode g)
    {
        children.Remove(g);
    }
}
