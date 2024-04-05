using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelData : ScriptableObject
{
    public Dictionary<Vector3, TileTypes> tiles;
}
