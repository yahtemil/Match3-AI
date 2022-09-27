using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [Range(3,10)]
    public int XValue;
    [Range(3, 10)]
    public int ZValue;

    public GameStates GameState;

    private void Start() // Tüm þablon boyutlarýna göre kamera görünümünün ayarlanmasý
    {
        Vector3 CameraXPos = new Vector3(1f + ((3.5f / 7f) * (XValue - 3)), 0.5f + ((2.5f / 7f) * (XValue - 3)), -6f + ((-11f / 7f) * (XValue - 3)));
        Vector3 CameraZPos = new Vector3(1f + ((3.5f / 7f) * (ZValue - 3)), 0.5f + ((2.5f / 7f) * (ZValue - 3)), -6f + ((-11f / 7f) * (ZValue - 3)));
        Vector3 CameraPos = Vector3.zero;
        CameraPos.x = CameraXPos.x > CameraZPos.z ? CameraXPos.x : CameraZPos.x;
        CameraPos.y = CameraXPos.x < CameraZPos.z ? CameraXPos.y : CameraZPos.y;
        CameraPos.z = XValue > ZValue ? CameraXPos.z : CameraZPos.z;
        CameraPos.z -= 3f;
        Camera.main.transform.position = CameraPos;
    }

    public void ReplayButton()
    {
        SceneManager.LoadScene(0);
    }

    public enum GameStates
    {
        Start,
        Play,
        Wait
    }

}
