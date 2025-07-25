using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PieceDistributionManager : MonoBehaviour
{
    string filedestination = null;
    public static PieceDistributionManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        filedestination = Application.streamingAssetsPath + "/PieceDistributionData.json";
    }

    public void SaveFile()
    {
        PiecesPercentages Data = GameData.Instance.PiecePercaentages;
        string jsonString = JsonUtility.ToJson(Data);
        File.WriteAllText(filedestination, jsonString);
    }

    public PiecesPercentages LoadFile()
    {
        PiecesPercentages data = null;

        if (File.Exists(filedestination))
        {
            string jsonString = File.ReadAllText(filedestination);
            data = JsonUtility.FromJson<PiecesPercentages>(jsonString);
        }
        else
        {
            SaveFile();
            return GameData.Instance.PiecePercaentages;
        }
        return data;
    }
}
