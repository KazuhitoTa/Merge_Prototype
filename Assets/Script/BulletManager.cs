using System.Collections.Generic;
using UnityEngine;


public class BulletManager : MonoBehaviour
{
    [SerializeField] UnitStatusSO unitStatusSO;
    private List<Bullet> _bullet = new List<Bullet>();

    private static BulletManager _instance;
    public static BulletManager Instance
    {
        get { return _instance; }
    }

    public void BulletAwake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    public Bullet CreateBullet(int unitNumber, Vector2 pos)
    {
        var obj = Instantiate(unitStatusSO.unitStatusList[unitNumber].BulletModel, pos, Quaternion.identity);

        var bullet = obj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.Init();

            _bullet.Add(bullet);
            return bullet;
        }

        return null;
    }

    public void RemoveBullet(Bullet bullet)
    {
        _bullet.Remove(bullet);
        Destroy(bullet.gameObject);
    }

    public void BulletUpdate()
    {
        for (int i = _bullet.Count - 1; i >= 0; i--)
        {
            _bullet[i].ManagedUpdate();
        }
    }

}
