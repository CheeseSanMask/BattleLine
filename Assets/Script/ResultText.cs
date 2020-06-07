using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultText : MonoBehaviour
{
    // テキスト
    private Text _text;


    // コンストラクタ
    private void Start()
    {
        _text = this.GetComponent<Text>();
        _text.text = FlagManager.GetWinTeam().ToString()+" Win!!";
    }
}