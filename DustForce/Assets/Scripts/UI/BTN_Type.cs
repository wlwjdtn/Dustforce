using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static MainUISystem;
public class BTN_Type: MonoBehaviour {

    private void Start() {
        
    }

    public CanvasGroup mainGroup;
    public CanvasGroup OptionGroup;

    public bool isSound;
    public BTNType currentType;
    public void OnBtnClick() {
        switch (currentType) {
            case BTNType.NEW:
                // 식별자 번호를 설정하고, ScreenManager 내부에서 실행.
                ScreenManager.LoadSceneHandle("GameScene_Stage01", 0);
                break;
            case BTNType.CONTINUE:
                ScreenManager.LoadSceneHandle("GameScene_Stage01", 1);
                break;
            case BTNType.OPTION:
                CanvasGroupOn(OptionGroup);
                CanvasGroupOff(mainGroup);
                break;
            case BTNType.BACK:
                CanvasGroupOn(mainGroup);
                CanvasGroupOff(OptionGroup);
                break;
            case BTNType.SOUND:
                if (isSound) {
                    isSound = !isSound;
                    Debug.Log("소리 끔");
                }
                else {
                    Debug.Log("소리 켬");
                }
                isSound = !isSound;
                break;
            case BTNType.QUIT:
                Application.Quit(); 
                Debug.Log("종료");
                break;
        }
    }
    public void CanvasGroupOn(CanvasGroup cg) {
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }
    public void CanvasGroupOff(CanvasGroup cg) {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts =false;
    }
}