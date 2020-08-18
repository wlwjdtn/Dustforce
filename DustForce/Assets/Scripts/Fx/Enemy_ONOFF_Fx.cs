using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Enemy_ONOFF_Fx: MonoBehaviour {

    // Bear Fx 오브젝트 활성 및 비활성화
    public GameObject bearAtk_Fx;

    public void Atk_Fx_On() {
        bearAtk_Fx.SetActive(true);
    }

    public void Atk_Fx_Off() {
        bearAtk_Fx.SetActive(false);
    }
}