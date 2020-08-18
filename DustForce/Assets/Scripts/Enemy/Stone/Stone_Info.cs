using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone_Info : EnemyInfomation {

    int HP = 50;
    int Atk = 20;
    int Def = 10;

    protected override void EnemyHP(int hp) {
        hp = HP;
        // Debug.Log("돌 체력은 " + hp + " 입니다.");
    }

    protected override void EnemyAtk(int atk) {
        atk = Atk;
        // Debug.Log("돌 공격력은 " + atk + " 입니다.");
    }

    protected override void EnemyDef(int def) {
        def = Def;
        // Debug.Log("돌 방어력은 " + def + " 입니다.");
    }
}