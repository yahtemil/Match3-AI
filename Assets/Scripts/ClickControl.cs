using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClickControl : MonoBehaviour
{
    int X;
    int Y;
    Vector3 firstMousePos;
    Vector3 endMousePos;
    bool Active = true;

    //t�klan�lan par�an�n ba�langi� bilgileri aliniyor
    public void OnMouseDown()
    {
        if (GameManager.Instance.GameState != GameManager.GameStates.Play)
        {
            return;
        }
        Active = true;
        firstMousePos = Input.mousePosition;
        X = (int)(transform.position.x);
        Y = (int)(transform.position.y);
    }
    // Burada yapilan i�leme g�re hareket sistemi kontrol edilip ba�latiliyor (Yukar�,a�a��,sa�a,sola)
    public void OnMouseDrag()
    {
        if (!Active || GameManager.Instance.GameState != GameManager.GameStates.Play)
        {
            return;
        }
        endMousePos = Input.mousePosition;
        float distance = (endMousePos - firstMousePos).magnitude;
        if (distance >= 5f)
        {
            if (endMousePos.x > firstMousePos.x)
            {
                if (Mathf.Abs(endMousePos.x - firstMousePos.x) > Mathf.Abs(endMousePos.y - firstMousePos.y))
                {
                    CreatePieces.Instance.MoveSystem(X,Y,1, 0);
                }
            }
            else
            {
                if (Mathf.Abs(endMousePos.x - firstMousePos.x) > Mathf.Abs(endMousePos.y - firstMousePos.y))
                {
                    CreatePieces.Instance.MoveSystem(X,Y,-1, 0);
                }
            }
            if (endMousePos.y > firstMousePos.y)
            {
                if (Mathf.Abs(endMousePos.y - firstMousePos.y) > Mathf.Abs(endMousePos.x - firstMousePos.x))
                {
                    CreatePieces.Instance.MoveSystem(X,Y, 0, 1);
                }
            }
            else
            {
                if (Mathf.Abs(endMousePos.y - firstMousePos.y) > Mathf.Abs(endMousePos.x - firstMousePos.x))
                {
                    CreatePieces.Instance.MoveSystem(X,Y, 0, -1);
                }
            }
            Active = false;
        }
    }
}
