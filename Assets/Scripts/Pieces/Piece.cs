using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


[System.Serializable]
public class Piece
{
    public int Number;

    public int X;
    public int Y;

    public bool CreateAdded;

    public List<int> Numbers = new List<int>{ 1, 2, 3, 4};

    public GameObject SelectObject;

    public MainSpriteControl mainSpriteControl;

    public bool PieceControl;

    public void SelectNumber(int selectNumber = -5)
    {
        if (SelectObject != null)
        {
            SelectObject.gameObject.SetActive(false);
        }
        CreateAdded = true;
        if (selectNumber == -5)
        {
            Number = Numbers[Random.Range(0, Numbers.Count)];
            SelectObject = ObjectPool.Instance.GetPooledObject(Number);
            SelectObject.transform.position = new Vector3(X, Y, 0f);
            SelectObject.SetActive(true);
        }
        else
        {
            Number = selectNumber;
            SelectObject = ObjectPool.Instance.GetPooledObject(Number);
            SelectObject.transform.position = new Vector3(X, Y, 0f);
            SelectObject.SetActive(true);
        }
        SelectObject.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InOutBack).From();
    }

    public void CheckEdgeTypes()
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if ((i == 0 && (j == -1 || j == 1)) || ((i == 1 || i == -1) && j == 0))
                {
                    if (CreatePieces.Instance.PieceList.GetLength(0) > X + i && X + i >= 0 && CreatePieces.Instance.PieceList.GetLength(1) > Y + j && Y + j >= 0)
                    {
                        CreatePieces.Instance.PieceList[X + i, Y + j].Numbers.Remove(Number);
                    }
                }
            }
        }
    }
}
