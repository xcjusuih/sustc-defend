using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public GameObject[] enemyPrefab;
    //length of enemyPrefab
    public int limit;
    //防止开场出大怪
    public int current;
    //一轮出几个
    public int count;
    //有多少轮
    public int round;
    public float interval;
}
