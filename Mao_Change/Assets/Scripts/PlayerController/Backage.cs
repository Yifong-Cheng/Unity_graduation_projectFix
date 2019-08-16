using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backage {

    public int Enegy;
    public int Mat0;
    public int Mat1;
    public int Mat2;
	
}

public class BackageInfo
{
    //public List<Backage> m_Backage = new List<Backage>();
    private Backage instance = new Backage();
    public Backage Instance
    {
        get
        {
            return instance;
        }
        set
        {
            instance = value;
        }
        
    }

    

}
