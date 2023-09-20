using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MoveStopMove.Core.Data;
public interface IPersistentData 
{
    void LoadGame(GameData data);
    void SaveGame(ref GameData data);
}
