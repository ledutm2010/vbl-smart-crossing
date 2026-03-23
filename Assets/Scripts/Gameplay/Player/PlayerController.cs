using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float baseSpeed = 5f;
    private string currentWeather = "sunny";

    private Vector2 moveInput;

    void Update()
    {
        // Captura input do teclado
        moveInput = Vector2.zero;

        if (Keyboard.current.leftArrowKey.isPressed) moveInput.x = -1;
        if (Keyboard.current.rightArrowKey.isPressed) moveInput.x = 1;
        if (Keyboard.current.upArrowKey.isPressed) moveInput.y = 1;
        if (Keyboard.current.downArrowKey.isPressed) moveInput.y = -1;

        // Multiplicador de velocidade por clima
        float weatherMultiplier = currentWeather switch
        {
            "sunny" => 1f,
            "clouded" or "foggy" => 0.8f,
            "light rain" => 0.6f,
            "heavy rain" => 0.4f,
            _ => 1f
        };

        float speed = baseSpeed * weatherMultiplier;

        // Movimento no plano XZ
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    // Atualiza o clima
    public void SetWeather(string weather)
    {
        currentWeather = weather;
    }
}