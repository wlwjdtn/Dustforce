using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro_Inputkey: MonoBehaviour {

    // Input "A" -> Lobby Scene
    private KeyCode inputkey = KeyCode.Space;
    private int StopContinueKey;

    private void FixedUpdate() {
        MainInput();
    }

    private void OnEnable() {
        StopContinueKey = 0;
    }

    private void MainInput() {
        if (Input.GetKey(inputkey) && StopContinueKey < 1) {
            SceneManager.LoadScene("LobbyScene");
            StopContinueKey++;
        }
    }
}