using System;
using System.Collections.Generic;

[Serializable]
public class TrafficResponse
{
    public Status current_status;
    public List<PredictedStatus> predicted_status = new List<PredictedStatus>();
}

[Serializable]
public class Status
{
    public float vehicleDensity; // 0.1 a 1.0
    public float averageSpeed;   // 0 a 100 km/h
    public string weather;       // sunny, clouded, foggy, light rain, heavy rain
}

[Serializable]
public class PredictedStatus
{
    public int estimated_time; // ms
    public Status predictions;
}