using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    //Custom Implementations
    private string highPlayerName;
    private int highPlayerScore;

    public Text highScoreText;

    public int initialZeroScore = 0;
    //

    public void Awake()
    {
        //custom implementations
        LoadPlayerData();
        //DeletePlayerData();
    }

    void Start()
    {
        // custom Implementations
        highScoreText.text = $"Score: {highPlayerName}: {highPlayerScore}";
        //

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            //custom implementations
            //if the score is bigger, replace them
            if(m_Points > highPlayerScore)
            {
                //update the score
                highScoreText.text = $"Score: {PlayerInfoManager.Instance.playerName}: {m_Points}";
                //save the player score data
                SavePlayerData();

            }
            //

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }


    //custom implementations
    [System.Serializable]
    class PlayerSaveData
    {
        public string playerName;
        public int playerScore;
    }

    //custom implementations
    public void SavePlayerData()
    {
        PlayerSaveData playerData = new PlayerSaveData();
        playerData.playerName = PlayerInfoManager.Instance.playerName;
        playerData.playerScore = m_Points;

        string json = JsonUtility.ToJson(playerData);

        File.WriteAllText(Application.persistentDataPath + "/playersavefile.json",json);
    }

    //custom implemetations
    public void LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/playersavefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerSaveData playerData = JsonUtility.FromJson<PlayerSaveData>(json);
            highPlayerName = playerData.playerName;
            highPlayerScore = playerData.playerScore;

        }
    }

    //custom implementation
    public void DeletePlayerData()
    {
        string path = Application.persistentDataPath + "/playersavefile.json";
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("File deleted susscessful .");
        }
        else
        {
            Debug.Log("File doesn't exist.");
        }
    }

}
