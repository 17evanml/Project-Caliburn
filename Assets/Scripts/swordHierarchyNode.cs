using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordHierarchyNode : MonoBehaviour
{
    private SwordHierarchyTree self;
    public List<swordHierarchyNode> children = new List<swordHierarchyNode>();
    int Block { get { return Self.getBlock(); } }
    Vector3 Position { get { return Self.getPos(); } }
    Quaternion Rotation { get { return Self.getRot(); } }
    //SwordHierarchyTree Parent { get { return Self.getParent(); } }
    swordHierarchyNode parent;
    public SwordHierarchyTree Self { get { return self; } }
    public GameObject[] snapSpots;
    public GameObject[] misc;

    public void add(SwordHierarchyTree s)
    {
        self = s;
    }
    public void SetParent(swordHierarchyNode node)
    {
        parent = node;
    }
    public swordHierarchyNode GetParent()
    {
        return parent;
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

    public void DeleteBlock(GameManager gm)
    {
        self.getParent().RemoveChild(self);
        gm.blocks[Block]++;
        for(int i = 0; i < children.Count; i++)
        {
            children[i].DeleteBlock(gm);
        }
        Destroy(gameObject);
    }

    public void BuildFromTree(SwordHierarchyTree tree, GameManager gm)
    {
        GameObject g = Instantiate(gm.blockObjects[tree.getBlock()], tree.getPos(), tree.getRot());
        swordHierarchyNode node = g.GetComponent<swordHierarchyNode>();
        addChild(node);
        for(int i = 0; i < tree.getChildren().Count; i++)
        {
            node.BuildFromTree(tree.getChildren()[i], gm);
        }
    }
}
