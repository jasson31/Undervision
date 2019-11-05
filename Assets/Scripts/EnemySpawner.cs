using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy Drone, Golem, Skull;
    public Transform player;
    public List<GameObject> enemys;
    public static float enemySpawnDist = 22;

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
        SpawnEnemy(EnemyType.Skull, VisionType.Blue, enemySpawnDist, 0);

        yield return new WaitForSeconds(0.5f);
        SpawnEnemy(EnemyType.Golem, VisionType.Red, enemySpawnDist, -12);

        yield return new WaitForSeconds(0.5f);
        SpawnEnemy(EnemyType.Drone, VisionType.White, enemySpawnDist, 12);
    }
    public void EnemyDead(GameObject g)
    {
        if (enemys.Contains(g)) enemys.Remove(g);
    }

    public void GameOver()
    {
        foreach (GameObject g in enemys) g.GetComponent<Enemy>().GameOver();
    }

    // Start is called before the first frame update
    void Start()
    {
        enemys = new List<GameObject>();
        StartCoroutine(Temp());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
