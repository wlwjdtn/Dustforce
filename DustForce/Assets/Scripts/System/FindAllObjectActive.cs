using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindAllObjectActive: MonoBehaviour {

    // 활성화 중인 오브젝트입니다. < Array >
    public static bool AllActiveGameObject(ref GameObject[] gameObjects) {

        // 인자값으로 들어온 배열의 전체 길이.
        int ActiveGameObjectMax = gameObjects.Length;

        // 인자값으로 들어온 배열의 활성화된 오브젝트 개수.
        int ActiveGameObjectCount = gameObjects.Count(obj => obj.activeSelf);

        // 배열 속의 인덱스 값들이 전부 활성화 되어있으면 True 반환.
        return ActiveGameObjectCount == ActiveGameObjectMax;
    }

    // 활성화 중인 오브젝트입니다. < List >
    public static bool AllActiveGameObject(ref List<GameObject> gameObjects) {

        // 인자값으로 들어온 배열의 전체 길이.
        int ActiveGameObjectMax = gameObjects.Count;

        // 인자값으로 들어온 배열의 활성화된 오브젝트 개수.
        int ActiveGameObjectCount = gameObjects.Count(obj => obj.activeSelf);

        // 배열 속의 인덱스 값들이 전부 활성화 되어있으면 True 반환.
        return ActiveGameObjectCount == ActiveGameObjectMax;
    }
}