using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRobot : MonoBehaviour
{
    public int hp = 100;
    public bool IsAlive()
    {
        return hp > 0;
    }
    public virtual void GetDamage(int dmg)
    {
        hp -= dmg;
        if (!IsAlive())
        {
            Die();
        }
    }
    public virtual void Die()
    {
        Destroy(this.gameObject);
    }
    public virtual void OpenFire()
    {

    }

}
