using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarryStageNumber : MonoBehaviour
{
    public static int StageNumber;
    public void Stage1()
    {
        StageNumber=0;
        SceneManager.LoadScene("test");
    }
    public void Stage2()
    {
        StageNumber=1;
        SceneManager.LoadScene("test");
    }
    public void Stage3()
    {
        StageNumber=2;
        SceneManager.LoadScene("test");
    }
    public void Ikusei()
    {
        SceneManager.LoadScene("NeglectfulDevelopment");
    }

}
