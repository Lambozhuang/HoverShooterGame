using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject turret;
    public Vector3 spawnValues;
    public int maxTurretNum;
    public int currentTurrentNum;
    public int score = 0;
    private VehicleController vehicleController;

    // UI
    public Text titleText;
    public Text finalScoreText;
    public Text guideText;
    public Text centerPrompt;
    public Text healthText;
    public Text scoreText;
    public Text timeText;
    public Button button;

    public float time;

    public GameState state = GameState.BeforeStart;
    public enum GameState
    {
        BeforeStart,
        ReadyToStart,
        Playing,
        GameOver
    }


    // Start is called before the first frame update
    void Start()
    {
        GameObject vehicleControllerObject = GameObject.FindWithTag("Player");
        vehicleController = vehicleControllerObject.GetComponent<VehicleController>();

        for (int i = 0; i < maxTurretNum; i++)
        {
            SpawnTurret();
        }
        currentTurrentNum = maxTurretNum;

        finalScoreText.gameObject.SetActive(false);
        guideText.gameObject.SetActive(false);
        centerPrompt.gameObject.SetActive(false);
        healthText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);

        DisplayTime(time);
        
        button.onClick.AddListener(() => {
            if (state == GameState.BeforeStart)
            {
                state = GameState.ReadyToStart;
                titleText.gameObject.SetActive(false);
                guideText.gameObject.SetActive(true);
                button.gameObject.SetActive(false);
            }
            else if (state == GameState.GameOver)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0 && state == GameState.Playing)
        {
            time -= Time.deltaTime;
            DisplayTime(time);
        }
        else if (time <= 0)
        {
            timeText.text = "Time: 00:00";
            GameOver();
        }

        if (currentTurrentNum <= 0)
        {
            GameOver();
        }

        healthText.text = "HP: " + vehicleController.health + "/100";
    }

    void SpawnTurret()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, Random.Range(-spawnValues.z, spawnValues.z));
        Quaternion spawnRotation = Quaternion.identity;
        Instantiate(turret, spawnPosition, spawnRotation);
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        scoreText.text = "Score: " + score;
    }

    public void StartGame()
    {
        state = GameState.Playing;
        guideText.gameObject.SetActive(false);
        healthText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true);

    }

    public void GameOver()
    {
        state = GameState.GameOver;
        button.GetComponentInChildren<Text>().text = "Restart";
        button.gameObject.SetActive(true);
        centerPrompt.gameObject.SetActive(true);
        finalScoreText.text = "Final Score: " + score;
        finalScoreText.gameObject.SetActive(true);
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds);
        if (timeToDisplay < 10)
        {
            if (seconds % 2 == 0)
            {
                timeText.color = Color.red;
            }
            else
            {
                timeText.color = Color.white;
            }
        }
    }
}
