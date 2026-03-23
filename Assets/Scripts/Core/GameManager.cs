using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public APIService apiService;
    public TrafficSpawner spawner;
    public PlayerController player;
    public PredictionScheduler scheduler;
    public HUDController hud;
    public GameOverUI gameOverUI;
    public Transform playerSpawnPoint;

    private TrafficResponse currentData;
    private int level = 1;
    private float levelTimer;

    private Status[] levelStates; 
    private int currentIndex = 0;

    void Start()
    {
        currentData = apiService.GetTrafficData();
        if (currentData == null) return;

        BuildLevelStates();
        LoadLevel();
    }

    void BuildLevelStates()
    {
        int total = currentData.predicted_status.Count;
        levelStates = new Status[total];

        for (int i = 0; i < total; i++)
        {
            levelStates[i] = currentData.predicted_status[i].predictions;
        }
    }

    void LoadLevel()
    {
        StopAllCoroutines();

        if (levelStates == null || levelStates.Length == 0) return;

        currentIndex = Mathf.Clamp(currentIndex, 0, levelStates.Length - 1);
        Status statusToApply = levelStates[currentIndex];

        // RESET DO PLAYER
        if (player != null && playerSpawnPoint != null)
        {
            player.transform.position = playerSpawnPoint.position;

            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
            }
        }

        // APLICA DADOS
        spawner.SetTraffic(statusToApply);
        player.SetWeather(statusToApply.weather);

        levelTimer = 0f;

        if (currentData.predicted_status != null && currentData.predicted_status.Count > 0)
        {
            int timeIndex = Mathf.Clamp(currentIndex, 0, currentData.predicted_status.Count - 1);
            int currentTime = currentData.predicted_status[timeIndex].estimated_time;

            Debug.LogWarning("estimated_time atual = " + currentTime);
            Debug.LogWarning("index = " + timeIndex);

            levelTimer = currentTime / 1000f;
        }
        else
        {
            Debug.LogWarning("Sem predições - tempo padrão usado");
            levelTimer = 10f;
        }

        // Agenda predições para os próximos níveis
        scheduler.SchedulePredictions(currentData.predicted_status);

        StartCoroutine(LevelTimerCoroutine());
    }

    IEnumerator LevelTimerCoroutine()
    {
        float timeLeft = levelTimer;

        while (timeLeft > 0)
        {
            hud.UpdateHUD(level, levelStates[currentIndex], timeLeft);
            timeLeft -= Time.deltaTime;
            yield return null;
        }

        GameOver();
    }

    public void PlayerWon()
    {
        StopAllCoroutines();
        Time.timeScale = 1f;

        currentIndex = (currentIndex + 1) % levelStates.Length; 
        level++;

        LoadLevel();
    }

    void GameOver()
    {
        Debug.Log("Game Over!");

        StopAllCoroutines();
        Time.timeScale = 0f;
        gameOverUI.Show();
    }
}