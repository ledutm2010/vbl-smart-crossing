using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI weatherText;
    public TextMeshProUGUI densityText;
    public TextMeshProUGUI timerText;

    public void UpdateHUD(int level, Status status, float timeLeft)
    {
        if (levelText != null)
            levelText.text = $"Level: {level}";

        if (weatherText != null)
            weatherText.text = status != null ? $"Weather: {status.weather}" : "Weather: -";

        if (densityText != null)
            densityText.text = status != null ? $"Density: {status.vehicleDensity:F2}" : "Density: -";

        if (timerText != null)
            timerText.text = $"Time Left: {timeLeft:F1}s";
    }
}