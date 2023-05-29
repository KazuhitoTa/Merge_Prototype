using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Enemy : MonoBehaviour
{
    Vector3 target;
    bool test;
    private int Hp;
    private int Atk;
    private float AtkSpeed;
    private float Speed;
    public float currentHP;
    [SerializeField]private GameObject hpBar;
    [SerializeField]private Transform barPos;
    private Image hpBarImage;
    [SerializeField]EnemyStatusSO enemyStatusSO;
    Animator animator;
    private Unit unitScriptTemp;
    private Tower towerScriptTemp;
    [SerializeField]UnitManager unitManager;
    public List<Unit> unitDate=new List<Unit>();

    public List<Vector3> unitMove=new List<Vector3>();

    

    // 初期化

    public void Init(int enemyNumber)
    {
        //var Towers=GameObject.FindWithTag("Tower").transform;
       
        EnemyStatusSO.EnemyStatus enemyStatus = enemyStatusSO.enemyStatusList[enemyNumber];

        Hp = enemyStatus.HP;
        Atk = enemyStatus.Attack;
        AtkSpeed = enemyStatus.AttackSpeed;
        Speed=enemyStatus.Speed;

        CreateHealthBar();

        animator = GetComponent<Animator>();

        unitManager = GameObject.FindWithTag("GameController").GetComponent<UnitManager>();

        unitMove.Add(new Vector3(-6.1f,2.5f,0f));
        unitMove.Add(new Vector3(-0.1f,2.5f,0f));
        unitMove.Add(new Vector3(5.8f,2.5f,0f));
       
        target=unitMove[1];
    }

	// Updateの呼び出しを制御する
	public void ManagedUpdate()
	{
        if(towerScriptTemp==null&&unitScriptTemp==null)
        {
            if(Vector3.Distance(unitMove[1],transform.position)<0.1f)
            {
                if(Atk%2==0)target=unitMove[2];
                else target=unitMove[0];
            }
            
            transform.position = Vector3.MoveTowards(transform.position, target, Speed * Time.deltaTime);
        }
        
        hpBarImage.fillAmount=Mathf.Lerp(hpBarImage.fillAmount,currentHP/Hp,Time.deltaTime*10f);
	}    

    private void CreateHealthBar()
    {
        GameObject newBar = Instantiate(this.hpBar, barPos.position, Quaternion.identity, transform);

        EnemyHPBar hpBar = newBar.GetComponent<EnemyHPBar>();
        hpBarImage = hpBar.hpBarImage;
        currentHP = Hp;
    }

    public void ReduceHP(float damage)
    {
        currentHP-=damage;
        DeathChack();
    }
    
    private void DeathChack()
    {
        if(currentHP<=0)
        {
            currentHP=0;
            EnemyManager.Instance.RemoveEnemy(this);
        }
    }

    private IEnumerator EnemyAttackRoutine()
    {
        if(unitScriptTemp!=null)
        {
            while (unitScriptTemp.currentHP > 0)
            {
                unitScriptTemp.ReduceHP(Atk);
                yield return new WaitForSeconds(AtkSpeed);
            }
        }
        else if(towerScriptTemp!=null)
        {
            while (towerScriptTemp.currentHP > 0)
            {
                towerScriptTemp.ReduceHP(Atk);
                yield return new WaitForSeconds(AtkSpeed);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Bullet":
                animator.SetTrigger("Hit");
                break;
            case "Player":
                unitScriptTemp = collision.gameObject.GetComponent<Unit>();
                StartCoroutine("EnemyAttackRoutine");
                break;
            case "Tower":
                towerScriptTemp = collision.gameObject.GetComponent<Tower>();
                StartCoroutine("EnemyAttackRoutine");
                break;
        }
    }
    
}
