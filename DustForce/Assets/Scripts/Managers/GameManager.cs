using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class GameManager {
    public static GameManager _Instance;

    public static GameManager GetInstance {
        get {
            if (_Instance == null) {
                _Instance = new GameManager();
            }
            return _Instance;
        }

        private set {
            _Instance = value;
        }
    }
    #region 기능
    // 게임상태 열거 ( 진행중, 부분 정지, 기능 정지 ) 
    private enum Game_State { IN_PROGRESS, PARTIAL_STOP, FUNCTIONAL_SUSPENSION }
    Game_State game_State = Game_State.IN_PROGRESS;

    // 게임 진행중일때...
    private void In_Progress(Game_State state) {
        state = Game_State.IN_PROGRESS;
    }

    // 진행중일때 부분정지...
    private void PARTIAL_STOP(Game_State state) {
        state = Game_State.PARTIAL_STOP;
    }

    // 게임 기능 전체 정지...
    private void FUNCTIONAL_SUSPENSION(Game_State state) {
        state = Game_State.FUNCTIONAL_SUSPENSION;
    }
    #endregion

    // 게터, 세터 정의 선언
    private string _SceneName;
    private bool _TimeSet;
    private bool _TimeClear;


    // 캐릭터 정보
    public void CharacterInfo() {

    }

    // 적 정보
    public void Emeny() {

    }

    // Getter
    public string Get_SceneName { get { return _SceneName; } }
    public bool Get_TimeSet { get { return _TimeSet; } }
    public bool Get_TimeClear { get { return _TimeClear; } }


    // Setter
    public void Set_SceneName(string str) { _SceneName = str; }
    public void Set_TimeSet(bool value) { _TimeSet = value; }
    public void Set_TimeClear(bool value) { _TimeClear = value; }
}