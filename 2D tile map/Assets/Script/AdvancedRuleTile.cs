using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "VinTools/Custom Tiles/Advanced Rule Tile 2")]
public class CustomRuleTile_Rivers : RuleTile<CustomRuleTile_Rivers.Neighbor>
{
    [Header("Advanced Tile")]
    [Tooltip("If enabled, the tile will connect to these tiles too when the mode is set to \"This\"")]
    public bool alwaysConnect;
    [Tooltip("Tiles to connect to")]
    public TileBase[] tilesToConnect3;
    [Space]
    [Tooltip("Tiles to connect to")]
    public TileBase[] tilesToConnect4;
    [Space]
    [Tooltip("Tiles to connect to")]
    public TileBase[] tilesToConnect5;
    [Space]
    [Tooltip("Tiles to connect to")]
    public TileBase[] tilesToConnect6; //ATTENTION : Sert juste à rien OU appartient à cette liste. 
    [Space]
    [Tooltip("Check itseft when the mode is set to \"any\"")]
    public bool checkSelf = true;

    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        public const int Specified3 = 3;
        public const int Specified4 = 4;
        public const int Specified5 = 5;
        public const int Specified6 = 6;
        public const int Nothing = 0;
    }

    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        switch (neighbor)
        {
            case Neighbor.This: return Check_This(tile);
            case Neighbor.NotThis: return Check_NotThis(tile);
            case Neighbor.Specified3: return Check_Specified3(tile);
            case Neighbor.Specified4: return Check_Specified4(tile);
            case Neighbor.Specified5: return Check_Specified5(tile);
            case Neighbor.Specified6: return Check_Specified6(tile);
            case Neighbor.Nothing: return Check_Nothing(tile);
        }
        return base.RuleMatch(neighbor, tile);
    }
    bool Check_This(TileBase tile)
    {
        if (!alwaysConnect) return tile == this;
        else return (tilesToConnect3.Contains(tile) || tilesToConnect4.Contains(tile) || tilesToConnect5.Contains(tile)) || tile == this;
        //.Contains requires "using System.Linq;"
    }
    bool Check_NotThis(TileBase tile)
    {
        if (!alwaysConnect) return tile != this;
        else return !tilesToConnect3.Contains(tile) && !tilesToConnect4.Contains(tile) && !tilesToConnect5.Contains(tile) && tile != this;
        //.contains requires "using system.linq;"
    }
    bool Check_Specified3(TileBase tile)
    {
        if (checkSelf) return tile != null;
        return tile != null && tilesToConnect3.Contains(tile);
    }
    bool Check_Specified4(TileBase tile)
    {
        if (checkSelf) return tile != null;
        return tile != null && tilesToConnect4.Contains(tile);
    }
    bool Check_Specified5(TileBase tile)
    {
        //Vrai si élément null ou € tilesToConnect5
        //if (checkSelf) return tile != null;
        bool var = tile == null || tilesToConnect5.Contains(tile);
        //Debug.Log(var);
        return var;
    }
    bool Check_Specified6(TileBase tile)
    {
        if (checkSelf) return tile != null;
        return tile == null || tilesToConnect6.Contains(tile);
    }
    bool Check_Nothing(TileBase tile)
    {
        return tile == null;
    }
}