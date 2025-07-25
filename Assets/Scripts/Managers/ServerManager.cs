using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class ServerManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SendData(Constants.SERVER_URL));
       // StartCoroutine(GetText(Constants.FILE_URL));
        
    }

    IEnumerator SendData(string url)
    {
        WWWForm leaderboardObj = new WWWForm();
        LeaderBoardData data0 = new LeaderBoardData("Sandeep", 5000, 1);
        LeaderBoardData data1 = new LeaderBoardData("Sahil", 4000, 2);
        LeaderBoardData data2 = new LeaderBoardData("Nihil", 3000, 3);
        LeaderBoardData data3 = new LeaderBoardData("Anshu", 3000, 4);
        

        LeaderBoardDataList dataList = new LeaderBoardDataList();
        UnityWebRequest www = UnityWebRequest.Get(Constants.FILE_URL + "LeaderBoardData" + SystemInfo.deviceUniqueIdentifier);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            LeaderBoardDataList data = JsonUtility.FromJson<LeaderBoardDataList>(www.downloadHandler.text);
            dataList.leaderboardlists = data.leaderboardlists;
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }

        if (dataList.leaderboardlists == null)
        {
            Debug.Log("List is null");
            dataList.leaderboardlists = new List<LeaderBoardData>();
        }
        dataList.leaderboardlists.Add(data0);
        dataList.leaderboardlists.Add(data1);
        dataList.leaderboardlists.Add(data2);
        dataList.leaderboardlists.Add(data3);
        string jsonsTRING = JsonUtility.ToJson(dataList);
        /*leaderboardObj.AddField("NAME", "Sahil");
        leaderboardObj.AddField("SCORE", "1000");*/
        leaderboardObj.AddField("JSONDATA", jsonsTRING);
        leaderboardObj.AddField("FILENAME", "LeaderBoardData"+ SystemInfo.deviceUniqueIdentifier);
        WWW www2 = new WWW(url, leaderboardObj);
        yield return www2;
        if(www2.error != null)
        {
            Debug.Log("UnsucessFull send" + www2.error);
        }
        else
        {
            Debug.Log(www2.text);
        }

    }
   

    IEnumerator GetText(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url + "LeaderBoardData" + SystemInfo.deviceUniqueIdentifier);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            LeaderBoardDataList data = JsonUtility.FromJson<LeaderBoardDataList>(www.downloadHandler.text);
            // Show results as text
            Debug.Log(www.downloadHandler.text);
            
            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }


    /*IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri + "LeaderBoardData" + SystemInfo.deviceUniqueIdentifier))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    File FILE = webRequest.result.;
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.responseCode);
                    break;
            }
        }
    }*/
}
