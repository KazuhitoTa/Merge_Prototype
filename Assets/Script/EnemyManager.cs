using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EnemyManager : MonoBehaviour
{
	[SerializeField]EnemyStatusSO enemyStatusSO;
	[SerializeField]MapStatusSO mapStatusSO;
	private Coroutine EnemyCreateCoroutine;
	[SerializeField]TextMeshProUGUI text;
	private bool check;

	private List<Enemy> _enemies = new List<Enemy>();

	private static EnemyManager _instance;
    public static EnemyManager Instance
    {
        get { return _instance; }
    }

    public void EnemyAwake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

	public void Init()
	{
		StartCoroutine("Wave");
	}


	// 敵を生成する
	public Enemy CreateEnemy(int enemyNumber,Vector2 pos)
	{
		var obj = Instantiate(enemyStatusSO.enemyStatusList[enemyNumber].EnemyModel,pos,Quaternion.identity);

		var enemy = obj.GetComponent<Enemy>();
		if (enemy != null)
		{
			// 初期化
			enemy.Init(enemyNumber);

			// 敵リストに追加する
			_enemies.Add(enemy);

			return enemy;
		}

		return null;
	}

	private IEnumerator Wave()
	{
		for(int i=0;i<3;i++)
		{
			EnemySet(0);
			yield return new WaitForSeconds(40);
		}
		check=true;
	}
	public void EnemySet(int stageNumber)
	{
		var temp=mapStatusSO.mapStatusList[stageNumber].Tuples;
		for(int i=0;i<temp.Length;i++)
		{
			CreateEnemy(temp[i].enemySpawnNumber,temp[i].enemySpawn);
		}

	}

	public void RemoveEnemy(Enemy enemy)
    {
        _enemies.Remove(enemy);
		Destroy(enemy.gameObject);
    }

	void ChangeScene()
    {
        SceneManager.LoadScene("StageSelect");
    }


	// Updateの呼び出しを制御する
	public void ManagedUpdate()
	{
		// 生成されている敵のUpdateを呼び出し
		foreach (var enemy in _enemies)
		{
			enemy.ManagedUpdate();
		}
		if(_enemies.Count==0&&check)
		{
			text.text="GameClear";
			Invoke("ChangeScene",5.0f);
		}
		
	}

}
