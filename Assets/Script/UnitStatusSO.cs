using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UnitStatusSO : ScriptableObject
{
    public List<UnitStatus> unitStatusList=new List<UnitStatus>();

    [System.Serializable]
    public class UnitStatus
    {
        [SerializeField] GameObject unitModel;
        [SerializeField] GameObject bulletModel;

        [SerializeField] string unitName;
        [SerializeField] int hp;
        [SerializeField] float attackSpeed;
        [SerializeField] int attack;
        [SerializeField] int unitNumber;


        public GameObject UnitModel{get=>unitModel;}
        public GameObject BulletModel{get=>bulletModel;}
        public string UnitName{get=>unitName;}
        public int HP{get=>hp;}
        public float AttackSpeed{get=>attackSpeed;}
        public int Attack{get=>attack;}
        public int UnitNumber{get=>unitNumber;}


    }
   
    
}
