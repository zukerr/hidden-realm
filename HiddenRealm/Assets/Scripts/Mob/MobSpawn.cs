using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MobSpawn : NetworkBehaviour
{
    [SerializeField]
    private MobGroup mobGroup = null;

    [SerializeField]
    private float rangeIndicator = 1f;

    private List<GameObject> spawnedMobs;

    [SyncVar]
    public int aliveMobs;

    private bool allDead = false;

    private Transform mobContainer;

    // Start is called before the first frame update
    void Start()
    {
        mobContainer = GameObject.Find("MobContainer").transform;
        SpawnMobs();
    }

    // Update is called once per frame
    void Update()
    {
        if(aliveMobs == 0)
        {
            if(!allDead)
            {
                allDead = true;
                Invoke("SpawnMobs", 15f);
                Invoke("DespawnMobs", 5f);
            }
        }
    }

    private Vector2 GetRandInCircle(float center_x, float center_y, float radius)
    {
        float rng = Random.Range(0f, 1f);
        float a = rng * 2 * Mathf.PI;
        float r = radius * Mathf.Sqrt(rng);
        float x = center_x + r * Mathf.Cos(a);
        float y = center_y + r * Mathf.Sin(a);

        return new Vector2(x, y);
    }

    
    private List<Vector2> GetCentres()
    {
        float x_inpt = transform.position.x;
        float y_inpt = transform.position.y;
        float x = (rangeIndicator * 2) / (2 + Mathf.Sqrt(3));
        float y = x / 2;
        float r = Mathf.Sqrt(3) * x / 2;
        List<Vector2> centres = new List<Vector2>();
        centres.Add(new Vector2(x_inpt, y_inpt + x));
        centres.Add(new Vector2(x_inpt + r, y_inpt - y));
        centres.Add(new Vector2(x_inpt - r, y_inpt - y));

        return centres;
    }
    

    public void SpawnMobs()
    {
        spawnedMobs = new List<GameObject>();
        aliveMobs = mobGroup.mobs.Count;
        if (mobGroup.mobs.Count == 3)
        {
            List<Vector2> centres = GetCentres();
            for (int i = 0; i < mobGroup.mobs.Count; i++)
            {
                Vector2 rngPosInCircle = GetRandInCircle(centres[i].x, centres[i].y, rangeIndicator);
                int randomAngle = Random.Range(0, 360);
                GameObject mobGo = Instantiate(mobGroup.mobs[i].prefab, rngPosInCircle, Quaternion.identity/*, mobContainer*/);
                mobGo.transform.GetChild(1).rotation = Quaternion.Euler(new Vector3(0, 0, randomAngle));
                mobGo.GetComponent<MobBehaviour>().parentSpawn = this;
                spawnedMobs.Add(mobGo);
                NetworkServer.Spawn(mobGo);
            }
        }
        else
        {
            for (int i = 0; i < mobGroup.mobs.Count; i++)
            {
                Vector2 rngPosInCircle = GetRandInCircle(transform.position.x, transform.position.y, rangeIndicator);
                int randomAngle = Random.Range(0, 360);
                GameObject mobGo = Instantiate(mobGroup.mobs[i].prefab, rngPosInCircle, Quaternion.identity/*, mobContainer*/);
                mobGo.transform.GetChild(1).rotation = Quaternion.Euler(new Vector3(0, 0, randomAngle));
                mobGo.GetComponent<MobBehaviour>().parentSpawn = this;
                spawnedMobs.Add(mobGo);
                NetworkServer.Spawn(mobGo);
            }
        }
        //adding teammates to spawned mobs
        SetupMobTeammates();
        allDead = false;
    }


    private void SetupMobTeammates()
    {
        for (int i = 0; i < mobGroup.mobs.Capacity; i++)
        {
            spawnedMobs[i].GetComponent<MobBehaviour>().InitTeammates();
            for (int j = 0; j < mobGroup.mobs.Capacity; j++)
            {
                if (j != i)
                {
                    spawnedMobs[i].GetComponent<MobBehaviour>().AddTeammate
                        (spawnedMobs[j].GetComponent<NetworkIdentity>().netId);
                    
                }
            }
        }
    }

    private void DespawnMobs()
    {
        foreach (GameObject mob in spawnedMobs)
        {
            NetworkServer.Destroy(mob);
        }
    }
}
