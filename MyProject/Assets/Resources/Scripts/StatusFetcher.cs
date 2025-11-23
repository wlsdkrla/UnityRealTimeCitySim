using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WeatherData {
    public float avg_temp;
    public float avg_wind;
    public float max_wind;
}

[System.Serializable]
public class TrafficData {
    public string link_id;
    public float speed;
    public float travel_time;
}

[System.Serializable]
public class StatusResponse {
    public WeatherData weather;
    public List<TrafficData> traffic;
}

[System.Serializable]
public class RoadLink {
    public string link_id;
    public Transform avenue;

    public List<Transform> spawnPoints;   
    public List<GameObject> carPrefabs;   
    [HideInInspector] public float spawnInterval = 1f;
    [HideInInspector] public bool isSpawning = false;
}

public class StatusFetcher : MonoBehaviour {
    public Material skyboxMaterial;
    public List<RoadLink> roadLinks;   

    void Start() {
        StartCoroutine(FetchStatus());
    }
    float GetSpawnInterval(float speed)
    {
        if (speed >= 40) return Random.Range(10f, 13f);  
        if (speed >= 20) return Random.Range(5f, 10f); 
        return Random.Range(3f, 5f);                  
    }
    bool IsSpawnAreaClear(Vector3 pos, float radius)
    {
        Collider[] hits = Physics.OverlapSphere(pos, radius);
        return hits.Length == 0;
    }
    IEnumerator SpawnLoop(RoadLink roadLink)
    {
        roadLink.isSpawning = true;

        while (true)
        {
        
            Transform spawn = roadLink.spawnPoints[Random.Range(0, roadLink.spawnPoints.Count)];
            GameObject prefab = roadLink.carPrefabs[Random.Range(0, roadLink.carPrefabs.Count)];

            Vector3 pos;
            int safetyTry = 0;


            do
            {
                pos = spawn.position + spawn.forward * Random.Range(0f, 20f);
                safetyTry++;
            }
            while (!IsSpawnAreaClear(pos, 2.5f) && safetyTry < 10);

     
            GameObject car = Instantiate(prefab, pos, spawn.rotation);
            CarMover mover = car.GetComponent<CarMover>();
            mover.speed = Random.Range(3f, 12f);

       
            yield return new WaitForSeconds(roadLink.spawnInterval);
        }
    }

    IEnumerator FetchStatus() {
        string url = "http://127.0.0.1:8000/status";
        UnityWebRequest req = UnityWebRequest.Get(url);

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success) {
            Debug.LogError("API 요청 실패: " + req.error);
        } else {
            StatusResponse data = JsonUtility.FromJson<StatusResponse>(req.downloadHandler.text);
            if (data != null) {
                UpdateSkybox(data.weather);
                UpdateRoadColors(data.traffic);
                UpdateCars(data.traffic);
            }
        }
    }
    void UpdateCars(List<TrafficData> traffic)
    {
        foreach (var roadLink in roadLinks)
        {
            TrafficData t = traffic.Find(x => x.link_id == roadLink.link_id);
            if (t == null) continue;

        
            roadLink.spawnInterval = GetSpawnInterval(t.speed);

        
            if (!roadLink.isSpawning)
                StartCoroutine(SpawnLoop(roadLink));
        }
    }

    void UpdateSkybox(WeatherData weather) {
        float temp = weather.avg_temp;
        Color color;

        if (temp < 10) color = Color.cyan;
        else if (temp < 25) color = Color.blue;
        else color = Color.red;

        skyboxMaterial.SetColor("_Tint", color);
    }

    void UpdateRoadColors(List<TrafficData> traffic) {
        foreach (var roadLink in roadLinks) {
            
            TrafficData t = traffic.Find(x => x.link_id == roadLink.link_id);
            if (t == null) {
                Debug.LogWarning($"{roadLink.link_id} 데이터 없음");
                continue;
            }

            
            Color color;
            if (t.speed >= 40) color = Color.green;
            else if (t.speed >= 20) color = Color.yellow;
            else color = Color.red;

            
            Transform road = roadLink.avenue.Find("Road");
            if (road != null) {
                Renderer[] renderers = road.GetComponentsInChildren<Renderer>();
                foreach (Renderer rend in renderers) {
                    rend.material.color = color;
                }
                Debug.Log($"{roadLink.avenue.name} ({roadLink.link_id}) 세그먼트 {renderers.Length}개 → {color}");
            } else {
                Debug.LogWarning($"{roadLink.avenue.name}: Road 오브젝트 없음");
            }
        }
    }
}
