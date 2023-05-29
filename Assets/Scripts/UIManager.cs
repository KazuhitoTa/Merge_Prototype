using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    GameObject GameManager;
    GameManager gameManager;

    /// <summary>
    /// メニューオブジェクト一覧(インスペクター内掲示)
    /// </summary>
    [NamedArrayAttribute(new string[]{"デフォルトメニュー","プレイヤーステータスメニュー","プレイヤー情報","培養メニュー","研究所メニュー","初期化メニュー","コア解放確認メニュー","コア解放済みメニュー"})]
    public GameObject[] MenuCanvas;
    public Text[] PlayerInfoText;

    /// <summary>
    /// UIマネージャー初期化(ゲーム開始時に呼ばれる)
    /// </summary>
    public void UIManagerInit()
    {
        GameManager = GameObject.Find("GameManager");
        gameManager = GameManager.GetComponent<GameManager>();
    }

    /// <summary>
    /// プレイヤーステータスメニューを開く
    /// </summary>
    public void SetStatusMenu()
    {
        SetExchangeActive(MenuCanvas[0],MenuCanvas[1]);
    }
    /// <summary>
    /// プレイヤ０情報を開く
    /// </summary>
    public void SetPlayerInfoMenu()
    {
        SetExchangeActive(MenuCanvas[1],MenuCanvas[2]);
    }
    /// <summary>
    /// ラボシーン遷移(開閉処理)
    /// </summary>
    public void SetLabo()
    {
        SetExchangeActive(MenuCanvas[0],MenuCanvas[4]);
        if(gameManager.gamePartStatus.gameName == GamePartStatus.GameName.Home)
        {
            gameManager.gamePartStatus.gameName = GamePartStatus.GameName.Labo;
            gameManager.laboManager.Labo_Start();
        }
        else
        {
            gameManager.gamePartStatus.gameName = GamePartStatus.GameName.Home;
        }
    }
    /// <summary>
    /// 培養シーン遷移(開閉処理)
    /// </summary>
    public void SetCultivation()
    {
        SetExchangeActive(MenuCanvas[0],MenuCanvas[3]);
        if(gameManager.gamePartStatus.gameName == GamePartStatus.GameName.Home)
        {
            gameManager.gamePartStatus.gameName = GamePartStatus.GameName.Cultivation;
            gameManager.cultivationManager.Cultivation_Start();
        }
        else
        {
            gameManager.gamePartStatus.gameName = GamePartStatus.GameName.Home;
            gameManager.cultivationManager.Cultivation_Quit();
        }
    }

    /// <summary>
    /// オブジェクトのアクティブを反転する
    /// </summary>
    /// <param name="gameObject">対象オブジェクト</param>
    public void SetActiveObject(GameObject gameObject)
    {
        if(gameObject.activeSelf == true)
        {
            gameObject.SetActive(false);
        }
        else gameObject.SetActive(true);
    }

    /// <summary>
    /// 2つのオブジェクトのアクティブを反転する
    /// </summary>
    /// <param name="Object1">対象オブジェクト1</param>
    /// <param name="Object2">対象オブジェクト2</param>
    public void SetExchangeActive(GameObject Object1,GameObject Object2)
    {
        if(Object1.activeSelf == true)
        {
            Object1.SetActive(false);
        }
        else Object1.SetActive(true);
        if(Object2.activeSelf == true)
        {
            Object2.SetActive(false);
        }
        else Object2.SetActive(true);
    }
    /// <summary>
    /// プレイヤー情報の更新
    /// </summary>
    public void PlayerInfoUpdate()
    {
        if(MenuCanvas[2].activeSelf == true)
        {
            PlayerInfoText[0].text = "プレイヤー名は" + gameManager.playerStatus.PlayerName;
            PlayerInfoText[1].text = "プレイヤーランクは" + gameManager.playerStatus.PlayerRank.ToString();
            PlayerInfoText[2].text = "プレイヤーの総経験値は" + gameManager.playerStatus.PlayerExperience.ToString();
            PlayerInfoText[3].text = "最初にプレイした時間は" + gameManager.gameManageStatus.FirstPlayTime;
            PlayerInfoText[4].text = "プレイヤーが所持しているお金は" + gameManager.playerStatus.PlayerMoney;
        }
    }

    /// <summary>
    /// プレイヤーの所持金増加処理(Debug)
    /// </summary>
    public void AddMoney()
    {
        gameManager.playerStatus.PlayerMoney += 100;
    }

    /// <summary>
    /// 培養開始ボタン処理
    /// </summary>
    public void CultivationStart()
    {   
        if(gameManager.cultivationManager.cultivationStatusList.cultivationStatus.Cultivation_Stop)
        {
            gameManager.cultivationManager.Cultivation_Set();
        }
    }
    /// <summary>
    /// 培養停止ボタン処理
    /// </summary>
    public void CultivationStop()
    {
        gameManager.cultivationManager.Cultivation_Stop();
    }
    /// <summary>
    /// 培養時間短縮ボタン処理
    /// </summary>
    public void CultivationShoten()
    {
        if(!gameManager.cultivationManager.cultivationStatusList.cultivationStatus.Cultivation_Stop)
        {
            gameManager.cultivationManager.Cultivation_Shoten();
        }
    }

    /// <summary>
    /// 選択コア番号入力箱の制限処理
    /// </summary>
    public void CoreNumberInputEnd()
    {
        gameManager.cultivationManager.SelectNumIFEnd();
    }
    /// <summary>
    /// オブジェクト初期データセーブボタン処理
    /// </summary>
    public void ObjectInitSaveButton()
    {
        gameManager.gameManagerFunction.SaveObjectInitData();
    }
    /// <summary>
    /// 初期化確認メニュー(開閉処理)
    /// </summary>
    public void SetDeleteMenu()
    {
        SetExchangeActive(MenuCanvas[0],MenuCanvas[5]);
    }
    /// <summary>
    /// 初期化選択処理
    /// </summary>
    public void InitYes()
    {
        gameManager.gameManageStatus.Initialize = true;
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    /// <summary>
    /// 初期化選択拒否処理
    /// </summary>
    public void InitNo()
    {
        SetExchangeActive(MenuCanvas[0],MenuCanvas[5]);
    }

    /// <summary>
    /// コア解放確認メニュー(開閉処理)
    /// </summary>
    public void SetCoreReleaseCheckMenu()
    {
        SetExchangeActive(MenuCanvas[4],MenuCanvas[6]);
    }
    /// <summary>
    /// 既解放確認メニュー(開閉処理)
    /// </summary>
    public void SetAlreadyReleaseMenu()
    {
        SetExchangeActive(MenuCanvas[4],MenuCanvas[7]);
    }

}
