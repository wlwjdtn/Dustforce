using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfomation : MonoBehaviour {

    readonly int HP = 0;
    readonly int Atk = 0;
    readonly int Def = 0;

    // Enemy 업데이트
    public void StateUpdate() {
        EnemyHP(HP);
        EnemyAtk(Atk);
        EnemyDef(Def);
    }

    protected virtual void EnemyHP(int hp) {
        Debug.Log("적 체력입니다.");
    }
    protected virtual void EnemyAtk(int atk) {
        Debug.Log("적 공격력 입니다.");
    }
    protected virtual void EnemyDef(int def) {
        Debug.Log("적 방어력 입니다.");
    }
}