using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public static class Utillities 
{
    public static string RemoveWhiteSpaces(string name)
    {
        string Name = name.Replace(" ", String.Empty);
        return Name;
    }

    public static bool ApplyValidation(string name)
    {
        bool status = false;
        status = (string.IsNullOrEmpty(name)) ? status : (name.Length < Constants.NAME_SHORTERLENGTH || name.Length > Constants.NAME_LONGERLENGTH) ? status : true;
        return status;
    }
    public static bool CompareName(string name1, string name2)
    {
        bool status = false;
        status = (name1 == name2) ? true : false;
        return status;
    }
#if UNITY_EDITOR

    [MenuItem("Tool/ClearPlayerPrefs")]
    public static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
#endif
}
