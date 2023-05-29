using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tower : MonoBehaviour
{
    private int Hp;
    public float currentHP;
    [SerializeField]private GameObject hpBar;
    [SerializeField]private Transform barPos;
    private Image hpBarImage;
    [SerializeField]TextMeshProUGUI gameOver;
    private MapManager mapManager;
    private int myNumber;

	public void Init()
	{
        mapManager=GameObject.FindWithTag("GameController").transform.GetComponent<MapManager>();
        Hp=100;
        CreateHealthBar();
	}

	// Updateの呼び出しを制御する
	public void ManagedUpdate()
	{   
        hpBarImage.fillAmount=Mathf.Lerp(hpBarImage.fillAmount,currentHP/Hp,Time.deltaTime*10f);
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
            mapManager.TowerCountMinus(myNumber);
        }
    }

    public int GetNumber(int Number)
    {
        myNumber=Number;
        //Debug.Log(myNumber);
        return myNumber;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
