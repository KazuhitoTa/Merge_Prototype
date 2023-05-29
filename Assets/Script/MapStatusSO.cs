using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MapStatusSO : ScriptableObject
{
    public List<MapStatus> mapStatusList=new List<MapStatus>();

    [System.Serializable]
    public class MapStatus
    {
        [SerializeField] MyTuple[] tuples;
        [SerializeField] Wave[] waves;

        public MyTuple[] Tuples{get=>tuples;}
        public Wave[] Waves{get=>waves;}

    }

    [System.Serializable]
    public class MyTuple
    {
        public Vector2 enemySpawn;
        public int enemySpawnNumber;
    }
    

    [System.Serializable]
    public class Wave
    {
        public MyTuple[] tuple;

        public MyTuple[] Tuple{get=>tuple;}
    }

   
    
}
