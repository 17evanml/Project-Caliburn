using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingHistory : MonoBehaviour
{
    private Queue<IHistoryToken> history = new Queue<IHistoryToken>();
    private Queue<IHistoryToken> undoHistory = new Queue<IHistoryToken>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Add(IHistoryToken token)
    {
        history.Enqueue(token);
    }

    public void Undo(GameManager gm)
    {
        IHistoryToken recent = history.Dequeue();
        recent.Undo(gm);
        undoHistory.Enqueue(recent);
    }

    public void Redo(GameManager gm)
    {
        IHistoryToken recent = undoHistory.Dequeue();
        recent.Redo(gm);
        history.Enqueue(recent);
    }

    public void ClearHistory()
    {
        history.Clear();
        undoHistory.Clear();
    }
    public void ClearRedo()
    {
        undoHistory.Clear();
    }
}
