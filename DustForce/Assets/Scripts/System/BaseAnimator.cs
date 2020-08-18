using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAnimator : MonoBehaviour {

    // 중력값 보정
    private bool _IsGround;
    // 오차검사값
    private bool _LandCheck;
    // 공격(Launch) 검사값
    [SerializeField]private bool _AtkLaunch_Check;
    // 공격(GroundStk) 검사값
    [SerializeField] private bool _AtkGroundStk1_Check;
    // 점프검사값
    [SerializeField] private bool _JumpCheck;

    // 오차값
    private float _ErrorValue;
    public int _AtkCount;

    #region DustGirl Anim
    // DustGirl Animator 접근
    public Animator _DustAnimator;
    // DustGirl AnimFx 접근
    private Animator _FxAnim;
    // DustGirl Renderer 접근
    private SpriteRenderer _DustGirlRender;
    #endregion

    #region AnimClip Length
    // DustGirl AnimationLength
    [SerializeField] private AnimationClip _AnimClip;

    // DustGirl AnimationWaitTime
    private float C_WaitingTime;
    // DustGirl Launch CoolDown Time
    private float C_LaunchTime;
    // DustGirl GroundStk1 CoolDown Time
    private float C_GroundStkTime;

    #endregion

    #region fx_DustGirl
    private GameObject  fx_Launch;
    private GameObject  fx_Stk1;
    #endregion

    private void Start() {
        _AtkCount = 2;

        _DustGirlRender = GameObject.Find("DustGirl").GetComponent<SpriteRenderer>();
        _DustAnimator = GameObject.Find("DustGirl").GetComponent<Animator>();

        // fx
        fx_Launch = GameObject.Find("Fx_Launch");
        fx_Stk1   = GameObject.Find("Fx_GroundStk1");
    }

    public void DustGirlMoveAnim(Vector2 moveinput, Controller2D colliderInfo) {

        /// 이동 애니메이션
        Vector2 input = moveinput;
        Controller2D controller = colliderInfo;
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
            else {
                fx_Launch.SetActive(true);
            }
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
    public float Get_LaunchCoolTime  { get { return C_LaunchTime; }}
    public bool  Get_LandCheck       { get { return _LandCheck; }}
}