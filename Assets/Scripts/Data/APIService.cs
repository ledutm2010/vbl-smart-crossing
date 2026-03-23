using UnityEngine;
using System.IO;

public class APIService : MonoBehaviour
{
    public string jsonFileName = "traffic_data.json"; 

    public TrafficResponse GetTrafficData()
    {
        string path = Path.Combine(Application.streamingAssetsPath, jsonFileName);

        if (!File.Exists(path))
        {
            Debug.LogError("Arquivo JSON não encontrado: " + path);
            return null;
        }

        string json = File.ReadAllText(path);

        TrafficResponse data = JsonUtility.FromJson<TrafficResponse>(json);

        if (data == null)
        {
            Debug.LogError("Erro ao desserializar JSON.");
        }

        return data;
    }
}