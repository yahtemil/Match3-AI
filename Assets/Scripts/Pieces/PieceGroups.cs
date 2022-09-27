using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Security.Cryptography;

public class PieceGroups : MonoSingleton<PieceGroups>
{
    public List<PieceGroupTypes> GroupTypesList = new List<PieceGroupTypes>();

    // 8x8 lik her bir kare i�in en uygun par�a gruplar�n� random bir �ekilde aliyor. Her birini tek tek kontrol ediyor ve e�er uyumlu hi� bir grup par�as� yoksa null d�nd�r�yor.
    public PieceGroupTypes GetPieceGroupType(int firstPosX, int firstPosZ)
    {
        PieceGroupTypes selectPieceGroupTypes = null;
        System.Random random = new System.Random();
        GroupTypesList = GroupTypesList.OrderBy(x => random.Next()).ToList();

        
        for (int i = 0; i < GroupTypesList.Count; i++)
        {
            bool trueArea = true;
            selectPieceGroupTypes = GroupTypesList[i];
            for (int j = 0; j < GroupTypesList[i].X.Length; j++)
            {
                if (CreatePieces.Instance.PieceList.GetLength(0) > GroupTypesList[i].X[j] + firstPosX && (GroupTypesList[i].X[j] + firstPosX) >= 0
                    && CreatePieces.Instance.PieceList.GetLength(1) > GroupTypesList[i].Z[j] + firstPosZ && (GroupTypesList[i].Z[j] + firstPosZ) >= 0)
                {
                    if (CreatePieces.Instance.PieceList[GroupTypesList[i].X[j] + firstPosX, GroupTypesList[i].Z[j] + firstPosZ].CreateAdded)
                    {
                        trueArea = false;
                        break;
                    }
                }
                else
                {
                    trueArea = false;
                    break;
                }
            }
            if (trueArea)
            {
                break;
            }
            selectPieceGroupTypes = null;
        }
        return selectPieceGroupTypes;
    }

}
