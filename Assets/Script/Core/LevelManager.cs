using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject[] allOrcs;
    [SerializeField] Light2D globalLight;
    [Header("Orc Kill Goal")]
    [SerializeField] int totalOrcs;
    [SerializeField] TextMeshProUGUI orcRemainingText;

    [Header("Spawn Info")]
    [SerializeField] int maximumOrcOnScreen = 10;
    [SerializeField] List<Transform> allSpawnPoints = new List<Transform>();
    [SerializeField] int waitBeginTime;
    [SerializeField] float spawnInterval;
    [SerializeField] TextMeshProUGUI waitText;


    GameOverManager gameOverManager;
    List<GameObject> allCurrentOrcs = new List<GameObject>();
    float spawnTimer;
    float baseGlobalLightIntensity;
    int orcRemaining;
    int orcSpawnLeft;
    bool startSpawning;
    bool alreadySpawnBoss;

    private void Awake()
    {
        waitText.text = $"You have {waitBeginTime} seconds to prepare!";
        orcRemaining = totalOrcs;
        orcSpawnLeft = totalOrcs;
        orcRemainingText.text = orcRemaining.ToString();
        baseGlobalLightIntensity = globalLight.intensity;
    }

    void Start()
    {
        StartCoroutine(WaitThenStart(waitBeginTime));
        gameOverManager = GameObject.FindGameObjectWithTag("GameOverManager").GetComponent<GameOverManager>();
    }

    void Update()
    {
        handleOrcRemainingText();
        if (orcRemaining == 0)
        {
            gameOverManager.EndGame("Win");
            return;
        }else if(orcRemaining == 1 && !alreadySpawnBoss)
        {
            StartCoroutine(WaitThenSpawnBoss(4));
            alreadySpawnBoss = true;
        }

        spawnTimer -= Time.deltaTime;

        orcSpawner();
        checkIfOrcDie();
    }

    private void handleOrcRemainingText()
    {
        orcRemainingText.text = orcRemaining.ToString();

        float percentage = (float)orcRemaining / totalOrcs;

        Color lightRed = new Color(1f, 0.5f, 0.5f);
        Color lightGreen = new Color(0.5f, 1f, 0.5f);

        orcRemainingText.color = Color.Lerp(lightGreen, lightRed, percentage);
    }
    void orcSpawner()
    {
        if (!startSpawning) return;

        if (spawnTimer <= 0 && allCurrentOrcs.Count < maximumOrcOnScreen && orcSpawnLeft > 0)
        {
            spawnTimer = spawnInterval;
            globalLight.intensity = baseGlobalLightIntensity * (float)(orcRemaining) / totalOrcs;

            if (orcSpawnLeft == 1) return;
            spawnWhichOrc(0);
        }
    }

    void spawnWhichOrc(int orcIndex)
    {
        if (allOrcs[orcIndex] == null) return;

        Vector2 spawnPos = allSpawnPoints[Random.Range(0, allSpawnPoints.Count)].position;
        GameObject newOrc = Instantiate(allOrcs[orcIndex], spawnPos, Quaternion.identity);
        allCurrentOrcs.Add(newOrc);
        orcSpawnLeft--;
    }

    void checkIfOrcDie()
    {
        for (int i = allCurrentOrcs.Count - 1; i >= 0; i--)
        {
            if (allCurrentOrcs[i] == null)
            {
                allCurrentOrcs.RemoveAt(i);
                orcRemaining--;
            }
        }
    }
    IEnumerator WaitThenSpawnBoss(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        spawnWhichOrc(1);
    }

    IEnumerator WaitThenStart(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        startSpawning = true;
        waitText.gameObject.SetActive(false);
    }

}
