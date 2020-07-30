using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerialPlayerRpg
{
    public string cName;
    public long yang;
    public int level;
    public long exp;
    public int sPoints;
    public string sPointsTab;
    public float cHp;
    public float cEn;
    public float cSt;
    public int bVit;
    public int bInt;
    public int bStr;
    public int bDex;

    public SerialPlayerRpg(string cName, long yang, int level, long exp, 
        int sPoints, string sPointsTab, float cHp, float cEn, float cSt, 
        int bVit, int bInt, int bStr, int bDex)
    {
        this.cName = cName;
        this.yang = yang;
        this.level = level;
        this.exp = exp;
        this.sPoints = sPoints;
        this.sPointsTab = sPointsTab;
        this.cHp = cHp;
        this.cEn = cEn;
        this.cSt = cSt;
        this.bVit = bVit;
        this.bInt = bInt;
        this.bStr = bStr;
        this.bDex = bDex;
    }
}
