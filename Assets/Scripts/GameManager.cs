using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    public GameObject table, leftH, rightH, outerWall;
    public Enemy Drone, Golem, Skull, Boss;
    public Panel Normal, Long;
    public Transform toolSpawnBox;
    public Transform player;
    public Transform floor;
    public GameObject hitParticle;
    public List<GameObject> enemies;
    public Enemy closestGreenEnemy;
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
        toolSpawnBox.position = new Vector3(player.position.x, toolSpawnBox.position.y, player.position.z + 0.3f);
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
        enemies.Add(temp.gameObject);
        return temp;
    }

    public IEnumerator StageTextShow(string s, float f)
    {
        stageText.text = s;
        for (float t = 0; t < 0.5; t += Time.deltaTime)
        {
            stageText.color = new Color(1, 1, 1, t * 2);
            yield return null;
        }
        stageText.color = new Color(1, 1, 1, 1);

        if (f < 0) yield break;

        yield return new WaitForSeconds(f);

        for (float t = 0; t < 2; t += Time.deltaTime)
        {
            stageText.color = new Color(1, 1, 1, 1 - t / 2);
            yield return null;
        }
        stageText.color = new Color(1, 1, 1, 0);
    }
    public IEnumerator StageTextShow(string s)
    {
        stageText.text = s;
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
        List<PanelType> panelCand = new List<PanelType>();

        enableType.Add(VisionType.White);
        disableType.Add(VisionType.Red);
        disableType.Add(VisionType.Green);
        disableType.Add(VisionType.Blue);

        panelCand.Add(PanelType.Long);
        panelCand.Add(PanelType.Normal);
        panelCand.Add(PanelType.Normal);

        int stage = 0;
        float delay = initialDelay;
        float nextDelay = 0;
        int enemyNum = initialEnemy;
        float angle = initialAngle;

        //fortest

        //스테이지 0 안내
        StartCoroutine(StageTextShow("Stage 0"));
        yield return new WaitForSeconds(5f);

        while (true)
        {
            for(int i = 0; i < enemyNum; ++i)
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
            while (enemies.Count > 0) yield return null;
            ++stage;
            if (stage >= 5) break;

            enemyNum += enemyIncByStage;
            delay = Mathf.Max(initialDelay - delayDecByStage * stage, delayMin);
            angle = Mathf.Min(angle + angleIncByStage, angleMax);
            if(disableType.Count > 0)
            {
                VisionType newVision = disableType[Random.Range(0, disableType.Count)];
                disableType.Remove(newVision);
                enableType.Add(newVision);

                int pTmp = Random.Range(0, panelCand.Count);
                SpawnPanel(panelCand[pTmp], newVision);
                panelCand.RemoveAt(pTmp);
            }
            StartCoroutine(StageTextShow("Stage " + stage));
            yield return new WaitForSeconds(6f);
        }
        StartCoroutine(StageTextShow("Boss Stage"));
        yield return new WaitForSeconds(6f);
        enemies.Add(Instantiate(Boss).gameObject);
    }
    public void EnemyDead(GameObject g)
    {
        if (enemies.Contains(g)) enemies.Remove(g);
    }

    public void GameOver()
    {
        gameOver = true;
        StopCoroutine(playCoroutine);
        foreach (GameObject g in enemies) g.GetComponent<Enemy>().GameOver();
    }

    IEnumerator Test()
    {
        SpawnEnemy(EnemyType.Golem, VisionType.Red, 22, 0);
        SpawnEnemy(EnemyType.Golem, VisionType.White, 22, 10);
        SpawnEnemy(EnemyType.Golem, VisionType.Blue, 22, 20);
        SpawnEnemy(EnemyType.Golem, VisionType.Green, 22, -20);
        SpawnPanel(PanelType.Normal, VisionType.Red);
        SpawnPanel(PanelType.Normal, VisionType.Blue);
        SpawnPanel(PanelType.Normal, VisionType.Green);
        yield return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<GameObject>();
        table = GameObject.Find("Table");
        leftH = GameObject.Find("Controller (left)");
        rightH = GameObject.Find("Controller (right)");
        outerWall = GameObject.Find("OuterWall");
        playCoroutine = StartCoroutine(Stage());
        //playCoroutine = StartCoroutine(Test());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
