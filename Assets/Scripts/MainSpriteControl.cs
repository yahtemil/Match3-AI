using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class MainSpriteControl : MonoBehaviour
{
    [SerializeField]
    public Piece selectPiece;

    List<Piece> LeftRightList = new List<Piece>();
    List<Piece> UpDownList = new List<Piece>();
    int leftRightCounter = 0;
    int upDownCounter = 0;

    public bool allMapControlActive;

    public void AllControl(int X, int Y, int SelectNumber)
    {
        leftRightCounter = 0;
        upDownCounter = 0;
        UpDownList = new List<Piece>();
        LeftRightList = new List<Piece>();
        int selectNumber = CreatePieces.Instance.PieceList[X, Y].Number;
        UpDownList.Add(CreatePieces.Instance.PieceList[X, Y]);
        LeftRightList.Add(CreatePieces.Instance.PieceList[X, Y]);
        leftRightCounter++;
        upDownCounter++;
        LeftControl(X, Y, selectNumber);
        RightControl(X, Y, selectNumber);
        UpControl(X, Y, selectNumber);
        DownControl(X, Y, selectNumber);
        ControlTiming();
    }

    public bool LeftRightCounterControl()
    {
        if (leftRightCounter >= 3)
        {
            return true;
        }
        return false;
    }

    public bool UpDownCounterControl()
    {
        if (upDownCounter >= 3)
        {
            return true;
        }
        return false;
    }

    void ControlTiming()
    {
        //yield return new WaitForSeconds(0f);
        if (leftRightCounter >= 3)
        {
            foreach (var item in LeftRightList)
            {
                CloseObject(item);
                UpDownRightLeft(item);
            }
            CreatePieces.Instance.Invoke("AllMapControl",0.28f);
        }
        if (upDownCounter >= 3)
        {
            int valueMinY = 100;
            int valueMaxY = 0;
            int valueX = 5;
            foreach (var item in UpDownList)
            {
                if (item.Y < valueMinY)
                {
                    valueMinY = item.Y;
                }
                if (item.Y > valueMaxY)
                {
                    valueMaxY = item.Y;
                }
                valueX = item.X;
                CloseObject(item);
            }
            UpDownUpDown(valueMinY,valueMaxY, valueX);
            CreatePieces.Instance.Invoke("AllMapControl", 0.28f);
        }
    }

    private void CloseObject(Piece item)
    {
        item.Number = -1;
        item.SelectObject?.gameObject.SetActive(false);
        item.SelectObject = null;
        
    }

    public GameObject CreateNewObject(int posX,int randomValue)
    {
        GameObject g = ObjectPool.Instance.GetPooledObject(randomValue);
        g.transform.position = new Vector3(posX, CreatePieces.Instance.PieceList.GetLength(1), 0f);
        g.SetActive(true);
        return g;
    }

    public void UpDownRightLeft(Piece item)
    {
        int total = CreatePieces.Instance.PieceList.GetLength(1) - item.Y;
        for (int i = item.Y; i < item.Y + total - 1 ; i++)
        {
            CreatePieces.Instance.PieceList[item.X, i].SelectObject = CreatePieces.Instance.PieceList[item.X, i + 1].SelectObject;
            CreatePieces.Instance.PieceList[item.X, i].Number = CreatePieces.Instance.PieceList[item.X, i + 1].Number;
            CreatePieces.Instance.PieceList[item.X, i].SelectObject.transform.DOKill();
            CreatePieces.Instance.PieceList[item.X, i].SelectObject.transform.DOMoveY(i, 0.25f);
        }
        int RandomValue = Random.Range(1, 5);
        CreatePieces.Instance.PieceList[item.X, CreatePieces.Instance.PieceList.GetLength(1) - 1].SelectObject = CreateNewObject(item.X, RandomValue);
        CreatePieces.Instance.PieceList[item.X, CreatePieces.Instance.PieceList.GetLength(1) - 1].Number = RandomValue;
        CreatePieces.Instance.PieceList[item.X, CreatePieces.Instance.PieceList.GetLength(1) - 1].SelectObject.transform.DOKill();
        CreatePieces.Instance.PieceList[item.X, CreatePieces.Instance.PieceList.GetLength(1) - 1].SelectObject.transform.DOMoveY(CreatePieces.Instance.PieceList.GetLength(1) - 1, 0.25f);
    }

    public void UpDownUpDown(int minY,int maxY,int posX)
    {
        int total = CreatePieces.Instance.PieceList.GetLength(1) -1 - maxY;
        int endPoint = minY+total;
        for (int i = minY; i < minY + total; i++)
        {
            maxY++;
            CreatePieces.Instance.PieceList[posX, i].SelectObject = CreatePieces.Instance.PieceList[posX, maxY].SelectObject;
            CreatePieces.Instance.PieceList[posX, i].Number = CreatePieces.Instance.PieceList[posX, maxY].Number;
            CreatePieces.Instance.PieceList[posX, i].SelectObject.transform.DOKill();
            CreatePieces.Instance.PieceList[posX, i].SelectObject.transform.DOMoveY(i, 0.25f);
        }
        for (int i = endPoint; i < CreatePieces.Instance.PieceList.GetLength(1); i++)
        {
            int RandomValue = Random.Range(1, 5);
            CreatePieces.Instance.PieceList[posX, i].SelectObject = CreateNewObject(posX, RandomValue);
            CreatePieces.Instance.PieceList[posX, i].Number = RandomValue;
            CreatePieces.Instance.PieceList[posX, i].SelectObject.transform.DOKill();
            CreatePieces.Instance.PieceList[posX, i].SelectObject.transform.DOMoveY(i, 0.25f);
        }
    }

    public void LeftControl(int X, int Y, int selectNumber)
    {
        if (X - 1 >= 0)
        {
            if (CreatePieces.Instance.PieceList[X - 1, Y].Number == selectNumber)
            {
                LeftControl(X - 1, Y, selectNumber);
                leftRightCounter++;
                LeftRightList.Add(CreatePieces.Instance.PieceList[X - 1, Y]);
            }
        }
    }

    public void RightControl(int X, int Y, int selectNumber)
    {
        if (CreatePieces.Instance.PieceList.GetLength(0) > X + 1)
        {
            if (CreatePieces.Instance.PieceList[X + 1, Y].Number == selectNumber)
            {
                RightControl(X + 1, Y, selectNumber);
                leftRightCounter++;
                LeftRightList.Add(CreatePieces.Instance.PieceList[X + 1, Y]);
            }
        }
    }

    public void UpControl(int X, int Y, int selectNumber)
    {
        if (CreatePieces.Instance.PieceList.GetLength(1) > Y + 1)
        {
            if (CreatePieces.Instance.PieceList[X, Y + 1].Number == selectNumber)
            {
                UpControl(X, Y + 1, selectNumber);
                upDownCounter++;
                UpDownList.Add(CreatePieces.Instance.PieceList[X, Y + 1]);
            }
        }
    }

    public void DownControl(int X, int Y, int selectNumber)
    {
        if (Y - 1 >= 0)
        {
            if (CreatePieces.Instance.PieceList[X, Y - 1].Number == selectNumber)
            {
                DownControl(X, Y - 1, selectNumber);
                upDownCounter++;
                UpDownList.Add(CreatePieces.Instance.PieceList[X, Y - 1]);
            }
        }
    }
}
