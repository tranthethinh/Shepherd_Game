
using System;
using UnityEngine;

public static class DataManager
{
    public static int GetSheepCount(int sheepfoldIndex)
    {
        switch (sheepfoldIndex)
        {
            case 1:
                return CountInSheepfold1;
            case 2:
                return CountInSheepfold2;
            case 3:
                return CountInSheepfold3;
            case 4:
                return CountInSheepfold4;
            case 5:
                return CountInSheepfold5;
            default:
                throw new ArgumentException("Invalid sheepfold index");
        }
    }
    public static int GetSheepCountTask(int sheepfoldIndex)
    {
        switch (sheepfoldIndex)
        {
            case 1:
                return CountTaskInSheepfold1;
            case 2:
                return CountTaskInSheepfold2;
            case 3:
                return CountTaskInSheepfold3;
            case 4:
                return CountTaskInSheepfold4;
            case 5:
                return CountTaskInSheepfold5;
            default:
                throw new ArgumentException("Invalid sheepfold index");
        }
    }
    public static int CountInSheepfold1
    {
        get => PlayerPrefs.GetInt("CountInSheepfold1", 30);
        set => PlayerPrefs.SetInt("CountInSheepfold1", value);
    }
    public static int CountTaskInSheepfold1
    {
        get => PlayerPrefs.GetInt("CountTaskInSheepfold1", CountInSheepfold1);
        set => PlayerPrefs.SetInt("CountTaskInSheepfold1", value);
    }
    public static int CountInSheepfold2
    {
        get => PlayerPrefs.GetInt("CountInSheepfold2", 10);
        set => PlayerPrefs.SetInt("CountInSheepfold2", value);
    }
    public static int CountTaskInSheepfold2
    {
        get => PlayerPrefs.GetInt("CountTaskInSheepfold2", CountInSheepfold2);
        set => PlayerPrefs.SetInt("CountTaskInSheepfold2", value);
    }
    public static int CountInSheepfold3
    {
        get => PlayerPrefs.GetInt("CountInSheepfold3", 0);
        set => PlayerPrefs.SetInt("CountInSheepfold3", value);
    }
    public static int CountTaskInSheepfold3
    {
        get => PlayerPrefs.GetInt("CountTaskInSheepfold3", CountInSheepfold3);
        set => PlayerPrefs.SetInt("CountTaskInSheepfold3", value);
    }
    public static int CountInSheepfold4
    {
        get => PlayerPrefs.GetInt("CountInSheepfold4", 40);
        set => PlayerPrefs.SetInt("CountInSheepfold4", value);
    }
    public static int CountTaskInSheepfold4
    {
        get => PlayerPrefs.GetInt("CountTaskInSheepfold4", 28);
        set => PlayerPrefs.SetInt("CountTaskInSheepfold4", value);
    }
    public static int CountInSheepfold5
    {
        get => PlayerPrefs.GetInt("CountInSheepfold5", 0);
        set => PlayerPrefs.SetInt("CountInSheepfold5", value);
    }
    public static int CountTaskInSheepfold5
    {
        get => PlayerPrefs.GetInt("CountTaskInSheepfold5", CountInSheepfold5);
        set => PlayerPrefs.SetInt("CountTaskInSheepfold5", value);
    }

    public static int DifficultyLevel
    {
        get => PlayerPrefs.GetInt("DifficultyLevel", 1);
        set => PlayerPrefs.SetInt("DifficultyLevel", value);
    }
    public static int SheepInNewMap
    {
        get => PlayerPrefs.GetInt("SheepInNewMap", 0);
        set => PlayerPrefs.SetInt("SheepInNewMap", value);
    }


 
}