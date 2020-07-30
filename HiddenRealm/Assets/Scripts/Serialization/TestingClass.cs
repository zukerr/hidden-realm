using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingClass : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //TestSpAquisitionSerialization();
    }

    private void TestSpAquisitionSerialization()
    {
        bool[,] tab = new bool[99, 3];

        int counter = 29;

        for(int i = 0; i < 99; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                if(counter > 0)
                {
                    tab[i, j] = true;
                    counter--;
                }
                else
                {
                    tab[i, j] = false;
                }
            }
        }

        Debug.Log(tab);
        string serial = GlobalSerialization.SerializeSpAquisitionTable(tab);
        Debug.Log(serial);
        bool[,] deserializedTab = GlobalSerialization.DeserializeSpAquisitionTable(serial);
        Debug.Log(deserializedTab);
    }
}
