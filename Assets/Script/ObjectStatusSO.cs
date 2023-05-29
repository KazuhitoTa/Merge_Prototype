using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectStatusSO : ScriptableObject
{
    public List<ObjectStatus> ObjectStatusList=new List<ObjectStatus>();

    [System.Serializable]
    public class ObjectStatus
    {
        [SerializeField] GameObject objectModel;
        [SerializeField] string objectName;
        [SerializeField] int hp;


        public GameObject ObjectModel{get=>objectModel;}
        public string ObjectName{get=>objectName;}
        public int HP{get=>hp;}
       


    }
   
    
}
