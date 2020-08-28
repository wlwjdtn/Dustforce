using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Stage_02_ClearCheck : MonoBehaviour
{
    // ScreenManager 접근
    [SerializeField] private ScreenManager Screen_Instance;

    // 트리거 온/오프
    private bool _TriggerSwitch;

    // 오브젝트 Box Collider
    private BoxCollider2D _ClearCheck_Collider;

    private void Awake() {
        if (Screen_Instance == null) {
            GetComponent<ScreenManager>();
        }
    }

    private void Start() {
        _ClearCheck_Collider = GetComponent<BoxCollider2D>();

    }

    private void Update() {
        PreStage_Trigger();
    }

    private void PreStage_Trigger() {
        if (Input.GetKeyDown(KeyCode.UpArrow) && _TriggerSwitch) {
            // GameManager.GetInstance.Set_SceneName("GameScene_Stage01");
            // GameManager.GetInstance.Set_TimeSet(true);
            // ScreenManager._Instance.PreviousScene();
        }
    }

    // 스테이지 조건을 만족 후 TriggerOn
    // private void Clear_Trigger() {
    //     // 임시적으로 설정
    //     if (Input.GetKeyDown(KeyCode.UpArrow) && _TriggerSwitch) {
    //         Debug.Log("Clear");
    //         Screen_Instance.NextScene("GameScene_Stage02");
    //         _TriggerSwitch = false;
    //     }
    // }

    // 트리거 설정
    private void OnTriggerStay2D(Collider2D collision) {

        // Player 와 접촉을 했을 경우?
        if (collision.transform.tag == "DustGirl") {
            _TriggerSwitch = true;
        }
    }
}
