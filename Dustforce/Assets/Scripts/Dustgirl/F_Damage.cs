using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_Damage: MonoBehaviour {
    private ContinuousAttack Damage;

    public int DamageValue;

    private void OnEnable() {
        Damage = gameObject.AddComponent<ContinuousAttack>();
        Debug.Log(DamageValue);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Bear") {
            DamageValue = Damage._FristAtk;
        }
    }

    // 데미지에 대한 게터, 세터
    private int Get_Damage { get { return DamageValue; } }
    private void Set_Damage(int damage) { DamageValue = damage; }

}
