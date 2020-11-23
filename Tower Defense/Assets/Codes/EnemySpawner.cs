using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Wave wave;
    public Transform SPAWN_POINT;
    public GameObject alive;
    float rate;
    // Start is called before the first frame update
    void Start(){
        StartCoroutine(spawnEnemy());
    }
    IEnumerator spawnEnemy(){
        if(alive){
            int cnt;
            for (int round=0;round<wave.round;round++)
            {
                cnt = 0;
                while(cnt<wave.count){
                    for (int i=0;i<wave.limit;i++)
                    {
                        rate = wave.enemyPrefab[i].GetComponent<RobotEnemy>().rate;
                        float tmp = Random.Range(0.0f,1.0f);
                        if(tmp<rate){
                            var enemy = GameObject.Instantiate(wave.enemyPrefab[i],SPAWN_POINT.position,Quaternion.identity);
                            enemy.GetComponent<RobotEnemy>().target = alive;
                            Global.enemy_count++;
                            cnt++;
                            yield return new WaitForSeconds(0.5f);
                        }
                    }
                }
                yield return new WaitForSeconds(wave.interval);
            }
        }
    }
}
