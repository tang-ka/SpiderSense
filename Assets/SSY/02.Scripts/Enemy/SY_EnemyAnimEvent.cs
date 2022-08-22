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
        //���� Hit�� �Ǵ� ����
        enemy.OnEnemyAttackHit();

    }
    public void OnEnemyAttackFinished()
    {
        //���� �ִϸ��̼��� ����Ǵ� ����
        enemy.OnEnemyAttackFinished();
    }

}
