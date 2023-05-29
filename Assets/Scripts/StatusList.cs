using UnityEngine;
using System;

[Serializable]
/// <summary>
/// 基本ステータス
/// </summary>
public class Status
{
    public int Lv;
    public int Hp;
    public int Attack;
    public float AttackSpeed;
    public int Defence;
    public int LvUpTime;
    public int ObjNum;
    public Status(int lv,int hp,int attack,float attackspeed,int defence,int lvuptime,int objnum){
        Lv = lv;
        Hp = hp;
        Attack = attack;
        AttackSpeed = attackspeed;
        Defence = defence;
        LvUpTime = lvuptime;
        ObjNum = objnum;
    }
}


/// <summary>
/// オブジェクトステータス変化値
/// </summary>
[Serializable]
public class GrowStatus
{
    public string Name;
    public int GrowHp_Value;
    public int GrowAttack_Value;
    public int GrowDefence_Value;
}

/// <summary>
/// プレイヤーステータス
/// </summary>
[Serializable]
public class PlayerStatus
{
    public string PlayerName;
    public int PlayerRank;
    public int PlayerExperience;
    public string PlayerFirstPlayTime;
    public int PlayerMoney;
}
