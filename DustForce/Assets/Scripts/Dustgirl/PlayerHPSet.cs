using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPSet: MonoBehaviour {

    [HideInInspector] Bear_Info bear_Info;
    private GameObject player;
    private SpriteRenderer playerRender;

    // DG HP, Def
    private int dgCurHP = 100;
    private int dgPreHP;
    private int dgDef = 5;

    // 받은 피해량
    private int damageReceived;
    // 총 피해량
    private int totalDamage;

    // 무적 판정
    private bool invincOn;

    // 시작적 표현을 제어하기 위한 제어값
    private float blinkCount;

    // 짧은무적 시간
    private float timeInvinc;

    // HP bar 를 조작할 슬라이더
    private Slider _HpSlider;

    private void Start() {
        bear_Info = gameObject.AddComponent<Bear_Info>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerRender = GameObject.Find("DustGirl").GetComponent<SpriteRenderer>();
        _HpSlider = GameObject.Find("Slider").GetComponent<Slider>();

        dgPreHP = dgCurHP;
        _HpSlider.maxValue = dgCurHP;
    }
    // HP Calculation
    private IEnumerator HPCalculation() {
        Debug.Log("1");
        // 총 피해량
        totalDamage = damageReceived - dgDef;
        // 현재 HP
        dgCurHP = dgPreHP - totalDamage;
        // 이전 HP : 현재 HP 에서 받은 피해량 계산
        dgPreHP = dgCurHP;
        // 실시간 HPUI 관리
        _HpSlider.value = dgCurHP;
        // 짧은무적 적용
        invincOn = true;

        if (invincOn)
            StartCoroutine(Invincibility());
        else {
            StopCoroutine(Invincibility());
            StopCoroutine(Blink());
        }

        // HP 가 '0' 일 경우
        if (dgCurHP == 0) {
            // 캐릭터 'DieAnimation 실행' -> 시킬려 했으나 Die Sprite 가 없어서 그냥 Destory 로 대체
            Destroy(player);
            // 게임을 중지시킵니다

            // 게임 씬을 GameOverSecne 으로 전환

        }

        
        yield return null;
    }
    // Invincibility
    private IEnumerator Invincibility() {
        Debug.Log("2");
        // 시각적 효과 표현 실행
        StartCoroutine(Blink());
        yield return null;
    }
    // 무적 시 시각적 표현
    private IEnumerator Blink() {
        Debug.Log("3");
        blinkCount = 0;
        while (blinkCount < 10) {
            if (blinkCount % 2 == 0)
                playerRender.color = new Color32(180, 180, 180, 255);
            else
                playerRender.color = new Color32(255, 255, 255, 255);

            yield return new WaitForSeconds(0.2f);
            blinkCount++;
        }
        invincOn = false;
        yield return null;
    }

    // Trigger
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Bear_Hit") {
            invincOn = true;
            damageReceived = bear_Info.Atk;

            StartCoroutine(HPCalculation());
        }
    }
}