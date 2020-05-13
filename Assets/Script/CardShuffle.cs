using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardShuffle : MonoBehaviour
{
    static private readonly int CardMaxNum = 60;

    [SerializeField] List<int> _numList = new List<int>();


    void Start()
    {
        
        for( int num = 0; num < CardMaxNum; num++ )
        {
            _numList.Add( num+1 );
        }

        ShuffleFunction();

        int a = 0;
    }

    void Update()
    {
        
    }

    private void ShuffleFunction()
    {
        int tempNum;

        for( int num = 0; num < CardMaxNum; num++ )
        {
            tempNum = _numList[num];
            int index = Random.Range( 0, CardMaxNum );
            _numList[num] = _numList[index];
            _numList[index] = tempNum;
        }
    }
}
