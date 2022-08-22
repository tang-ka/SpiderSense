using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SY_EnemyAnimEvent : MonoBehaviour
{
    public SY_Enemy enemy;
    void Start()
    {
        enemy = GetComponentInParent<SY_Enemy>();
    }
    public void OnEnemyAttackHit()
    {
        //공격 Hit가 되는 순간
        enemy.OnEnemyAttackHit();

    }
    public void OnEnemyAttackFinished()
    {
        //공격 애니메이션이 종료되는 순간
        enemy.OnEnemyAttackFinished();
    }

}
