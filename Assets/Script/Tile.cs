using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public void Init(int x, int y,int z)
    {
        transform.position = new Vector3(x,y,z);
    }
}
