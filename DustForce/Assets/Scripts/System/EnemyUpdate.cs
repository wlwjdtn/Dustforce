using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUpdate : MonoBehaviour
{
    // 템플릿 메서드로 정보 정렬화
    Stone_Info stn_Info;
    Bear_Info bear_Info;

    // 스테이터스 변화
    Bear_Hitbox bear_Hitbox;

    private void Start() {
        stn_Info = gameObject.AddComponent<Stone_Info>();
        bear_Info = gameObject.AddComponent<Bear_Info>();

        // Enemy State 초기화
        stn_Info.StateUpdate();
        bear_Info.StateUpdate();
    }

    private void Update() {
        // Enemy State Change Update
    }
}