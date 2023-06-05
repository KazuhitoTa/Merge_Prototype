using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;
using TMPro;

public class Stage : MonoBehaviour
{
    //マップ生成とプレイヤー操作用変数
    [SerializeField] MapManager _mapManager=null;
    //敵生成用変数
    [SerializeField]EnemyManager _enemyManager = null;
    [SerializeField]UnitManager _unitManager=null;
    [SerializeField]BulletManager _bulletManager=null;
    
    bool pause;

    void Awake()
    {
        _enemyManager.EnemyAwake();
        _unitManager.UnitAwake();
        _bulletManager.BulletAwake();
    }

    void Start()
    {
        _enemyManager.Init();
        _mapManager.Init();
        //_tower.Init();
        //_tower2.Init();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))SceneManager.LoadScene("StageSelect");
        if(pause)return;
        _mapManager.ManagedUpdate();
        _unitManager.ManagedUpdate();
        _enemyManager.ManagedUpdate();
        _bulletManager.BulletUpdate();
        //_tower.ManagedUpdate();
        //_tower2.ManagedUpdate();
    }

    
}
