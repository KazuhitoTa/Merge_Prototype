using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float atk;
    [SerializeField]private float moveSpeed=10f;
    [SerializeField]private float damagerDistance=10f;
    private Enemy enemyTarget;
    private int bulletNumber;

    //////////////////////////////////////////////
    
    private SystemManager systemManager;
    private int RaceNumber;
    private int LevelNumber;
    private int EvolutionNum = 5;

   
    public void Init()
    {
        GameObject system = GameObject.Find("SystemManager");
        systemManager = system.GetComponent<SystemManager>();
    }
    
    public void ManagedUpdate()
    {
        int ListAttack;
        RaceNumber = bulletNumber / EvolutionNum;
        ListAttack = systemManager.ReadList.objects[RaceNumber]._status.Attack;
        LevelNumber = (bulletNumber % EvolutionNum) + 1;
        atk = ListAttack * LevelNumber;

    
        if(enemyTarget!=null)
        {
            MoveBullet();
        }
        else BulletManager.Instance.RemoveBullet(this);
    }

    public void GetInformation(Enemy enemy,int unitNumber)
    {
        enemyTarget=enemy;
        bulletNumber=unitNumber;
    }

    void MoveBullet()
    {
        //現在地からの移動
        transform.position=Vector2.MoveTowards(transform.position,enemyTarget.transform.position,moveSpeed*Time.deltaTime);

        //弾と敵の距離確認
        CheckDistance();
    }

    /// <summary>
    /// 弾と敵の位置を確認して近ければダメージ
    /// </summary>
    void CheckDistance()
    {
        float distanceToTarget=(enemyTarget.transform.position-transform.position).magnitude;

        if(distanceToTarget<damagerDistance)
        {
            enemyTarget.ReduceHP(atk);
            BulletManager.Instance.RemoveBullet(this);
        }

    }

}
