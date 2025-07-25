using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class OptionTemplate
{
    public int SinglePlayerGameRows;
    public int SinglePlayerGameColumns;
    public int TwoPlayerGameRows;
    public int TwoPlayerGameColumn;
    public int ObstacleCoverage;
    public bool ShowOptionStatus;
    public float FlowSpeed;
    public string CompundSpeed;
    public bool DisplayHighScore;
    public int LengthPoints;
    public int CrossPoints;
    public int ClosedLoopPoints;
    public int TileReplacementPenalty;
    public int UnusedTilePenalty;
    public int StartingPieceTimer;
    public int MultiplayerLoopCompleted;
    public int RowColumnIncreaseRate;
    public string BombProbability;
    public bool UpcomingTileStatus;
    public float StartingPieceTimeDifference;
    public float DifficultyStartingPieceTimeReduction;
    public int MinimumStartingTimer;
    public float DifficultFlowSpeed;
    public int AuxillaryTimer;
}
[Serializable]
public class BrokenPieceImage
{
    public Vector2 FlowVector;
    public List<TargetBrokenImage> targetImages = new List<TargetBrokenImage>();
}

[Serializable]
public class TargetBrokenImage
{
    public int tileCode;
    public GameObject BrokenFlowImage;
}



public class StripesTemplate : MonoBehaviour
{
    
}
