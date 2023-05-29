using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameManageStatus
{
    /// <summary>
    /// プロジェクトパス
    /// </summary>
    public string ProjectPath;
    /// <summary>
    /// ゲームステータスデータ保存ファイル名
    /// </summary>
    public string GameDataFile = "GameSaveData.json";
    /// <summary>
    /// プレイヤーステータスデータ保存ファイル名
    /// </summary>
    public string PlayerDataFile = "PlayerSaveData.json";
    /// <summary>
    /// 培養ステータスデータ保存ファイル名
    /// </summary>
    public string CultivationDataFile = "CultivationSaveData.json";
    /// <summary>
    /// オブジェクトデータ保存ファイル名
    /// </summary>
    public string ObjectDataFile = "ObjectSaveData.json";
    /// <summary>
    /// オブジェクト初期データ保存ファイル名
    /// </summary>
    public string ObjectInitDataFile = "ObjectInitSaveData.json";
    /// <summary>
    /// 新規プレイ時間（初期化された時間）
    /// </summary>
    public string FirstPlayTime;
    /// <summary>
    /// 最終プレイ時間
    /// </summary>
    public string LastPlayTime;
    /// <summary>
    /// 初期化判定
    /// </summary>
    public bool Initialize;
}

/// <summary>
/// 培養ステータス
/// </summary>
[Serializable]
public class CultivationStatus
{
    public int SelectCoreNum;
    public string CultivationStartTime_string;
    public string CultivationFinishTime_stirng;
    public string CultivationLastTime_stirng;
    public bool Cultivation_Stop;
    public int Cultivation_Time;
}
