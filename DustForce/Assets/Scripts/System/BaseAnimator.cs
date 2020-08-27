using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAnimator : MonoBehaviour {

    // 오차검사값
    private bool _LandCheck;
    // 공격(Launch) 검사 값
    [SerializeField] private bool _AtkLaunch_Check;
    // 벽을 타고 위로 올라가는지에 대한 검사 값
    [SerializeField] private bool _WallRun_Check;

    // Land Anim 실행 후 다음 애니메이션 실행 시킬 오차값
    private float _ErrorValue;

    #region DustGirl Anim
    // DustGirl Animator 접근
    public Animator _DustAnimator;
    // DustGirl Renderer 접근
    private SpriteRenderer _DustGirlRender;
    #endregion

    #region AnimClip Length
    // DustGirl AnimationLength
    [SerializeField] private AnimationClip _AnimClip;
    #endregion

    #region fx_DustGirl
    private GameObject  fx_Launch;
    #endregion

    private void Start() {
        if (_DustAnimator != null) { return; }
        else { _DustAnimator = GameObject.Find("DustGirl").GetComponent<Animator>(); }

        if(_DustGirlRender != null) { return; } 
        else { _DustGirlRender = GameObject.Find("DustGirl").GetComponent<SpriteRenderer>(); }

        // fx
        fx_Launch = GameObject.Find("Fx_Launch");
    }

    public void DustGirlMoveAnim(Vector2 moveInput, Controller2D colliderInfo) {

        /// 이동 애니메이션
        Vector2 input = moveInput;
        bool isGround = colliderInfo._ColliderInfo._Below;

        // 땅에 닿고 있으면?
        if (isGround) {
            // 중력값 보정
            input.y = 0;
            _LandCheck = true;
            if (_LandCheck) { 
                _ErrorValue += Time.deltaTime;
                if (_ErrorValue > 0.4f) {
                    _DustAnimator.SetBool("IsLand", _LandCheck);
                    _ErrorValue = 0.0f;
                }
            }
            // 착지 후 fx 켜기
            else fx_Launch.SetActive(true);
        }
        // 공중에 있으면?
        else if (!isGround) {
            _LandCheck = false;
            _DustAnimator.SetBool("IsLand", _LandCheck);
            _ErrorValue = 0.0f;
        }
        // Left, Right, Hold
        _DustAnimator.SetInteger("ValueX", (int)input.x);
        // Jump
        _DustAnimator.SetInteger("ValueY", (int)input.y);
        // IsGround?
        _DustAnimator.SetBool("IsGround", isGround);

        // AnimfilpX
        if (input.x < 0 && !_AtkLaunch_Check) {
            _DustGirlRender.flipX = true;
        } else if (input.x > 0 && !_AtkLaunch_Check) {
            _DustGirlRender.flipX = false;
        }
    }
}