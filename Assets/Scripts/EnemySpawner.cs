using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy Drone, Golem, Skull;
    public Transform player;
    public float enemySpawnDist = 22;

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
        temp.transform.position = new Vector3(Mathf.Sin(rad) * dist, 0, Mathf.Cos(rad) * dist);
        temp.ChangeColor(_visionType);
        temp.player = player;
        return temp;
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemy(EnemyType.Skull, VisionType.Blue, enemySpawnDist, 0);
        SpawnEnemy(EnemyType.Golem, VisionType.Red, enemySpawnDist, -12);
        SpawnEnemy(EnemyType.Skull, VisionType.White, enemySpawnDist, 12);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
