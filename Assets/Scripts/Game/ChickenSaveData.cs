using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ChickenSaveData 
{
    public int Level;
    public float PosX, PosY;

    public ChickenSaveData(int level, Vector2 position)
    {
        Level = level;
        PosX = position.x;
        PosY = position.y;
    }

    public Vector2 GetPosition() => new Vector2(PosX, PosY);
}
