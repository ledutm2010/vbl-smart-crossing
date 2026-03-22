using UnityEngine;

public class TrafficSpawner : MonoBehaviour
{
    [Header("Prefab e SpawnPoints")]
    public GameObject vehiclePrefab;
    public Transform[] spawnPoints; // Arraste SpawnPoint1,2,3 aqui no Inspector

    [Header("Configurações internas")]
    private float spawnInterval = 1f;
    private float vehicleSpeed = 5f;

    private bool spawning = false;

    /// Configura o tráfego com os dados da API
    public void SetTraffic(Status status, float referenceSpeed = 10f)
    {
        if (status == null) return;

        // Calcula intervalo de spawn baseado na densidade da via
        spawnInterval = 1f / Mathf.Clamp(status.vehicleDensity, 0.1f, 1f);

        // Calcula velocidade dos veículos proporcional à média da API
        vehicleSpeed = (status.averageSpeed / 100f) * referenceSpeed;

        // Reinicia o spawn com novo intervalo
        CancelInvoke(nameof(SpawnVehicle));
        InvokeRepeating(nameof(SpawnVehicle), 0f, spawnInterval);
        spawning = true;

        UpdateActiveVehiclesSpeed();
    }

    /// Instancia um veículo em um SpawnPoint aleatório
    void SpawnVehicle()
    {
        if (spawnPoints == null || spawnPoints.Length == 0 || vehiclePrefab == null) return;

        int index = Random.Range(0, spawnPoints.Length);
        Transform spawn = spawnPoints[index];

        GameObject vehicle = Instantiate(vehiclePrefab, spawn.position, spawn.rotation);

        Vehicle vehicleScript = vehicle.GetComponent<Vehicle>();
        if (vehicleScript == null)
        {
            vehicleScript = vehicle.AddComponent<Vehicle>();
        }

        // Direção por faixa
        float direction = 1f;

        if (index == 1) // faixa do meio
        {
            direction = -1f;
        }

        vehicleScript.speed = vehicleSpeed * direction;
    }

    //Ajusta a velocidade dos veículos já existentes
    public void UpdateActiveVehiclesSpeed()
    {
        Vehicle[] vehicles = FindObjectsOfType<Vehicle>();

        foreach (var v in vehicles)
        {
            float direction = Mathf.Sign(v.speed); // mantém direção atual
            v.speed = vehicleSpeed * direction;
        }
    }
}