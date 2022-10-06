using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon : MonoBehaviour
{
    public float x = 0;
    public float y = 0;
    public float z = 0;
    public bool isPlacable;

    //public Icon()
    //{
   ///     int x = 0;
   //     int y = 0;
    //    int z = 0;
   //     bool isPlacable = false;       
   // }
    public Icon getSpawnCoords(string type)
    {
        Icon returnVal = gameObject.GetComponent<Icon>();
        if(type == "Gate")
        {

            returnVal.x = 3257;
            returnVal.y = 0;
            returnVal.z= 5699;
            
        }
        else if (type == "Assignment")
        {
            returnVal.x = 3211;
            returnVal.y = 0;
            returnVal.z= 5435;          
        }
        else if (type == "Test")
        {
            returnVal.x = 3215;
            returnVal.y = 0;
            returnVal.z = 5218;
        }
        else
        {
            return null;
        }
        return returnVal;
    }
}
