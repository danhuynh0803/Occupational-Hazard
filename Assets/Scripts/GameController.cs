using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public static int deathCount;
    public static float score;
    [SerializeField]
    public static float highScore;
    public Transform minSpawnBound;
    public Transform maxSpawnBound;

    public float minHazardCooldown = 2.0f;
    public float maxHazardCooldown = 8.0f;

    public GameObject[] hazards;
    public GameObject GameOverPanel;
    public Text GameOverMessageText;
    public string gameOverMessage;
    private float timer;

    public static bool isGameOver;

	// Use this for initialization
	void Start () {
        timer = UnityEngine.Random.Range(minHazardCooldown, maxHazardCooldown);
        Load();
        score = 0;
        isGameOver = false;
        GameOverPanel.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!isGameOver)
        {            
            score += Time.deltaTime; // Score is based on how long player can keep VIP alive
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SpawnHazard();
            }
        }
        else
        {
            GameOver();
        }
    }

    void SpawnHazard()
    {
        float y = UnityEngine.Random.Range(
                minSpawnBound.position.y, maxSpawnBound.position.y);
        Vector3 spawnPos = new Vector3(
            UnityEngine.Random.Range(
                minSpawnBound.position.x, maxSpawnBound.position.x),
             y, y);

        // Spawn the object in a random position
        // within the box designated by the minSpawn and maxSpawn
        Instantiate(hazards[UnityEngine.Random.Range(0, hazards.Length)],
                    spawnPos, Quaternion.identity);
        // Decrement the spawn timer, so that the level gets more difficult
        Mathf.Clamp(maxHazardCooldown -= 0.1f, minHazardCooldown, maxHazardCooldown);
        // Reset spawn timer
        timer = UnityEngine.Random.Range(minHazardCooldown, maxHazardCooldown);
    }

    void GameOver()
    {
        GameOverPanel.SetActive(true);

        TimeSpan timeSpan = TimeSpan.FromSeconds(GameController.score);
        GameOverMessageText.text = gameOverMessage + "\n" + string.Format("<color=#ff0000>{0:D2}</color> min <color=#ff0000>{1:D2}</color> sec <color=#ff0000>{2:D2}</color> ms",
            timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);

        if (score >= highScore)
        {
            highScore = score;
            Save(highScore);
        }


    }
    public static void Save(float highScore)
    {
        BinaryFormatter bf = new BinaryFormatter();
        //remeber to change the file name during each Ludum Dare
        FileStream saveFile = 
            File.Create(Application.persistentDataPath + "/LD43PlayerData.dat");
        PlayerData data = new PlayerData(highScore);
        data.highScore = highScore;
        bf.Serialize(saveFile, data);
        saveFile.Close();

        //UI.cs updates highscore to score, and the HUD
        //this if statement must uses >= sign
        /* if (score >= highScore), 
        {
            highScore = score;
            Save(highScore);
        }*/
    }
    // call this once in start to create a saveFile
    public static void Load()
    {
        BinaryFormatter bf = new BinaryFormatter();
        try
        {
            System.IO.FileStream saveFile = 
                File.Open(Application.persistentDataPath + "/LD43PlayerData.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(saveFile);
            saveFile.Close();
            highScore = data.highScore;
        }
        catch (FileNotFoundException e)
        {
            //create a save file
            Save(0f);
        }
    }
}
[Serializable]
public class PlayerData
{
    public float highScore;
    public PlayerData(float highScore)
    {
        this.highScore = highScore;
    }
}

