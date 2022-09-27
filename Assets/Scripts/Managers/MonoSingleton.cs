using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Her bir Singleton i�in tanimlama ve start i�inde yazmak yerine kolay bir �ekilde kullanmay� sa�layan s�n�f.
public class MonoSingleton<T> : MonoBehaviour where T: MonoSingleton<T>
{
    private static volatile T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType((typeof(T))) as T;
            }
            
            return instance;
        }
    }
}