using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHistoryToken
{
    void Undo(GameManager gm);
    void Redo(GameManager gm);
}
