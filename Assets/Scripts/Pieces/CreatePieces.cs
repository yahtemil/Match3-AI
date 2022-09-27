using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CreatePieces : MonoSingleton<CreatePieces>
{
    [SerializeField]
    public Piece[,] PieceList;

    private Piece firstPiece;
    private Piece secondPiece;

    private int AllControlCounter;
    private bool AllControlActive;


    void Awake()
    {
        PieceList = new Piece[GameManager.Instance.XValue, GameManager.Instance.ZValue];
        for (int i = 0; i < PieceList.GetLength(0); i++)
        {
            for (int j = 0; j < PieceList.GetLength(1); j++)
            {
                PieceList[i, j] = new Piece();
                PieceList[i, j].X = i;
                PieceList[i, j].Y = j;
                GameObject g = ObjectPool.Instance.GetPooledObject(0);
                g.transform.position = new Vector3(i, j, 0);
                g.SetActive(true);
                PieceList[i, j].mainSpriteControl = g.GetComponent<MainSpriteControl>();
                PieceList[i, j].mainSpriteControl.selectPiece = PieceList[i,j];
            }
        }
        Invoke("CreateMap", 0.1f);
    }



    public void RestartMap()
    {
        ClearMap();
        CreateMap();
    }

    public void ClearMap()
    {
        for (int i = 0; i < PieceList.GetLength(0); i++)
        {
            for (int j = 0; j < PieceList.GetLength(1); j++)
            {
                PieceList[i, j].CreateAdded = false;
                PieceList[i, j].Numbers = new List<int> { 1, 2, 3, 4 };
            }
        }
    }

    public void CreateMap()
    {
        for (int i = 0; i < PieceList.GetLength(0); i++)
        {
            for (int j = 0; j < PieceList.GetLength(1); j++)
            {
                if (PieceList[i,j].CreateAdded == false)
                {
                    PieceGroupTypes pieceGroupTypes = PieceGroups.Instance.GetPieceGroupType(i, j);
                    if (pieceGroupTypes == null)
                    {
                        //PieceList[i, j].CreateAdded = true;
                        PieceList[i, j].SelectNumber();
                        PieceList[i, j].CheckEdgeTypes();
                    }
                    else
                    {
                        int numberCounter = 0;
                        int number = 0;
                        List<Piece> reservePieceList = new List<Piece>();
                        for (int k = 0; k < pieceGroupTypes.X.Length; k++)
                        {
                            Piece piece = PieceList[i + pieceGroupTypes.X[k], j + pieceGroupTypes.Z[k]];
                            if (numberCounter == 0)
                            {
                                numberCounter++;
                                piece.SelectNumber();
                                number = piece.Number;
                                reservePieceList.Add(piece);
                            }
                            else
                            {
                                if (piece.Numbers.Contains(number) == false)
                                {
                                    piece.SelectNumber();
                                    number = piece.Number;
                                    k = 0;
                                    reservePieceList.Clear();
                                    continue;
                                }
                                else
                                {
                                    piece.SelectNumber(number);
                                }
                                reservePieceList.Add(piece);
                            }
                        }
                        foreach (var item in reservePieceList)
                        {
                            item.CheckEdgeTypes();
                        }
                    }
                }
            }
        }
    }

    public void AllMapControl()
    {
        if (AllControlActive)
        {
            return;
        }
        AllControlActive = true;
        AllControlCounter++;
        StartCoroutine(AllMapTimingControl(AllControlCounter));
    }

    IEnumerator AllMapTimingControl(int counter)
    {
        for (int i = 0; i < PieceList.GetLength(0); i++)
        {
            for (int j = 0; j < PieceList.GetLength(1); j++)
            {
                if (counter != AllControlCounter)
                {
                    AllControlActive = false;
                    yield break;
                }
                PieceList[i, j].mainSpriteControl.AllControl(i, j, PieceList[i, j].Number);
                if (PieceList[i, j].mainSpriteControl.LeftRightCounterControl() || PieceList[i, j].mainSpriteControl.UpDownCounterControl())
                {
                    yield return new WaitForSeconds(0.3f);
                }
            }
        }
        AllControlActive = false;
    }

    public void MoveSystem(int X,int Y,int plusX, int plusY)
    {
        if (PieceList.GetLength(0) > X + plusX && X + plusX >= 0 && PieceList.GetLength(1) > Y + plusY && Y + plusY >= 0)
        {
            firstPiece = PieceList[X, Y];
            secondPiece = PieceList[X + plusX, Y + plusY];
            ChangeTwoPieceBetween();
            secondPiece.SelectObject.transform.DOMove(firstPiece.SelectObject.transform.position, 0.5f);
            firstPiece.SelectObject.transform.DOMove(secondPiece.SelectObject.transform.position, 0.5f).OnComplete(() =>
            {
                secondPiece.mainSpriteControl.AllControl(secondPiece.X, secondPiece.Y, secondPiece.Number);
                if (secondPiece.mainSpriteControl.LeftRightCounterControl() == false && secondPiece.mainSpriteControl.UpDownCounterControl() == false)
                {
                    firstPiece.mainSpriteControl.AllControl(firstPiece.X, firstPiece.Y, firstPiece.Number);
                    if (firstPiece.mainSpriteControl.LeftRightCounterControl() == false && firstPiece.mainSpriteControl.UpDownCounterControl() == false)
                    {
                        secondPiece.SelectObject.transform.DOMove(firstPiece.SelectObject.transform.position, 0.25f);
                        firstPiece.SelectObject.transform.DOMove(secondPiece.SelectObject.transform.position, 0.25f);
                        ChangeTwoPieceBetween();
                    }
                }
            });
        }
    }

    public void ChangeTwoPieceBetween()
    {
        Piece piece = new Piece();
        piece.mainSpriteControl = secondPiece.mainSpriteControl;
        piece.Number = secondPiece.Number;
        piece.SelectObject = secondPiece.SelectObject;
        secondPiece.mainSpriteControl = firstPiece.mainSpriteControl;
        secondPiece.Number = firstPiece.Number;
        secondPiece.SelectObject = firstPiece.SelectObject;
        firstPiece.mainSpriteControl = piece.mainSpriteControl;
        firstPiece.Number = piece.Number;
        firstPiece.SelectObject = piece.SelectObject;
    }
}
