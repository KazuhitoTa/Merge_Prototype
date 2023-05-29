using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CreateMap : ScriptableObject
{
    public List<MapDate> mapDateList=new List<MapDate>();
    //public MapDate mapDate;
   
   [System.Serializable]
   public class MapDate
   {
       [SerializeField]int xMax;
       [SerializeField]int xMin;
       [SerializeField]int yMax;
       [SerializeField]int yMin;
       [SerializeField]List<Vector3> mapObjPos=new List<Vector3>();
       [SerializeField]List<int> mapGenre=new List<int>();
       [SerializeField]List<Vector3> enemyRootPos=new List<Vector3>();
       


       public List<Vector3> MapObjPos{get=>mapObjPos;}
       public List<int> MapGenre{get=>mapGenre;}
        public List<Vector3> EnemyRootPos{get=>enemyRootPos;}
       public int XMax{get=>xMax;}
       public int XMin{get=>xMin;}
       public int YMax{get=>yMax;}
       public int YMin{get=>yMin;}
   }

}
