using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_FlipX_Fx: MonoBehaviour {

    private GameObject Enemy;
    private SpriteRenderer bear;
    private SpriteRenderer myRender;

    // 오브젝트가 활성화 되면 실행
    private void OnEnable() {

        // 태그가 플레이어인 오브젝트를 찾습니다.
        Enemy = GameObject.FindGameObjectWithTag("Bear");
        // enemy 의 SpriteRenderer 에 접근합니다.
        bear = Enemy.GetComponent<SpriteRenderer>();
        // 자기 랜더러
        myRender = GetComponent<SpriteRenderer>();

        // 플레이어가 왼쪽을 바라볼 경우?
        if (bear.flipX == true) {
            transform.localPosition = new Vector2(-(Mathf.Abs(transform.localPosition.x * 1)), transform.localPosition.y);
            myRender.flipX = true;
        }
        else if (bear.flipX == false) {
            transform.localPosition = new Vector2(Mathf.Abs(transform.localPosition.x * 1), transform.localPosition.y);
            myRender.flipX = false;
        }
    }
}