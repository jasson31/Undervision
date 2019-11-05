﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy Drone, Golem, Skull;
    public Panel Normal, Long;
    public Transform toolSpawnBox;
    public Transform player;
    public List<GameObject> enemys;
    public static float enemySpawnDist = 22;
    public bool gameOver;

    public float initialDelay;
    public float delayDec;
    public float delayMin;
    public float delayDecByStage;
    public int initialEnemy;
    public int enemyIncByStage;
    public float initialAngle;
    public float angleIncByStage;
    public float angleMax;
    public TextMesh stageText;

    Coroutine playCoroutine;
    private Panel SpawnPanel(PanelType _panelType, VisionType _visionType)
    {
        Panel temp = null;
        switch (_panelType)
        {
            case PanelType.Normal:
                temp = Instantiate(Normal);
                break;
            case PanelType.Long:
                temp = Instantiate(Long);
                break;
            default:
                Debug.LogError("Invalid Enemy Type of " + _panelType);
                return null;
        }
        temp.transform.position = new Vector3(
            Random.Range(toolSpawnBox.position.x - toolSpawnBox.localScale.x / 2, toolSpawnBox.position.x + toolSpawnBox.localScale.x / 2),
            Random.Range(toolSpawnBox.position.y - toolSpawnBox.localScale.y / 2, toolSpawnBox.position.y + toolSpawnBox.localScale.y / 2),
            Random.Range(toolSpawnBox.position.z - toolSpawnBox.localScale.z / 2, toolSpawnBox.position.z + toolSpawnBox.localScale.z / 2));
        temp.ChangeColor(_visionType);
        return temp;
    }

    private Enemy SpawnEnemy(EnemyType _enemyType, VisionType _visionType, float dist, float degrees)
    {
        Enemy temp = null;
        switch (_enemyType)
        {
            case EnemyType.Skull:
                temp = Instantiate(Skull);
                break;
            case EnemyType.Drone:
                temp = Instantiate(Drone);
                break;
            case EnemyType.Golem:
                temp = Instantiate(Golem);
                break;
            default:
                Debug.LogError("Invalid Enemy Type of " + _enemyType);
                return null;
        }
        float rad = degrees * Mathf.PI / 180f;
        temp.transform.position = new Vector3(Mathf.Sin(rad) * dist, _enemyType == EnemyType.Drone ? 5 : 0, Mathf.Cos(rad) * dist);
        temp.ChangeColor(_visionType);
        temp.player = player;
        temp.spawner = this;
        enemys.Add(temp.gameObject);
        return temp;
    }
    IEnumerator Temp()
    {
        SpawnEnemy(EnemyType.Skull, VisionType.Red, enemySpawnDist, 0);

        yield return new WaitForSeconds(0.5f);
        SpawnEnemy(EnemyType.Golem, VisionType.Red, enemySpawnDist, -12);

        yield return new WaitForSeconds(0.5f);
        SpawnEnemy(EnemyType.Drone, VisionType.White, enemySpawnDist, 12);
    }
    IEnumerator StageTextShow(int s)
    {
        stageText.text = "Stage " + s;
        for(float t = 0; t < 0.5; t += Time.deltaTime)
        {
            stageText.color = new Color(1, 1, 1, t * 2);
            yield return null;
        }
        stageText.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(2f);

        for (float t = 0; t < 2; t += Time.deltaTime)
        {
            stageText.color = new Color(1, 1, 1, 1 - t / 2);
            yield return null;
        }
        stageText.color = new Color(1, 1, 1, 0);
    }
    IEnumerator Stage()
    {
        List<VisionType> enableType = new List<VisionType>();
        List<VisionType> disableType = new List<VisionType>();

        enableType.Add(VisionType.White);
        disableType.Add(VisionType.Red);
        disableType.Add(VisionType.Green);
        disableType.Add(VisionType.Blue);

        int stage = 0;
        float delay = initialDelay;
        float nextDelay = 0;
        int enemyNum = initialEnemy;
        float angle = initialAngle;

        //스테이지 0 안내
        StartCoroutine(StageTextShow(0));
        yield return new WaitForSeconds(5f);

        while (true)
        {
            for(int i = 0; i < initialEnemy; ++i)
            {
                SpawnEnemy((EnemyType)Random.Range(0,3), enableType[Random.Range(0, enableType.Count)], enemySpawnDist, Random.Range(-angle, angle));
                if(nextDelay >= delay)
                {
                    yield return new WaitForSeconds(delay + nextDelay);
                    nextDelay = 0;
                }
                else
                {
                    float tDelay = Random.Range(0, delay + nextDelay);
                    nextDelay = delay + nextDelay - tDelay;
                    delay = Mathf.Max(delay - delayDec, delayMin);
                    yield return new WaitForSeconds(tDelay);
                }
            }
            while (enemys.Count > 0) yield return null;
            ++stage;
            enemyNum += enemyIncByStage;
            delay = Mathf.Max(initialDelay - delayDecByStage * stage, delayMin);
            angle = Mathf.Min(angle + angleIncByStage, angleMax);
            if(disableType.Count > 0)
            {
                VisionType newVision = disableType[Random.Range(0, disableType.Count)];
                disableType.Remove(newVision);
                enableType.Add(newVision);
                SpawnPanel(PanelType.Normal, newVision);
            }
            StartCoroutine(StageTextShow(stage));
            yield return new WaitForSeconds(6f);
        }
    }
    public void EnemyDead(GameObject g)
    {
        if (enemys.Contains(g)) enemys.Remove(g);
    }

    public void GameOver()
    {
        gameOver = true;
        StopCoroutine(playCoroutine);
        foreach (GameObject g in enemys) g.GetComponent<Enemy>().GameOver();
    }

    // Start is called before the first frame update
    void Start()
    {
        enemys = new List<GameObject>();
        playCoroutine = StartCoroutine(Stage());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
