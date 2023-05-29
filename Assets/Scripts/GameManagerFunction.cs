using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


/// <summary>
/// ゲーム状態
/// </summary>
[Serializable]
public class GamePartStatus
{
    public enum GameName
    {
        Home,
        Cultivation,
        Labo,
    }

    public GameName gameName;
}

public class GameManagerFunction : MonoBehaviour
{
    GameManager gameManager;
    private string FolderNamePath = "/";

    /// <summary>
    /// 培養シーン開始時に呼ばれる
    /// </summary>
    public void ApplicationInit()
    {
        gameManager = transform.parent.gameObject.GetComponent<GameManager>();
        gameManager.gamePartStatus.gameName = GamePartStatus.GameName.Home;
        gameManager.gameManageStatus.ProjectPath = Application.dataPath;
        gameManager.GameDataPath = gameManager.gameManageStatus.ProjectPath + FolderNamePath + gameManager.gameManageStatus.GameDataFile;
        gameManager.PlayerDataPath = gameManager.gameManageStatus.ProjectPath + FolderNamePath + gameManager.gameManageStatus.PlayerDataFile;
        gameManager.CultivationDataPath = gameManager.gameManageStatus.ProjectPath + FolderNamePath + gameManager.gameManageStatus.CultivationDataFile;
        gameManager.ObjectDataPath = gameManager.gameManageStatus.ProjectPath + FolderNamePath + gameManager.gameManageStatus.ObjectDataFile;
        gameManager.ObjectInitDataPath = gameManager.gameManageStatus.ProjectPath + FolderNamePath + gameManager.gameManageStatus.ObjectInitDataFile;
    }

    /// <summary>
    /// 初期化実行かどうか
    /// </summary>
    public void InitCheck()
    {
        if(!File.Exists(gameManager.GameDataPath) || gameManager.gameManageStatus.Initialize)
        {
            gameManager.gameManageStatus.Initialize = true;
        }
        else gameManager.gameManageStatus.Initialize = false;
    }

    /// <summary>
    /// 初期化処理(共通処理)
    /// </summary>
    public void GameInit()
    {
        if(gameManager.gameManageStatus.Initialize)
        {
            DataInitialize();
            SaveObjectData();
        }
        DateTime FirstTime;
        DateTime.TryParse(gameManager.gameManageStatus.FirstPlayTime,out FirstTime);
        gameManager.playerStatus.PlayerFirstPlayTime = gameManager.gameManageStatus.FirstPlayTime;
        for(int i = 0; i < gameManager.uiManager.MenuCanvas.Length; i++)
        {
            gameManager.uiManager.MenuCanvas[i].SetActive(false);
        }
        gameManager.uiManager.MenuCanvas[0].SetActive(true);
    }


    /// <summary>
    /// データ初期化(Trueなら)
    /// </summary>
    public void DataInitialize()
    {
        Debug.Log("全初期化");
        DateTime FirstTime;
        FirstTime = DateTime.Now;
        gameManager.gameManageStatus.FirstPlayTime = FirstTime.ToString();
        gameManager.gameManageStatus.LastPlayTime = FirstTime.ToString();

        ObjectDataInitialize();
        CultivationDataInitialize();



        gameManager.gameManageStatus.Initialize = false;
    }


    /// <summary>
    /// ゲーム進行
    /// </summary>
    public void GameProcess()
    {   
        Game_Update();
        if(gameManager.gamePartStatus.gameName == GamePartStatus.GameName.Home)
        {
            if(Input.GetKeyDown(KeyCode.Delete))
            {
                gameManager.uiManager.SetDeleteMenu();
            }
        }
        else if(gameManager.gamePartStatus.gameName == GamePartStatus.GameName.Cultivation)
        {
            gameManager.cultivationManager.Cultivation_Update();
            SaveCultivationData();
        }
        else if(gameManager.gamePartStatus.gameName == GamePartStatus.GameName.Labo)
        {
            
        }
    }

    /// <summary>
    /// ゲームステータスアップデート(全シーン)
    /// </summary>
    public void Game_Update()
    {
        gameManager.NowTime = DateTime.Now;
        gameManager.NowTime_string = gameManager.NowTime.ToString();
    }

    /// <summary>
    /// ゲーム終了時処理
    /// </summary>
    public void GameQuit()
    {
        DateTime LastTime;
        LastTime = DateTime.Now;
        gameManager.gameManageStatus.LastPlayTime = LastTime.ToString();

        
        SaveGameData();
        SavePlayerData();
        SaveObjectData();
    }

    /// <summary>
    /// オブジェクトデータ初期化
    /// </summary>
    public void ObjectDataInitialize()
    {
        string ObjectInitData = File.ReadAllText(gameManager.ObjectInitDataPath);
        gameManager.objectList = JsonUtility.FromJson<GameManager.ObjectList>(ObjectInitData);
    }

    /// <summary>
    /// 培養ステータス初期化
    /// </summary>
    public void CultivationDataInitialize()
    {
        LoadCultivationData();
        gameManager.cultivationManager.cultivationStatusList.cultivationStatus.Cultivation_Stop = gameManager.cultivationManager.cultivationStatusList.cultivationStatus_Init.Cultivation_Stop;
        gameManager.cultivationManager.cultivationStatusList.cultivationStatus.Cultivation_Time = gameManager.cultivationManager.cultivationStatusList.cultivationStatus_Init.Cultivation_Time;
        gameManager.cultivationManager.cultivationStatusList.cultivationStatus.CultivationLastTime_stirng = gameManager.cultivationManager.cultivationStatusList.cultivationStatus_Init.CultivationLastTime_stirng;
        gameManager.cultivationManager.cultivationStatusList.cultivationStatus.CultivationStartTime_string = gameManager.cultivationManager.cultivationStatusList.cultivationStatus_Init.CultivationStartTime_string;
        gameManager.cultivationManager.cultivationStatusList.cultivationStatus.CultivationFinishTime_stirng = gameManager.cultivationManager.cultivationStatusList.cultivationStatus_Init.CultivationFinishTime_stirng;
        SaveCultivationData();

        gameManager.cultivationManager.BeforeIFNum = 1;
        gameManager.cultivationManager.CurrentIFNum = 1;
    }

    /// <summary>
    /// ゲームステータスのデータロード
    /// </summary>
    public void LoadGameData()
    {
        if(File.Exists(gameManager.GameDataPath))
        {
            string GameData = File.ReadAllText(gameManager.GameDataPath);
            gameManager.gameManageStatus = JsonUtility.FromJson<GameManageStatus>(GameData);
        }
    }
    /// <summary>
    /// プレイヤーステータスのデータロード
    /// </summary>
    public void LoadPlayerData()
    {
        if(File.Exists(gameManager.PlayerDataPath))
        {
            string PlayerData = File.ReadAllText(gameManager.PlayerDataPath);
            gameManager.playerStatus = JsonUtility.FromJson<PlayerStatus>(PlayerData);
        }
    }

    /// <summary>
    /// 培養ステータスのデータロード
    /// </summary>
    public void LoadCultivationData()
    {
        if(File.Exists(gameManager.CultivationDataPath))
        {
            string CultivationData = File.ReadAllText(gameManager.CultivationDataPath);
            gameManager.cultivationManager.cultivationStatusList = JsonUtility.FromJson<CultivationManager.CultivationStatusList>(CultivationData);
        }
    }
    /// <summary>
    /// オブジェクト初期データのロード
    /// </summary>
    public void LoadObjectInitData()
    {
        if(File.Exists(gameManager.ObjectInitDataPath))
        {
            string ObjectInitData = File.ReadAllText(gameManager.ObjectInitDataPath);
            gameManager.objectInitList = JsonUtility.FromJson<GameManager.ObjectInitList>(ObjectInitData);
        }
    }

    /// <summary>
    /// オブジェクトデータのロード
    /// </summary>
    public void LoadObjectData()
    {
        if(File.Exists(gameManager.ObjectDataPath))
        {
            string ObjectData = File.ReadAllText(gameManager.ObjectDataPath);
            gameManager.objectList = JsonUtility.FromJson<GameManager.ObjectList>(ObjectData);
        }
    }

    /// <summary>
    /// ゲームステータスデータのセーブ
    /// </summary>
    public void SaveGameData()
    {
        var GameData = JsonUtility.ToJson(gameManager.gameManageStatus,true);
        File.WriteAllText(gameManager.GameDataPath,GameData);
    }
    /// <summary>
    /// プレイヤーステータスデータのセーブ
    /// </summary>
    public void SavePlayerData()
    {
        var PlayerData = JsonUtility.ToJson(gameManager.playerStatus,true);
        File.WriteAllText(gameManager.PlayerDataPath,PlayerData);
    }
    /// <summary>
    /// 培養ステータスのセーブ
    /// </summary>
    public void SaveCultivationData()
    {
        var CultivationData = JsonUtility.ToJson(gameManager.cultivationManager.cultivationStatusList,true);
        File.WriteAllText(gameManager.CultivationDataPath,CultivationData);
    }
    /// <summary>
    /// オブジェクト初期データのセーブ
    /// </summary>
    public void SaveObjectInitData()
    {
        var ObjectInitData = JsonUtility.ToJson(gameManager.objectInitList,true);
        File.WriteAllText(gameManager.ObjectInitDataPath,ObjectInitData);
    }
    /// <summary>
    /// オブジェクトデータのセーブ
    /// </summary>
    public void SaveObjectData()
    {
        var ObjectData = JsonUtility.ToJson(gameManager.objectList,true);
        File.WriteAllText(gameManager.ObjectDataPath,ObjectData);
    }
}
