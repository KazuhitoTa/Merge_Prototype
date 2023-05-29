using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CultivationManager : MonoBehaviour
{   
    GameManager gameManager;
    /// <summary>
    /// コアオブジェ
    /// </summary>
    public GameObject[] Core;
    /// <summary>
    /// コア番号入力
    /// </summary>
    public InputField CoreNumber;
    /// <summary>
    /// システムテキスト
    /// </summary>
    public Text CultivationStartTime_Text,CultivationFinishTime_Text,CultivationLastTime_Text,NowTime_Text,LevelUp_Text,NotRelease_Text,RemainTime_Text,CultivationStop_Text;
    /// <summary>
    /// エフェクトテキストオブジェ
    /// </summary>
    public GameObject LevelUp_Text_Obj,NotRelease_Text_Obj;
    /// <summary>
    /// 培養開始時間
    /// </summary>
    public DateTime CultivationStartTime;
    /// <summary>
    /// 培養終了時間
    /// </summary>
    public DateTime CultivationFinishTime;
    /// <summary>
    /// 前の培養時間
    /// </summary>
    public DateTime CultivationLastTime;

    /// <summary>
    /// 培養短縮時間
    /// </summary>
    public int CultivationShotenTime;

    /// <summary>
    /// 培養シーンステータスリスト
    /// </summary>
    [Serializable]
    public class CultivationStatusList
    {
        [SerializeField] public CultivationStatus cultivationStatus = new CultivationStatus();
        [SerializeField] public CultivationStatus cultivationStatus_Init = new CultivationStatus();
    }

    /// <summary>
    /// 培養シーンステータスリスト
    /// </summary>
    [SerializeField]public CultivationStatusList cultivationStatusList;

    /// <summary>
    /// コア番号インプットフィールド格納数字
    /// </summary>
    public int BeforeIFNum,CurrentIFNum;

    /// <summary>
    /// 時間表示バー
    /// </summary>
    public Slider TimeBar;


    /// <summary>
    /// 培養シーン初期化(シーン開始時に呼ばれる)
    /// </summary>
    public void Cultivation_Init()
    {   
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.gameManagerFunction.LoadCultivationData();
        DateTime.TryParse(cultivationStatusList.cultivationStatus.CultivationStartTime_string, out CultivationStartTime);
        Debug.Log("Init");
    }


    /// <summary>
    /// 培養シーン開始処理
    /// </summary>
    public void Cultivation_Start()
    {
        Debug.Log("Start");
        CoreNumber.text = (cultivationStatusList.cultivationStatus.SelectCoreNum + 1).ToString();
        BeforeIFNum = cultivationStatusList.cultivationStatus.SelectCoreNum + 1;
        CurrentIFNum = cultivationStatusList.cultivationStatus.SelectCoreNum + 1;
        if(!cultivationStatusList.cultivationStatus.Cultivation_Stop)
        {
            Core[cultivationStatusList.cultivationStatus.SelectCoreNum].SetActive(true);
            Cultivation_Excess();
        }
        else
        {
            TextClear();
        }
    }

    /// <summary>
    /// 培養シーンアップデート
    /// </summary>
    public void Cultivation_Update()
    {   
        if(!cultivationStatusList.cultivationStatus.Cultivation_Stop)
        {
            CoreNumber.readOnly = true;
            SetTimeGage();
        }
        else
        {
            CoreNumber.readOnly = false;
            TimeBar.gameObject.SetActive(false);
            RemainTime_Text.gameObject.SetActive(false);
        }
        if(cultivationStatusList.cultivationStatus.CultivationFinishTime_stirng == gameManager.NowTime_string & !cultivationStatusList.cultivationStatus.Cultivation_Stop)
        {
            Cultivation_Finish();
        }
        gameManager.gameManagerFunction.SaveCultivationData();

        NowTime_Text.text = gameManager.NowTime_string;

        Cultivation_Excess();

        if(Input.GetKeyDown(KeyCode.R))AllObjReloadStatus(gameManager.objectList.objects);
    }

    /// <summary>
    /// 培養シーン終了
    /// </summary>
    public void Cultivation_Quit()
    {
        Debug.Log("Quit");
        Core[cultivationStatusList.cultivationStatus.SelectCoreNum].SetActive(false);
    }

    /// <summary>
    /// 培養開始ボタン処理
    /// </summary>
    public void Cultivation_Set()
    {
        Core[cultivationStatusList.cultivationStatus.SelectCoreNum].SetActive(true);
        cultivationStatusList.cultivationStatus.Cultivation_Time = gameManager.objectList.objects[cultivationStatusList.cultivationStatus.SelectCoreNum]._status.LvUpTime;

        Cultivation_TimeSet();
    }

    /// <summary>
    /// 培養終了処理
    /// </summary>
    public void Cultivation_Finish()
    {
        Debug.Log("FINISH");
        Core[cultivationStatusList.cultivationStatus.SelectCoreNum].SetActive(false);
        cultivationStatusList.cultivationStatus.Cultivation_Time = 0;
        Cultivation_LevelUp();
        cultivationStatusList.cultivationStatus.CultivationStartTime_string = "";
        cultivationStatusList.cultivationStatus.CultivationFinishTime_stirng = "";
        cultivationStatusList.cultivationStatus.Cultivation_Stop = true;
        TextClear();
    }

    /// <summary>
    /// 培養コア変更
    /// </summary>
    public void Cultivation_CoreChange()
    {
        for(int i = 0; i < Core.Length; i++)
        {
            if(i == cultivationStatusList.cultivationStatus.SelectCoreNum)
            {
                Core[i].SetActive(true);
            }
            else Core[i].SetActive(false);
        }
    }

    /// <summary>
    /// 培養停止
    /// </summary>
    public void Cultivation_Stop()
    {
        cultivationStatusList.cultivationStatus.Cultivation_Stop = true;
        Core[cultivationStatusList.cultivationStatus.SelectCoreNum].SetActive(false);
        TextClear();
        CultivationStop_Text.gameObject.SetActive(true);
        Invoke("StopText",3);
    }

    /// <summary>
    /// Invoke用
    /// </summary>
    private void StopText()
    {
        CultivationStop_Text.gameObject.SetActive(false);
    }

    /// <summary>
    /// 培養短縮処理
    /// </summary>
    public void Cultivation_Shoten()
    {
        CultivationFinishTime = CultivationFinishTime.AddSeconds(-CultivationShotenTime);
        cultivationStatusList.cultivationStatus.CultivationLastTime_stirng = CultivationLastTime.ToString();
        cultivationStatusList.cultivationStatus.CultivationFinishTime_stirng = CultivationFinishTime.ToString();
        CultivationFinishTime_Text.text = cultivationStatusList.cultivationStatus.CultivationFinishTime_stirng;
        CultivationLastTime_Text.text = cultivationStatusList.cultivationStatus.CultivationLastTime_stirng;
        Cultivation_Excess();
        TimeBar.value += CultivationShotenTime;
    }
    
    /// <summary>
    /// 培養時間超過処理
    /// </summary>
    public void Cultivation_Excess()
    {
        DateTime.TryParse(cultivationStatusList.cultivationStatus.CultivationFinishTime_stirng,out CultivationFinishTime);
        TimeSpan timeSpan;
        timeSpan = CultivationFinishTime - gameManager.NowTime;
        int TimeSum;
        TimeSum = timeSpan.Days*86400 + timeSpan.Hours*3600 + timeSpan.Minutes*60 + timeSpan.Seconds;
        //Debug.Log(TimeSum);
        if(TimeSum <= 0)
        {
            Cultivation_Finish();
        }
    }








    /// <summary>
    /// 培養時間設定
    /// </summary>
    public void Cultivation_TimeSet()
    {   
        Debug.Log("TimeSet");
        CultivationStartTime = DateTime.Now;
        CultivationFinishTime = CultivationStartTime.AddSeconds(cultivationStatusList.cultivationStatus.Cultivation_Time);
        cultivationStatusList.cultivationStatus.CultivationLastTime_stirng = CultivationLastTime.ToString();
        cultivationStatusList.cultivationStatus.CultivationStartTime_string = CultivationStartTime.ToString();
        cultivationStatusList.cultivationStatus.CultivationFinishTime_stirng = CultivationFinishTime.ToString();
        CultivationStartTime_Text.text = cultivationStatusList.cultivationStatus.CultivationStartTime_string;
        CultivationFinishTime_Text.text = cultivationStatusList.cultivationStatus.CultivationFinishTime_stirng;
        CultivationLastTime_Text.text = cultivationStatusList.cultivationStatus.CultivationLastTime_stirng;
        cultivationStatusList.cultivationStatus.Cultivation_Stop = false;
    }

    /// <summary>
    /// 培養レベル上昇
    /// </summary>
    public void Cultivation_LevelUp()
    {
        gameManager.objectList.objects[cultivationStatusList.cultivationStatus.SelectCoreNum]._status.Lv ++;
        LevelUp_Text.gameObject.SetActive(true);
        Invoke("Cultivation_LevelUpText",3);
        GrowthStatus(cultivationStatusList.cultivationStatus.SelectCoreNum);
    }
    
    /// <summary>
    /// Invoke用
    /// </summary>
    private void Cultivation_LevelUpText()
    {
        LevelUp_Text.gameObject.SetActive(false);
    }

    /// <summary>
    /// インプットフィールド制限用処理
    /// </summary>
    public void SelectNumIFEnd()
    {
        if(CoreNumber.text == "")CoreNumber.text = BeforeIFNum.ToString();

        CurrentIFNum = int.Parse(CoreNumber.text);
        if(CurrentIFNum < 1 || CurrentIFNum > gameManager.objectList.objects.Length + 1)
        {
            CoreNumber.text = BeforeIFNum.ToString();
            return;
        }
        
        if(gameManager.objectList.objects[CurrentIFNum - 1].Lock)
        {
            CoreNumber.text = BeforeIFNum.ToString();
            NotRelease_Text.gameObject.SetActive(true);
            Invoke("CoreNotRelease",3);
        }
        else CoreNumber.text = CurrentIFNum.ToString();
        
        cultivationStatusList.cultivationStatus.SelectCoreNum = int.Parse(CoreNumber.text) - 1;
        CurrentIFNum = int.Parse(CoreNumber.text);
        BeforeIFNum = int.Parse(CoreNumber.text);
    }

    /// <summary>
    /// Invoke用
    /// </summary>
    private void CoreNotRelease()
    {
        NotRelease_Text.gameObject.SetActive(false);
    }

    /// <summary>
    /// ステータス上昇処理
    /// </summary>
    /// <param name="ObjNum">上昇させるObject</param>
    public void GrowthStatus(int ObjNum)
    {
        gameManager.objectList.objects[ObjNum]._status.Hp += gameManager.objectInitList.Growth_Value[ObjNum].GrowHp_Value;
        gameManager.objectList.objects[ObjNum]._status.Attack += gameManager.objectInitList.Growth_Value[ObjNum].GrowAttack_Value;
        gameManager.objectList.objects[ObjNum]._status.Defence += gameManager.objectInitList.Growth_Value[ObjNum].GrowDefence_Value;
    }

    /// <summary>
    /// ステータスリロード処理（再計算）
    /// </summary>
    /// <param name="ObjNum">指定Object</param>
    private void ReloadStatus(int ObjNum)
    {
        gameManager.objectList.objects[ObjNum]._status.Hp = gameManager.objectInitList.objects[ObjNum]._status.Hp + gameManager.objectInitList.Growth_Value[ObjNum].GrowHp_Value * (gameManager.objectList.objects[ObjNum]._status.Lv - 1);
        gameManager.objectList.objects[ObjNum]._status.Attack = gameManager.objectInitList.objects[ObjNum]._status.Attack + gameManager.objectInitList.Growth_Value[ObjNum].GrowAttack_Value * (gameManager.objectList.objects[ObjNum]._status.Lv - 1);
        gameManager.objectList.objects[ObjNum]._status.Defence = gameManager.objectInitList.objects[ObjNum]._status.Defence + gameManager.objectInitList.Growth_Value[ObjNum].GrowDefence_Value * (gameManager.objectList.objects[ObjNum]._status.Lv - 1);
    }

    /// <summary>
    /// 複数ステータスリロード
    /// </summary>
    /// <param name="ArreyObj">Object配列</param>
    public void AllObjReloadStatus(GameManager.Object[] ArreyObj)
    {
        for(int i = 0; i < ArreyObj.Length; i++)
        {
            ReloadStatus(i);
        }
    }

    /// <summary>
    /// 時間表示バ―設定処理
    /// </summary>
    public void SetTimeGage()
    {
        TimeBar.gameObject.SetActive(true);
        RemainTime_Text.gameObject.SetActive(true);
        TimeBar.maxValue = gameManager.objectList.objects[cultivationStatusList.cultivationStatus.SelectCoreNum]._status.LvUpTime;
        DateTime.TryParse(cultivationStatusList.cultivationStatus.CultivationFinishTime_stirng,out CultivationFinishTime);
        TimeSpan timeSpan;
        timeSpan = CultivationFinishTime - DateTime.Now;
        int TimeSum;
        TimeSum = timeSpan.Days*86400 + timeSpan.Hours*3600 + timeSpan.Minutes*60 + timeSpan.Seconds;
        int RemainTime;
        RemainTime = gameManager.objectList.objects[cultivationStatusList.cultivationStatus.SelectCoreNum]._status.LvUpTime - TimeSum;
        TimeBar.value = RemainTime;
        int CourseTime;
        CourseTime = gameManager.objectList.objects[cultivationStatusList.cultivationStatus.SelectCoreNum]._status.LvUpTime - RemainTime;

        RemainTime_Text.text = "残り" + CourseTime+ "秒";
    }

    /// <summary>
    /// システムテキスト表示クリア
    /// </summary>
    private void TextClear()
    {
        cultivationStatusList.cultivationStatus.CultivationStartTime_string = "";
        cultivationStatusList.cultivationStatus.CultivationFinishTime_stirng = "";

        CultivationStartTime_Text.text = "";
        CultivationLastTime_Text.text = "";
        CultivationFinishTime_Text.text = "";
    }
}