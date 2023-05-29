using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;


public class MapManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;//床のプレハブ
    private bool canUnitPut;//置けるかどうかの確認用
    private List<List<GameObject>> mapTile = new List<List<GameObject>>();//マップに置かれている床を管理
    private List<List<int>> mapDate = new List<List<int>>();//床がない場所には999キャラが何も置かれていない場所には0キャラが置かれている場合はその識別番号を入れる
    private List<GameObject> evolutionUnitsTemp = new List<GameObject>();//進化するために置かれたユニットと同じmapDateのunitを一時的に保持
    private int unitNumber=1;//キャラクターを表す番号(仮)
    [SerializeField]UnitManager unitManager;//StageオブジェクトにつけられているunitManagerを継承
    private List<int> nextUnitList=new List<int>();//次に置けるunitの番号を保持
    private List<int> nextUnitList2=new List<int>();//次に置けるunitの番号を保持

    private bool syringeCheck;//スポイトが使えるかどうかを確認
    private int syringeCoolDown=300;//スポイトのクールタイム
    [SerializeField]int nowUnitCost=6;//現在のunitを置くためのコスト
    private int gameTime=0;//経過時間
    [SerializeField]int maxUnitCost=6;//コストの上限
    private int unitChoice;//どちらのunit欄をクリックしているか確認
    [SerializeField]Image[] unitImg;
    [SerializeField]Color[] unitColor;
    //テスト
    [SerializeField]CreateMap createMapTest; 
    [SerializeField]GameObject tstObj;
    [SerializeField] List<GameObject> towerPrefabs=new List<GameObject>();
    [SerializeField]int stageNumber=0; 

    [SerializeField]List<Tower> towers=new List<Tower>();
    private int towerCount=0;
    [SerializeField]TextMeshProUGUI gameText;
    bool tower1;
    bool tower2;
    [SerializeField]GameObject[] notUseImg; 


    
    /// <summary>
    /// Start
    /// </summary>
    public void Init()
    {
        MapCreate();
        UnitInit();
        TowerPut();
        TowerPosNotPut();
    }

    /// <summary>
    /// Update
    /// </summary>
    public void ManagedUpdate()
    {
        GameTime();
        UnitPut();
        TowerUpdate();
    }

    void MapCreate()
    {
        int x=Mathf.Abs(createMapTest.mapDateList[stageNumber].XMax-createMapTest.mapDateList[stageNumber].XMin);
        int y=Mathf.Abs(createMapTest.mapDateList[stageNumber].YMax-createMapTest.mapDateList[stageNumber].YMin);

        List<List<Vector3>> mapObjPosTest=new List<List<Vector3>>();

        List<int> mapDateTemp=createMapTest.mapDateList[stageNumber].MapGenre;
        List<Vector3> mapObjPosTemp=createMapTest.mapDateList[stageNumber].MapObjPos;

        for (int i = 0; i < x; i++) // 行数
        {
            mapDate.Add(new List<int>());
            mapTile.Add(new List<GameObject>());
            mapObjPosTest.Add(new List<Vector3>());

            for (int j = 0; j < y; j++) // 列数
            {   
                mapTile[i].Add(null);
                mapDate[i].Add(0); 
                mapObjPosTest[i].Add(new Vector3(0,0,0));
            }
        }

        
        mapDate=ConvertToTwoDimensionalList<int>(mapDateTemp,x,y);
        mapObjPosTest=ConvertToTwoDimensionalList<Vector3>(mapObjPosTemp,x,y);
        
        for(int a=0;a<mapDate.Count;a++)
        {
            for(int b=0;b<mapDate[a].Count;b++)
            {
                var tempNumber=mapDate[a][b];
                var tempObjPosTest=mapObjPosTest[a][b];
    
                if(tempNumber==0)mapTile[a][b]=Instantiate(tstObj,tempObjPosTest,Quaternion.identity);
                //else mapTile[a][b]=Instantiate(tstObj2,tempObjPosTest,Quaternion.identity);
            }
        }
        
    }

    /// <summary>
    /// 初めのunitの設定
    /// </summary>
    void UnitInit()
    {
        unitNumber =6;//Random.Range(6,11); 
        nextUnitList2.Insert(0, unitNumber - 1);
        NextCharacter();
    
        unitNumber = 1;//Random.Range(1,6);
        nextUnitList.Insert(0, unitNumber - 1);
        NextCharacter();
    }
    
    /// <summary>
    /// ゲームプレイ中の時間を管理
    /// </summary>
    int GameTime()
    {
        if(nowUnitCost<maxUnitCost&&gameTime%300==0)nowUnitCost++;
        gameTime++;
        syringeCoolDown++;
        return syringeCoolDown;
    }


    /// <summary>
    /// unitを置く処理をまとめる
    /// </summary>
    void UnitPut()
    {
        if (Input.GetMouseButtonDown(0)&&!syringeCheck)
        {
            GameObject temp=null;
            GameObject clickObj=GetClickObj();
            if(clickObj!=null)
            {
                PlacementVerifier(clickObj);
                if(canUnitPut)
                {
                    if(unitChoice==0)
                    {
                        temp=unitManager.CreateUnit(unitNumber-1,clickObj.transform.position).gameObject;
                        temp.transform.parent=clickObj.gameObject.transform;
                        unitNumber=1;//Random.Range(1,4);
                        nextUnitList.Insert(0,unitNumber-1);
                    }
                    else
                    {
                        temp=unitManager.CreateUnit(unitNumber-1,clickObj.transform.position).gameObject;
                        temp.transform.parent=clickObj.gameObject.transform;
                        unitNumber=6;//Random.Range(4,7);
                        nextUnitList2.Insert(0,unitNumber-1);
                    } 
                    
                    canUnitPut=false;
                    nowUnitCost--;
                }
                NextCharacter();
            }
            
        }
        else if(Input.GetMouseButtonDown(0)&&syringeCheck)
        {
            GameObject clickObj=GetClickObj();
            //クリックしたオブジェクトの子オブジェクトがあるか確認
            if(clickObj.transform.childCount > 0)
            {
                GameObject temp=clickObj.transform.GetChild(0).gameObject;
                Unit tep=temp.GetComponent<Unit>();
                //クリックした位置のmapDateを０に
                for (int i = 0; i <mapDate.Count; i++)
                {
                    for (int j = 0; j <mapDate[i].Count; j++)
                    {
                        if(clickObj==mapTile[i][j])
                        {
                            unitNumber=mapDate[i][j];
                            mapDate[i][j]=0;
                        }
                    }
                };
                if(unitNumber<6)nextUnitList.Insert(0,unitNumber-1);
                else nextUnitList2.Insert(0,unitNumber-1);
                NextCharacter();
                unitManager.RemoveUnit(tep);
                if(nowUnitCost<maxUnitCost)nowUnitCost++;
                syringeCheck=false;
            }
            
        }
        
    }
    
    /// <summary>
    /// 次のunitを表示
    /// </summary>

    void NextCharacter()
    {
        if (unitNumber >= 1 && unitNumber <= 5)
        {
            unitImg[0].color = unitColor[unitNumber - 1];
        }
        else if (unitNumber >= 6 && unitNumber <= 10)
        {
            unitImg[1].color = unitColor[unitNumber - 6];
        }
    }

    /// <summary>
    /// クリックしたオブジェクトをclickedGameObjectに入れる
    /// </summary>
    GameObject GetClickObj()
    {
        GameObject clickedObject = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
    
        if (hit.collider != null) 
        {
            clickedObject = hit.collider.gameObject;
        }
        for(int i=0;i<mapTile.Count;i++)
        {
            for(int j=0;j<mapTile[i].Count;j++)
            {
                if(mapTile[i][j]==clickedObject)
                {
                    //Debug.Log(i+" "+j);
                    //Debug.Log(clickedObject);
                    //Debug.Log(mapDate[i][j]);
                }
                
                
            }
        }
        
        return clickedObject;
        
    }


    /// <summary>
    /// unitが置けるかどうかと進化するかどうかを確認
    /// </summary>
    void PlacementVerifier(GameObject clickObj)
    {
        for (int i = 0; i <mapDate.Count; i++)
        {
            for (int j = 0; j <mapDate[i].Count; j++)
            {   
                if(mapDate[i][j]==0&&mapTile[i][j]==clickObj&&nowUnitCost>0)
                { 
                    canUnitPut=true;
                    mapDate[i][j]=unitNumber;
                    for (int k = 0; k < 5; k++)
                    {
                        UnitEvolutionCheck(i, j);
                    }  
                    return;
                }
            }
        }
            
    }

    /// <summary>
    /// ///指定された座標の周囲のユニットを調べ、同じ色のユニットが2つ以上あれば進化する処理を行う関数
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>

    void UnitEvolutionCheck(int x, int y)
    {
        // 最大進化までしていたら処理をしない
        if (unitNumber % 5 == 0)return;

        List<(int, int)> evolutionState = new List<(int, int)>();
        List<GameObject> objectTemp = new List<GameObject>();
        objectTemp.Add(mapTile[x][y]);

        int mapSizeX=Mathf.Abs(createMapTest.mapDateList[stageNumber].XMax-createMapTest.mapDateList[stageNumber].XMin);
        int mapSizeY=Mathf.Abs(createMapTest.mapDateList[stageNumber].YMax-createMapTest.mapDateList[stageNumber].YMin);

        for (int a = -1; a < 2; a++)
        {
            for (int b = -1; b < 2; b++)
            {
                if (a == 0 && b == 0)continue;

                int nowNumber1 = x - a;
                int nowNumber2 = y - b;

                if (IsCenterCellOrExtraCell(y, a, b) || !IsWithinMapBounds(nowNumber1, nowNumber2))continue;

                if (mapDate[x][y] == mapDate[nowNumber1][nowNumber2])
                {
                    evolutionState.Add((nowNumber1, nowNumber2));
                    objectTemp.Add(mapTile[nowNumber1][nowNumber2]);
                }
            }
        }

        for (int i = 0; i < evolutionState.Count; i++)
        {
            for (int a = -1; a < 2; a++)
            {
                for (int b = -1; b < 2; b++)
                {
                    if (a == 0 && b == 0)
                        continue;

                    int nowNumber1 = evolutionState[i].Item1 - a;
                    int nowNumber2 = evolutionState[i].Item2 - b;

                    if (IsCenterCellOrExtraCell(evolutionState[i].Item2, a, b) || !IsWithinMapBounds(nowNumber1, nowNumber2))continue;

                    if (mapDate[x][y] == mapDate[nowNumber1][nowNumber2] && !objectTemp.Contains(mapTile[nowNumber1][nowNumber2]))
                    {
                        evolutionState.Add((nowNumber1, nowNumber2));
                        objectTemp.Add(mapTile[nowNumber1][nowNumber2]);
                    }
                }
            }
        }

        foreach (var item in evolutionState)
        {
            evolutionUnitsTemp.Add(mapTile[item.Item1][item.Item2]);
        }

        if (evolutionUnitsTemp.Count > 1)
        {
            UnitEvolution(evolutionState, (x, y));
            unitNumber++;
        }

        evolutionUnitsTemp.Clear();
        mapDate[x][y] = unitNumber;
    }

     bool IsCenterCellOrExtraCell(int y, int a, int b)
    {
        if ((a == -1 && b == 1) || (a == -1 && b == -1))return y % 2 == 0;
        else if ((a == 1 && b == 1) || (a == 1 && b == -1))return y % 2 == 1;
        return false;
    }

    bool IsWithinMapBounds(int x, int y)
    {
        (int,int) mapSize=(Mathf.Abs(createMapTest.mapDateList[stageNumber].XMax-createMapTest.mapDateList[stageNumber].XMin),Mathf.Abs(createMapTest.mapDateList[stageNumber].YMax-createMapTest.mapDateList[stageNumber].YMin));
        return x >= 0 && x < mapSize.Item1 && y >= 0 && y < mapSize.Item2;
    }


     /// <summary>
     /// 進化する時に呼ばれる
     /// </summary>
     /// <param name="List"></param>
     /// <param name="putPos"></param>
     void UnitEvolution(List<(int, int)> list, (int, int) putPos)
    {
        list.Add(putPos);

        foreach (var item in evolutionUnitsTemp)
        {
            Destroy(item.gameObject.transform.GetChild(0).gameObject);
        }

         // 最後に置いた場所以外のマスにキャラを置けるようにする
        foreach (var position in list)
        {
            mapDate[position.Item1][position.Item2] = 0;
        }
    }

    /// <summary>
    /// オブジェクトが削除されたときにマップデータを0にする
    /// </summary>
    /// <param name="delObj"></param>
    public void ResetMapData(GameObject delObj)
    {
        for (int i = 0; i <mapDate.Count; i++)
        {
            for (int j = 0; j <mapDate[i].Count; j++)
            { 
                if(mapTile[i][j]==delObj.transform.root.gameObject)
                {
                    mapDate[i][j]=0;
                }
            }
        }
        
    }

    /// <summary>
    /// Listを2次元Listに変換するやつ
    /// </summary>
    /// <param name="originalList"></param>
    /// <param name="rows"></param>
    /// <param name="columns"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    List<List<T>> ConvertToTwoDimensionalList<T>(List<T> originalList, int rows, int columns)
    {   
        List<List<T>> result = new List<List<T>>();
        int currentIndex = 0;

        for (int i = 0; i < rows; i++)
        {
            List<T> row = new List<T>();

           for (int j = 0; j < columns; j++)
           {
               if (currentIndex < originalList.Count)
               {
                   row.Add(originalList[currentIndex]);
                   currentIndex++;
               }
               else
               {
                   break;
               }
           }
    
           result.Add(row);
       }
    
        return result;
    }
    
    //ボタン管理
    public void UseSyringe()
    {
       int syringeCoolDown=GameTime();
       if(syringeCoolDown>300)
       {
           if(syringeCheck)syringeCheck=false;
           else syringeCheck=true;
           syringeCoolDown=0;
       }
    }

    public void ChoiceUnit()
    {
        if(tower1)return;
        unitChoice=0;
        unitNumber=nextUnitList[0]+1;
    }
    public void ChoiceUnit2()
    {
        if(tower2)return;
        unitChoice=1;
        unitNumber=nextUnitList2[0]+1;
    }

    void TowerPut()
    {
        towers.Clear();
        foreach(var tower in towerPrefabs)
        {
            var towerTemp=Instantiate(tower);
            var towerTempScript=towerTemp.GetComponent<Tower>();
            towers.Add(towerTempScript);
            towerTempScript.Init();
            towerTempScript.GetNumber(towers.Count);
        }
        towerCount=towers.Count;  
    }

    void TowerUpdate()
    {
        foreach(var tower in towers)
        {
            tower.ManagedUpdate();
        }
        if(towerCount==0)gameText.text="Game Over";
    }

    public void TowerCountMinus(int number)
    {
        towerCount--;
        if(number==1)
        {
            tower1=true;
            if(unitChoice==0)unitChoice=1;
            unitNumber=nextUnitList2[0]+1;
            notUseImg[0].SetActive(true);
        }
        else
        {
            tower2=true;
            if(unitChoice==1)unitChoice=0;
            unitNumber=nextUnitList[0]+1;
            notUseImg[1].SetActive(true);
        } 
        
    }

    //ごみ
    void TowerPosNotPut()
    {
        int[] rows = { 10, 11, 12 };
        int[] columns = { 7, 8, 9, 23, 24, 25 };

        foreach (int row in rows)
        {
            foreach (int column in columns)
            {
                mapDate[row][column] = 999;
            }
        }
    }
}
