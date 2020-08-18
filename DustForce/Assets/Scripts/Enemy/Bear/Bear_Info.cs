using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear_Info : EnemyInfomation {

    public int HP = 30;
    public int Atk = 10;
    public int Def = 5;

    protected override void EnemyHP(int hp) {
        hp = HP;
        // Debug.Log("곰의 체력은 " + hp + " 입니다.");
    }
    protected override void EnemyAtk(int atk) {
        atk = Atk;
        // Debug.Log("곰의 공격력은 " + atk + " 입니다.");
    }
    protected override void EnemyDef(int def) {
        def = Def;
        // Debug.Log("곰의 방어력은 " + def + " 입니다.");
    }
}