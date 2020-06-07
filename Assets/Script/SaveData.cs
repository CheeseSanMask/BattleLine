using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData : MonoBehaviour
{
    // 色
    public CardColor _saveColor;

    // 数字
    public int _saveNumber;

    // 親の名前
    public string _saveParentName;


    // デフォルトコンストラクタ
    public SaveData()
    {
        _saveColor = CardColor.Max;
        _saveNumber = 0;
        _saveParentName = "";
    }


    // 引数付きコンストラクタ
    public SaveData( CardColor color, int number, string parentName )
    {
        _saveColor = color;
        _saveNumber = number;
        _saveParentName = parentName;
    }
}