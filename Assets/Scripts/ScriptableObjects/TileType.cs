using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class TileTypes
{
    public int TilePieceType = 0;
    public string TileName = "";
    public TileCode[] TileCode;
}
[System.Serializable]
public class TileCode
{
    public float tileAngle = 0f;
    public double[] tileCode = new double[4];
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TileType", order = 1)]
public class TileType : ScriptableObject
{
    public int Type = 0;  //Type Defination Type 0 => Ltype1,  1 => Ltype2, 2 => straightType, 3 => crosstype 4 => bombtype
    public float Angle = 0f; //Possible angle for all the pieces are 0, 90, 180 , 270 during game expcept Bomb Piece(It cannot be rotated)
    public bool Mask = true; //Mask is used to show the Invalid connection when two piece of diffrent gradient joins
    public Sprite PieceImage;
    public List<TileTypes> TilePiecesCodes = new List<TileTypes>();
}



