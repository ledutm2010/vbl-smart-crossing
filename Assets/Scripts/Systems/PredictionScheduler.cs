using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PredictionScheduler : MonoBehaviour
{
    public PlayerController player;
    public TrafficSpawner spawner;

    public void SchedulePredictions(List<PredictedStatus> predictions)
    {
        StopAllCoroutines();

        if (predictions == null) return;

        foreach (var pred in predictions)
        {
            if (pred != null && pred.predictions != null)
            {
                StartCoroutine(ApplyPredictionAfterDelay(pred));
            }
        }
    }

    private IEnumerator ApplyPredictionAfterDelay(PredictedStatus pred)
    {
        yield return new WaitForSeconds(pred.estimated_time / 1000f);

        // Atualiza clima e tráfego
        player.SetWeather(pred.predictions.weather);
        spawner.SetTraffic(pred.predictions);
    }
}