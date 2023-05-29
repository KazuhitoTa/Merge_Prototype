using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu]
public class EnemyStatusSO : ScriptableObject
{
    public List<EnemyStatus> enemyStatusList=new List<EnemyStatus>();

    [System.Serializable]
    public class EnemyStatus
    {
        [SerializeField] GameObject enemyModel;
        [SerializeField] string enemyName;
        [SerializeField] int hp;
        [SerializeField] int attackSpeed;
        [SerializeField] int attack;
        [SerializeField] int speed;
        public GameObject EnemyModel{get=>enemyModel;}
        public string EnemyName{get=>enemyName;}
        public int HP{get=>hp;}
        public int AttackSpeed{get=>attackSpeed;}
        public int Attack{get=>attack;}
        public int Speed{get=>speed;}

    }
}
