using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBarSerialization : MonoBehaviour
{
    public GameObject grid1;
    public GameObject grid2;

    public List<GameObject> grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new List<GameObject>();
        for(int i = 0; i < grid1.transform.childCount; i++)
        {
            grid.Add(grid1.transform.GetChild(i).gameObject);
            grid.Add(grid2.transform.GetChild(i).gameObject);
        }
    }
}
