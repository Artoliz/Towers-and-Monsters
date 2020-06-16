using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScores : MonoBehaviour
{

    [System.Serializable]
    public class HighScore
    {
        public string name;
        public int score;

        public HighScore(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
    }

    private class HighScoreList
    {
        public List<HighScore> highScoreList;
    }
    
    public string HighScoreListName;

    public List<Text> entryNames;
    public List<Text> entryScores;

    private List<HighScore> highScoreList = new List<HighScore>();
    
    void Start()
    {
        if (PlayerPrefs.HasKey(HighScoreListName) == false)
        {
            HighScoreList highScores = new HighScoreList { highScoreList = highScoreList};
            string json = JsonUtility.ToJson(highScores);
            PlayerPrefs.SetString(HighScoreListName, json);
            PlayerPrefs.Save();
        }
        //PlayerPrefs.DeleteKey(HighScoreListName);
    }

    public bool IsBetterScore(int score)
    {
        if (highScoreList[0].score < score)
            return true;
        return false;
    }

    public void SaveScore(string name, int score)
    {
        int i = 0;
        
        while (i < highScoreList.Count && score < highScoreList[i].score)
            i++;
        highScoreList.Insert(i, new HighScore(name, score));
        
        HighScoreList highScores = new HighScoreList { highScoreList = highScoreList};
        string json = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString(HighScoreListName, json);
        PlayerPrefs.Save();
    }

    private void refreshScoreList()
    {
        string json = PlayerPrefs.GetString(HighScoreListName);

        if (json.Length == 0)
            return;
        
        highScoreList = JsonUtility.FromJson<HighScoreList>(json).highScoreList;
    }

    public void UpdateScores()
    {
        refreshScoreList();
        int i = 0;

        while (i < highScoreList.Count && i < entryNames.Count && i < entryScores.Count)
        {
            entryNames[i].text = highScoreList[i].name;
            entryScores[i].text = highScoreList[i].score.ToString();
            i++;
        }
    }

    private void OnEnable()
    {
        UpdateScores();
    }
}
