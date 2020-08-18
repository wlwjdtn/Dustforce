using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear_RecognitionDistance: MonoBehaviour {
    public enum EnemyAnimState { IDLE, WALK, ATTACK }
    // Bear_Atk_Fx Inst.
    public GameObject Bear_AtkFx_Prefabs;

    // 인식범위내에 들어왔을 경우 플레이어 추적
    private Rigidbody2D rigid;
    private Transform target;

    // BearObject;
    private GameObject bear;
    private Animator anim;
    private SpriteRenderer render;

    [SerializeField] [Range(1f, 4f)] float moveSpeed = 3f;
    [SerializeField] [Range(0f, 3f)] float contactDistance = 1f;

    // 플레이를 추격중인지 검사하는 변수
    public bool follow;
    // 공격 끝지점
    public bool endAtk;

    // 플레이어와의 간격
    public Vector2 spacing;

    // 공격딜레이
    private float attackDelay;

    // 공격 리셋 타임
    private float atkResetTime;

    private void Start() {
        bear = GameObject.FindGameObjectWithTag("Bear");
        rigid = GameObject.FindGameObjectWithTag("Bear").GetComponent<Rigidbody2D>();
        anim = GameObject.FindGameObjectWithTag("Bear").GetComponent<Animator>();
        render = GameObject.FindGameObjectWithTag("Bear").GetComponent<SpriteRenderer>();

        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update() {
    }

    public float MoveDuration(EnemyAnimState animState) {
        string name = string.Empty;
        switch (animState) {
            case EnemyAnimState.IDLE:
                name = "B_Idle";
                break;

            case EnemyAnimState.WALK:
                name = "B_Walk";
                break;

            case EnemyAnimState.ATTACK:
                name = "B_Atk";
                break;

            default:
                return 0;
        }

        float animPlayTime = 0;
        RuntimeAnimatorController animClipLength = anim.runtimeAnimatorController;
        for (int i = 0; i < animClipLength.animationClips.Length; i++) {
            if (animClipLength.animationClips[i].name == name) {
                animPlayTime = animClipLength.animationClips[i].length;
            }
        }
        return animPlayTime;
    }

    public void FollowTarget() {
        if (Vector2.Distance(bear.transform.position, target.position) > contactDistance + 0.1f && follow && !endAtk) {
            bear.transform.position = Vector2.MoveTowards(bear.transform.position, target.position, moveSpeed * Time.deltaTime);
            anim.Play("B_Walk");
                Bear_AtkFx_Prefabs.SetActive(false);
        }
        else {
            rigid.velocity = Vector2.zero;
            // 공격 애니메이션 실행
            anim.Play("B_Atk");
            // 공격 딜레이 ( 0.76 )
            attackDelay = MoveDuration(EnemyAnimState.ATTACK);
            // 다음 공격 재사용 시간
            atkResetTime += Time.deltaTime;
            // 공격시간동안 움직임 금지
            endAtk = true;

            // 다음공격을 할수 있다면?
            if (atkResetTime >= attackDelay) {
                // 움직임 제어
                endAtk = false;
                // 재사용 시간 리셋
                atkResetTime = 0.0f;
            }
        }

        // 'Player' 와 'Creater' 의 간격
        spacing = bear.transform.position - target.position;
        if (spacing.x < 0) {
            render.flipX = false;
        }
        else if (spacing.x > 0) {
            render.flipX = true;
        }
    }

    // 인식거리에 "player' 가 들어왔을 경우
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            Debug.Log("공격 범위내에 목표를 포착했다.");
            follow = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            Debug.Log("공격 범위를 벗어났다.");
            follow = false;
            Bear_AtkFx_Prefabs.SetActive(false);
        }
    }
}