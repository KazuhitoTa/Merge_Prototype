using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public int kindNumber;
    private int Hp;
    private int Atk;
    private float AtkSpeed;
    public float currentHP;
    [SerializeField]private GameObject hpBar;
    [SerializeField]private Transform barPos;
    private Image hpBarImage;
    private Coroutine bulletCreateCoroutine; 
    private BulletManager manager;
    private List<Collider2D> collsionTemp=new List<Collider2D>();

    //////////////////////////////////////////////
    
    private SystemManager systemManager;
    private int RaceNumber;
    private int LevelNumber;
    private int EvolutionNum = 5;

    // 初期化
	public void Init(int unitNumber)
	{
        ///////////////
        GameObject system = GameObject.Find("SystemManager");
        systemManager = system.GetComponent<SystemManager>();
        kindNumber=unitNumber;
        defaultState();
        CreateHealthBar();
        var obj =GameObject.FindGameObjectWithTag("GameController");
        manager=obj.GetComponent<BulletManager>();

        //Debug.Log(systemManager.ReadList.objects[RaceNumber]._status.Hp * ( ( kindNumber % ( RaceNumber * 5 ) ) + 1 ));
	}

	// Updateの呼び出しを制御する
	public void ManagedUpdate()
	{
        if(hpBarImage!=null)hpBarImage.fillAmount=Mathf.Lerp(hpBarImage.fillAmount,currentHP/Hp,Time.deltaTime*10f);
        if(collsionTemp.Count!=0)
        {
            foreach (var collision in collsionTemp)
            {
                bulletCreateCoroutine = StartCoroutine(BulletCreateRoutine(collision));
            }
            collsionTemp.Clear();
        }  
	}
    
    public void defaultState()
    {
        int ListHp;
        int ListAttack;
        float ListAttackSpeed;
        RaceNumber = kindNumber / EvolutionNum;
        ListHp = systemManager.ReadList.objects[RaceNumber]._status.Hp;
        ListAttack = systemManager.ReadList.objects[RaceNumber]._status.Attack;
        ListAttackSpeed = systemManager.ReadList.objects[RaceNumber]._status.AttackSpeed;
        LevelNumber = (kindNumber % EvolutionNum) + 1;
        Hp = ListHp * LevelNumber;
        Atk = ListAttack * LevelNumber;
        AtkSpeed = ListAttackSpeed;
    }

    private void CreateHealthBar()
    {
        GameObject newBar=Instantiate(hpBar,barPos.position,Quaternion.identity);
        newBar.transform.SetParent(transform);

        EnemyHPBar healthBar=newBar.GetComponent<EnemyHPBar>();
        hpBarImage=healthBar.hpBarImage; 
        currentHP=Hp;   
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
            UnitManager.Instance.RemoveUnit(this);
        }
    }

    private IEnumerator BulletCreateRoutine(Collider2D collision)
    {
        Enemy enemyScriptTemp = collision.gameObject.GetComponent<Enemy>();
        while (enemyScriptTemp.currentHP > 0)
        {
            BulletCreate(enemyScriptTemp);
            yield return new WaitForSeconds(AtkSpeed);
        }
    }

    private void BulletCreate(Enemy enemy)
    {
        Bullet bulletScriptTemp = manager.CreateBullet(kindNumber,transform.position);
        bulletScriptTemp.GetInformation(enemy,kindNumber);  
    }

    public void HitEnemy(Collider2D collision)
    {
        collsionTemp.Add(collision);
    }

    public float Distance(GameObject targetUnit)
    {
        return Vector3.Distance(transform.position, targetUnit.transform.position);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collsionTemp.Remove(collision);
    }



}
