using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaboManager : MonoBehaviour
{
    GameManager gameManager;
    /// <summary>
    /// コアオブジェクト格納
    /// </summary>
    public GameObject[] CoreButton;
    /// <summary>
    /// コアボタンImageオブジェクト
    /// </summary>
    public Image[] CoreButton_Image;
    /// <summary>
    /// UIテキスト
    /// </summary>
    public Text ReleaseCheck_Text;
    /// <summary>
    /// 選択されているコアナンバー
    /// </summary>
    public int SelectCoreNum;
    /// <summary>
    /// ラボシーン初期化(ゲーム開始時に呼ばれる)
    /// </summary>
    public void Labo_Init()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    /// <summary>
    /// ラボシーン開始処理
    /// </summary>
    public void Labo_Start()
    {
        SelectCoreNum = 0;
        for(int i = 0; i < CoreButton_Image.Length; i++)
        {
            if(gameManager.objectList.objects[i].Lock)
            {
                CoreButton_Image[i].color = new Color(0,0,0,0.3f);
            }
        }
    }

    /// <summary>
    /// ラボシーンUpdate
    /// </summary>
    public void Labo_Update()
    {

    }


    
    /// <summary>
    /// コア1解放
    /// </summary>
    public void Core1Release()
    {
        if(gameManager.objectList.objects[0].Lock)
        {
            gameManager.uiManager.SetCoreReleaseCheckMenu();
            //ReleaseCheck_Text.text = "Core1を開放しますか？";
            //SelectCoreNum = 1;
        }
        else
        {
            gameManager.uiManager.SetAlreadyReleaseMenu();
        }
    }
    /// <summary>
    /// コア2解放
    /// </summary>
    public void Core2Release()
    {
        if(gameManager.objectList.objects[1].Lock)
        {
            gameManager.uiManager.SetCoreReleaseCheckMenu();
            ReleaseCheck_Text.text = "Core2を開放しますか？";
            SelectCoreNum = 2;
        }
        else
        {
            gameManager.uiManager.SetAlreadyReleaseMenu();
        }
    }
    /// <summary>
    /// コア3解放
    /// </summary>
    public void Core3Release()
    {
        if(gameManager.objectList.objects[2].Lock)
        {
            gameManager.uiManager.SetCoreReleaseCheckMenu();
            ReleaseCheck_Text.text = "Core3を開放しますか？";
            SelectCoreNum = 3;
        }
        else
        {
            gameManager.uiManager.SetAlreadyReleaseMenu();
        }
    }
    /// <summary>
    /// コア4解放
    /// </summary>
    public void Core4Release()
    {
        if(gameManager.objectList.objects[3].Lock)
        {
            gameManager.uiManager.SetCoreReleaseCheckMenu();
            ReleaseCheck_Text.text = "Core4を開放しますか？";
            SelectCoreNum = 4;
        }
        else
        {
            gameManager.uiManager.SetAlreadyReleaseMenu();
        }
    }
    /// <summary>
    /// コア5解放
    /// </summary>
    public void Core5Release()
    {
        if(gameManager.objectList.objects[4].Lock)
        {
            gameManager.uiManager.SetCoreReleaseCheckMenu();
            ReleaseCheck_Text.text = "Core5を開放しますか？";
            SelectCoreNum = 5;
        }
        else
        {
            gameManager.uiManager.SetAlreadyReleaseMenu();
        }
    }
    /// <summary>
    /// 解放確認(Yes)
    /// </summary>
    public void Release_Yes()
    {
        gameManager.objectList.objects[SelectCoreNum - 1].Lock = false;
        gameManager.uiManager.SetCoreReleaseCheckMenu();
        CoreButton_Image[SelectCoreNum -1].color = new Color(255,255,255,1.0f);
        SelectCoreNum = 0;
    }
    /// <summary>
    /// 解放確認(No)
    /// </summary>
    public void Release_No()
    {
        gameManager.uiManager.SetCoreReleaseCheckMenu();
    }
}
