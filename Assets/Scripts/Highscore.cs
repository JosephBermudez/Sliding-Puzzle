using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Highscore : MonoBehaviour
{
    [SerializeField]
    private Transform entryContainer = null;
    [SerializeField]
    private Transform entryTemplate = null;
    [SerializeField]
    private Text position = null, score = null, namePlayer = null;

    public List<HighscoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;

    private int rank;
    private string persistentDataPath = "";
    private string path = "";
    private string json = "";

    private void Awake()
    {
        entryTemplate.gameObject.SetActive(false);

        highscoreEntryList = new List<HighscoreEntry>()
        {
            new HighscoreEntry{ score = 145, name = "ARA"},
            new HighscoreEntry{ score = 333, name = "BER"},
            new HighscoreEntry{ score = 242, name = "TAR"},
            new HighscoreEntry{ score = 177, name = "MUR"},
        };

        for(int i = 0; i < highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscoreEntryList.Count; j++)
            {
                if (highscoreEntryList[j].score > highscoreEntryList[i].score)
                {
                    HighscoreEntry tmp = highscoreEntryList[i];
                    highscoreEntryList[i] = highscoreEntryList[j];
                    highscoreEntryList[j] = tmp;
                }
            }
        }
        SetPaths();
        SaveData();
        

        highscoreEntryTransformList = new List<Transform>();
        foreach(HighscoreEntry highscoreEntry in highscoreEntryList)
        {
        CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    }

    public void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 20.0f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            case 1:
                rankString = "1st";
                break;
            case 2:
                rankString = "2nd";
                break;
            case 3:
                rankString = "3rd";
                break;
            default:
                rankString = rank + "th";
                break;
        }

        entryTransform.Find("rankText").GetComponent<Text>().text = rankString;

        int score = highscoreEntry.score;
        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

        string name = highscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<Text>().text = name;

        transformList.Add(entryTransform);
    }


    [System.Serializable]
    public class HighscoreEntry
    {
        public int score;
        public string name;
    }

    public class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    private void SetPaths()
    {
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json";

        persistentDataPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
    }

    public void SaveData()
    {
        Highscores highscores = new Highscores { highscoreEntryList = highscoreEntryList };

        HighscoreEntry highScoreEntryBase = new HighscoreEntry();
        json = JsonUtility.ToJson(highscores, true);
        string savePath = persistentDataPath;
        Debug.Log("Saving Data at " + savePath);

        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);
    }

    public void LoadData()
    {
        using StreamReader reader = new StreamReader(persistentDataPath);
        json = reader.ReadToEnd();

        PlayerData data = JsonUtility.FromJson<PlayerData>(json);
    }
}
