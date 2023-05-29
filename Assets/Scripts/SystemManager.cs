using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public sealed class SystemManager : MonoBehaviour
{
    static public SystemManager instance;
    static public SystemManager Instance=>instance;
    static public GameManager.ObjectList UnitList = new GameManager.ObjectList();

    /// <summary>
    /// オブジェクトステータス伝達用
    /// </summary>
    /// <returns>オブジェクトステータス</returns>
    [SerializeField] public GameManager.ObjectList ReadList = new GameManager.ObjectList();
    

    /// <summary>
    /// データパス
    /// </summary>
    private string ProjectPath,FilePath;
    /// <summary>
    /// 直前シーン
    /// </summary>
    private string BeforeScene;

    private void Awake()
    {
        if(instance && this != instance)
        {
            Destroy(this.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(this);

        ProjectPath = Application.dataPath;
        FilePath = ProjectPath + "/ObjectSaveData.json";

        string UnitData = File.ReadAllText(FilePath);
        UnitList = JsonUtility.FromJson<GameManager.ObjectList>(UnitData);
        BeforeScene = "BranchScene";

        ReadList = UnitList;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(BeforeScene);
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0) & (BeforeScene == "NeglectfulDevelopment" || BeforeScene == "test"))
        {
            SceneManager.LoadScene("BranchScene");
        }
        if(Input.GetKeyDown(KeyCode.Alpha1) & BeforeScene == "BranchScene")
        {
            SceneManager.LoadScene("NeglectfulDevelopment");
        }
        if(Input.GetKeyDown(KeyCode.Alpha2) & BeforeScene == "BranchScene")
        {
            SceneManager.LoadScene("StageSelect");
        }
        if(Input.GetKeyDown(KeyCode.Escape))GameEnd();
    }

    /// <summary>
    /// シーン遷移時実行処理
    /// </summary>
    /// <param name="PrevScene">現在シーン(未使用)</param>
    /// <param name="NextScene">遷移後シーン</param>
    void OnActiveSceneChanged(Scene PrevScene, Scene NextScene)
    {
        if(BeforeScene == "BranchScene" & NextScene.name == "NeglectfulDevelopment")
        {
            //Debug.Log("");
        }
        if(BeforeScene == "BranchScene" & NextScene.name == "StageSelect")
        {
            //Debug.Log("");
        }
        if(BeforeScene == "StageSelect" & NextScene.name == "test")
        {
            
        }
        if(BeforeScene == "NeglectfulDevelopment" & NextScene.name == "BranchScene")
        {
            ReadList = UnitList;
        }
        if(BeforeScene == "test" & NextScene.name == "BranchScene")
        {
            
        }
        BeforeScene = NextScene.name;
    }

    public void GameEnd()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
