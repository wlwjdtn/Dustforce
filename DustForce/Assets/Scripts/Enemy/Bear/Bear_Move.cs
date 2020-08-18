using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear_Move: MonoBehaviour {

    // 컴퍼넌트 접근
    private SpriteRenderer spt;
    private Animator anim;

    [SerializeField] private Bear_RecognitionDistance recognitionDis;

    #region Bear Move
    // 거리를 루프할 왼쪽, 오른쪽 방향
    private int minDisX;
    private int maxDisX;

    // 움직일 수 있는 거리를 왼쪽 또는, 오른쪽, 멈출 지 설정
    private int randomDis;

    // 이동중
    private bool isMoving;
    // 대기중
    private bool isWaiting;

    // 행동패턴 시간
    private int randomWaitingTime;

    // 이동값
    private Vector2 velocity;
    #endregion

    private void Start() {
        spt = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // 한번 초기화
        randomDis = 0;

        StartCoroutine(PartternRandomTime());
        StartCoroutine(RandomDisValue());
    }

    private void Update() {

        if(recognitionDis.follow) {
            recognitionDis.FollowTarget();
        }
        else {

        PartternTime();
        BearMove();
        BearRenderer();
        AnimPlay();
        }
    }

    // 유지거리에서 반복적으로 왔다갔다 이동
    private IEnumerator RandomDisValue() {
        // Bear 를 기준으로 왼쪽, 오른쪽 결정 (-1, 0, 1)
        minDisX = -1;
        maxDisX = 2;
        // 이동할 랜덤 값
        randomDis = Random.Range(minDisX, maxDisX);
        // n 초마다 함수 실행
        yield return new WaitForSecondsRealtime(randomWaitingTime);
        StartCoroutine(RandomDisValue());
    }

    // 행동시간을 랜덤으로 설정
    private IEnumerator PartternRandomTime() {

        // 다음 행동을 취할때까지의 대기 시간
        randomWaitingTime = Random.Range(3, 7);
        while (true) {
            yield return new WaitForSeconds(randomWaitingTime);
            randomWaitingTime = Random.Range(3, 7);
        }
    }

    private void PartternTime() {
        // 만약 움직이는 중이라면?
        if (randomDis != 0) {
            isMoving = true;
            if (isMoving) isWaiting = false;
        }
        // 만약 대기중이라면?
        else if (randomDis == 0) {
            isWaiting = true;
            if (isWaiting) isMoving = false;
        }
    }
    // 오브젝트의 이동
    private void BearMove() {
        // 컴퍼넌트를 가진 객체의 벡터값
        velocity = new Vector2(transform.position.x + randomDis, transform.position.y);
        // 컴퍼넌트를 가진 객체를 움직임
        transform.position = Vector2.Lerp(transform.position, velocity, 1f * Time.deltaTime);
    }
    // 오브젝트 이미지 방향
    private void BearRenderer() {
        if (randomDis < 0) { spt.flipX = true; }
        else if (randomDis > 0) { spt.flipX = false; }
    }

    // 오브젝트 애니메이션
    private void AnimPlay() {
        if (isMoving) anim.Play("B_Walk");
        if (isWaiting) anim.Play("B_Idle");
    }
}