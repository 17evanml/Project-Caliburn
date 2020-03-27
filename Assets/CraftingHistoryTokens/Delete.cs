using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delete : IHistoryToken
{
    private SwordHierarchyTree tree;
    private swordHierarchyNode parent;

    public Delete(SwordHierarchyTree t, swordHierarchyNode node)
    {
        tree = t;
        parent = node;
    }


    public void Redo(GameManager gm)
    {
        throw new System.NotImplementedException();
    }

    public void Undo(GameManager gm)
    {
        parent.BuildFromTree(tree, gm);
    }

}
