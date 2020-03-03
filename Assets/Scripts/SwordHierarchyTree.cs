using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHierarchyTree
{
    GameObject block;
    Vector3 position;
    Quaternion rotation;
    SwordHierarchyTree parent;
    List<SwordHierarchyTree> children = new List<SwordHierarchyTree>();
    public SwordHierarchyTree(GameObject g, Vector3 pos, Quaternion rot, SwordHierarchyTree p)
    {
        block = g;
        position = pos;
        rotation = rot;
        parent = p;
        if (p != null)
        {
            p.addChild(this);
        }
    }

    public void addChild(SwordHierarchyTree c)
    {
        children.Add(c);
    }
    public void resetBlock(GameObject g)
    {
        block = g;
    }
    public List<SwordHierarchyTree> getChildren()
    {
        return children;
    }

    public GameObject getBlock()
    {
        return block;
    }
    public Vector3 getPos()
    {
        return position;
    }
    public Quaternion getRot()
    {
        return rotation;
    }
    public SwordHierarchyTree getParent()
    {
        return parent;
    }

    //public string getType()
    //{
    //    return type;
    //}


}
