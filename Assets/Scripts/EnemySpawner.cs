using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy Drone, Golem, Skull;



    private Enemy SpawnEnemy(EnemyType _enemyType, VisionType _visionType, Vector3 pos)
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
                break;
        }
        temp.transform.position = pos;
        temp.ChangeColor(_visionType);
        return temp;
    }


    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemy(EnemyType.Drone, VisionType.White, new Vector3(-1.76f, 0.61f, 23.35f));
        SpawnEnemy(EnemyType.Golem, VisionType.Green, new Vector3(14.12f, 0, 17.51f));
        SpawnEnemy(EnemyType.Golem, VisionType.Blue, new Vector3(14.8f, 0, 17.51f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
