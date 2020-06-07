using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Save : MonoBehaviour
{
    // ファイルパス
    private string _filePath;

    // セーブデータ
    private SaveData _saveData;

    // カードプレイ順
    private List<CardData> _cardPlayOrder;


    // コンストラクタ
    private void Start()
    {
        _filePath = Application.dataPath+"/SaveData/savedata.json";

        _saveData = new SaveData();

        _cardPlayOrder = new List<CardData>();
    }


    // 対戦記録のセーブ
    public void SaveBattleData()
    {
        string dataString = "";

        for( int num = 0; num < _cardPlayOrder.Count; num++ )
        {
            dataString += _cardPlayOrder[num]._color.ToString()    +",";
            dataString += _cardPlayOrder[num]._number              +",";
            dataString += _cardPlayOrder[num].transform.parent.name+",";
        }

        string json = JsonUtility.ToJson( dataString );

        StreamWriter streamWriter = new StreamWriter( _filePath );
        streamWriter.Write( json );
        streamWriter.Flush();
        streamWriter.Close();
    }


    // 対戦記録のロード
    public void LoadBattleData()
    {
        if( File.Exists( _filePath ) )
        {
            StreamReader streamReader = new StreamReader( _filePath );
            string data = streamReader.ReadToEnd();
            streamReader.Close();

            _cardPlayOrder.Clear();
            string[] dataArray = JsonUtility.FromJson<string>( data ).Split( ',' );

            for( int num = 0; num < dataArray.Length; num++ )
            {
                CardColor color = (CardColor)Enum.Parse( typeof( CardColor ), dataArray[num] );
                int number = int.Parse( dataArray[num+1] );
                Team team = (Team)Enum.Parse( typeof( Team ), dataArray[num+2] );

                CardData cardData = new CardData( color, number, team );

                _cardPlayOrder.Add( cardData );

                Debug.Log( color.ToString() );
                Debug.Log( number           );
                Debug.Log( team             );
            }
        }
    }


    // カードプレイ順挿入
    //public void SetCardPlayOrder( CardData cardData )
    //{
    //    _cardPlayOrder.Add( cardData );
    //}
}
