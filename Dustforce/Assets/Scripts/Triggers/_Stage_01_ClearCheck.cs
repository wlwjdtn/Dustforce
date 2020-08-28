using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class _Stage_01_ClearCheck : MonoBehaviour {
// ScreenManager
    [SerializeField] private ScreenManager Screen_Instance;

    // 트리거 온/오프
    private bool _TriggerSwitch;

    private void Awake() {
        if (Screen_Instance == null) {
            GetComponent<ScreenManager>();
        }
    }

    private void Update() {
        Clear_Trigger();
    }

    // 스테이지 조건을 만족 후 TriggerOn
    private void Clear_Trigger() {
        // 임시적으로 설정
        if (Input.GetKeyDown(KeyCode.UpArrow) && _TriggerSwitch) {
            // GameManager.GetInstance.Set_SceneName("GameScene_Stage02");
            // GameManager.GetInstance.Set_TimeSet(true);
            // ScreenManager._Instance.NextScene();
        }
    }

    // 트리거 설정
    private void OnTriggerStay2D(Collider2D collision) {

        // Player 와 접촉을 했을 경우?
        if(collision.transform.tag == "Player") {
            _TriggerSwitch = true;
        }
    }
}