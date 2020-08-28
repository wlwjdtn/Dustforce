using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousAttack: MonoBehaviour {

    // 캐릭터 애니메이션 권한
    private Animator _Anim;
    // 캐릭터 공격력
    public int _FristAtk;
    public int _SecondAtk;
    public int _ThirdAtk;


    // 전체 Fx 오브젝트
    [SerializeField] private GameObject[] _FxObject;
    // [SerializeField] private GameObject[] _DisableFxObject;
    // Fx 오브젝트의 부모
    private GameObject _FxParent;
    // 캐릭터 Fx 오브젝트 권한
    private GameObject _FxObj;
    // 캐릭터 공격값
    private bool _IsAtk;

    // '1' 타 공격권한을 가지고 있는 검사값
    [SerializeField] private bool FristAtk;
    // '2' 타 공격권한을 가지고 있는 검사값
    [SerializeField] private bool SecondAtk;
    // '3' 타 공격권한을 가지고 있는 검사값
    [SerializeField] private bool ThirdAtk;

    [System.Obsolete]
    private void Start() {
        // 애니메이션 권한 접근
        _Anim = GameObject.Find("DustGirl").GetComponent<Animator>();
        // Fx 부모 오브젝트
        _FxParent = GameObject.Find("Fx");
        // Fx 전체 오브젝트를 배열에 담음
        _FxObject = new GameObject[_FxParent.transform.GetChildCount()];

        // 한 번 초기화
        FristAtk = true;

        // 캐릭터 공격력
        _FristAtk = 10;
        _SecondAtk = 15;
        _ThirdAtk = 20;

        // Fx 오브젝트를 순회합니다.
        for (int i = 0; i < _FxObject.Length; i++) {
            if (_FxObject[i] == null) {
                _FxObject[i] = _FxParent.transform.GetChild(i).gameObject;
            }
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.X)) {
            _IsAtk = true;
        }
        else {
            if (_IsAtk)
                _IsAtk = false;
        }
        _Anim.SetBool("IsAtk", _IsAtk);
        JudgmentAtk();
    }

    // 이 곳에서 연속 공격을 조작합니다.
    private void JudgmentAtk() {
        // 첫번 째 공격을 조작합니다.
        if (Input.GetKeyDown(KeyCode.X) && FristAtk == true) {
            // 애니메이션 실행
            _Anim.Play("DustGirl_Launch");
        }
        // 두번 째 공격을 조작합니다.
        else if (Input.GetKeyDown(KeyCode.X) && SecondAtk == true) {
            // 애니메이션 실행
            _Anim.Play("DustGirl_GroundStrike1");
        }
        // 세번 째 공격을 조작합니다.
        else if (Input.GetKeyDown(KeyCode.X) && ThirdAtk == true) {
            // 애니메이션 실행
            _Anim.Play("DustGirl_GroundStrike2");
        }
    }

    // 키 프레임 실행 중 조작 활성상태
    private void SecondCheckOn() {
        SecondAtk = true;
        FristAtk = false;
        ThirdAtk = false;
    }
    private void SecondCheckOff() {
        SecondAtk = false;
    }
    
    private void ThirdCheckOn() {
        ThirdAtk = true;
        FristAtk = false;
        SecondAtk = false;
    }
    private void ThirdCheckOff() {
        ThirdAtk = false;
    }

    // 키 프레임 리셋
    private void ResetAnim() {
        FristAtk = true;
        SecondAtk = false;
        ThirdAtk = false;
        _Anim.Play("DustGirl_Idle");
    }

    // Fx 오브젝트 활성/비활성 조작
    private void Launch_Fx_On() {
        foreach (GameObject @object in _FxObject) {
            if (@object.name == "Launch_Fx") {
                @object.SetActive(true);
            }
        }
    }
    private void Launch_Fx_Off() {
        foreach (GameObject @object in _FxObject) {
            if (@object.name == "Launch_Fx") {
                @object.SetActive(false);
            }
        }
    }
    private void Ground01_Fx_On() {
        foreach (GameObject @object in _FxObject) {
            if (@object.name == "Ground01_Fx") {
                @object.SetActive(true);
            }
        }
    }
    private void Ground01_Fx_Off() {
        foreach (GameObject @object in _FxObject) {
            if (@object.name == "Ground01_Fx") {
                @object.SetActive(false);
            }
        }
    }
    private void Ground02_Fx_On() {
        foreach (GameObject @object in _FxObject) {
            if (@object.name == "Ground02_Fx") {
                @object.SetActive(true);
            }
        }
    }
    private void Ground02_Fx_Off() {
        foreach (GameObject @object in _FxObject) {
            if (@object.name == "Ground02_Fx") {
                @object.SetActive(false);
            }
        }
    }
}

