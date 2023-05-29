using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;


public class GameManager : MonoBehaviour
{
    [SerializeField] public GameManageStatus gameManageStatus = new GameManageStatus();
    public GameManagerFunction gameManagerFunction;

    [SerializeField] public GamePartStatus gamePartStatus = new GamePartStatus();


    [SerializeField] public PlayerStatus playerStatus = new PlayerStatus();
    //GameManagerFunction gameManagerFunction = new GameManagerFunction();

    public DateTime NowTime;
    public string NowTime_string;

    /// <summary>
    /// オブジェクトデータ
    /// </summary>
    [Serializable]
    public class Object
    {
        public string Name;
        public bool Lock;
        public GameObject GameObj;
        public Vector3 Vec3;
        public Status _status;
        public Object(string name,GameObject gameobj,Vector3 vec3,Status status)
        {
            Name = name;
            GameObj = gameobj;
            Vec3 = vec3;
            _status = status;
        }
    }


    /// <summary>
    /// オブジェクトデータリスト
    /// </summary>
    [Serializable]
    public class ObjectList
    {
        public Object[] objects;
    }

    [SerializeField]public ObjectList objectList = new ObjectList();


    /// <summary>
    /// オブジェクトデータ初期化リスト
    /// </summary>
    [Serializable]
    public class ObjectInitList
    {
        public Object[] objects;
        public GrowStatus[] Growth_Value;
    }
    [SerializeField]public ObjectInitList objectInitList = new ObjectInitList();






    [SerializeField]

    //シーン内全マネージャー
    public UIManager uiManager;
    public CultivationManager cultivationManager;
    public LaboManager laboManager;

    /// <summary>
    /// ゲームステータスデータのパス
    /// </summary>
    public string GameDataPath;
    /// <summary>
    /// プレイヤーデータのパス
    /// </summary>
    public string PlayerDataPath;
    /// <summary>
    /// 培養ステータスデータのパス
    /// </summary>
    public string CultivationDataPath;
    /// <summary>
    /// オブジェクトデータのパス
    /// </summary>
    public string ObjectDataPath;
    /// <summary>
    /// オブジェクト初期データのパス
    /// </summary>
    public string ObjectInitDataPath;

    void Start()
    {   

        gameManagerFunction = transform.GetChild(0).gameObject.GetComponent<GameManagerFunction>();
        gameManagerFunction.ApplicationInit();
        gameManagerFunction.LoadGameData();
        gameManagerFunction.LoadPlayerData();
        gameManagerFunction.LoadCultivationData();
        gameManagerFunction.InitCheck();
        gameManagerFunction.GameInit();
        cultivationManager.Cultivation_Init();
        laboManager.Labo_Init();
        uiManager.UIManagerInit();
        gameManagerFunction.LoadObjectInitData();
        gameManagerFunction.LoadObjectData();
        
        Debug.Log(gameManageStatus.ProjectPath);

    }

    void Update()
    {
        gameManagerFunction.GameProcess();

        uiManager.PlayerInfoUpdate();
    }

    void OnApplicationQuit()
    {
        gameManagerFunction.GameQuit();
    }
}
