using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear_Hitbox : MonoBehaviour
{
    // 방어력에 접근하기 위한 매개체
    [HideInInspector] Bear_Info bear_Info;
    // 플레이어 공격력에 접근하기 위한 매개체
    ContinuousAttack player_Damage;

    GameObject bear;

    // 받은 피해량
    private int damageReceived;
    // 현재 HP
    private int curHP;
    // 이전 HP
    private int preHP;
    // 총 피해량
    private int totalDamage;

    private void Start() {
        player_Damage = GameObject.Find("DustGirl").GetComponent<ContinuousAttack>();
        bear = GameObject.Find("Bear");

        // 정보 초기화
        OnReset();
    }

    public void OnReset() {  
        // 스크립트 찾기
        bear_Info = gameObject.AddComponent<Bear_Info>();

        // 초기화
        curHP = bear_Info.HP;
        preHP = curHP;
    }

    public void OnEnable() {
        if(bear != null) {
            if(bear.activeSelf == true) {
                Debug.Log("실행중!");
                curHP = bear_Info.HP;
            }
        }
        else {
            return;
        }
    }

    // 총 피해량 계산 -> totalDamage = damageRecived - enemyinfo_Def
    private IEnumerator DamageCalculation() {

        // 체력이 '0' 이 아닐경우?
        // 총 피해량
        totalDamage = damageReceived - bear_Info.Def;

        // 총 피해량 예외처리
        if (totalDamage < 0) {
            totalDamage = 0;
        }

        // 체력 감소 (현재 체력 = 이전 체력 - 총 피해량)
        curHP = preHP - totalDamage;

        // 이전 체력을 현재 체력으로 전환
        preHP = curHP;
        Debug.Log("곰의 남은 체력 : " + curHP);

        if (curHP == 0) {
            Debug.Log("사망했습니다.");
            bear.SetActive(false);
            preHP = bear_Info.HP;
        }

        // HP 예외처리
        if (curHP < 0 || preHP < 0) { curHP = 0; preHP = 0; }
        yield return null;
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.tag == "Launch_Hit") {
            damageReceived = player_Damage._FristAtk;
            StartCoroutine(DamageCalculation());
        }
    }
}