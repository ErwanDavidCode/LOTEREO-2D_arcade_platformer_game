using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTileAtlas", menuName = "Tile Atlas")]
public class TileAtlas : ScriptableObject
{
    //on référence toutes nos Tile dans cet Atlas
    public TileClass foldTile;

    public TileClass waterLeftTile;
    public TileClass waterRightTile;

    public TileClass grassTile;
    public TileClass dirtTile;
    public TileClass stoneTile;
    public TileClass clayTile;
    public TileClass bottomDirtTile;
    public TileClass bottomStoneTile;

    public TileClass trunkTile;
    public TileClass leafTile;

    public TileClass herbTile;
    public TileClass flower1Tile;
    public TileClass lianaTile;

    public TileClass goldTile;

    //Objects
    public TileClass doorCloseTile;
    public TileClass doorOpenTile;

    //Snow
    public TileClass snowTile;
    public TileClass darkStoneTile;
    public TileClass freezeTrunkTile;
    public TileClass freezeLeafTile;
    public TileClass freezeFlower1Tile;
    public TileClass freezeLianaTile;
}
