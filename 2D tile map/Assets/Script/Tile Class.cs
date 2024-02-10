using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTileClass", menuName = "Tile Class")]
public class TileClass : ScriptableObject
{
    //on spécifie nos Tile et leurs caractéristiques dans cette TileClass
    public string tileName;
    public RuleTile tileSprite;
    public float freqOre;
    public int veinMaxSizeOre;
    public int seed;
    public float flatMultiplicator;

}
