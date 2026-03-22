using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Destrói veículo se sair da tela
        if (transform.position.x > 20f)
        {
            Destroy(gameObject);
        }
    }
}