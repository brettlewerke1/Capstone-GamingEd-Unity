using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class WebTest : MonoBehaviour
{
    readonly string GetURL = "http://localhost/UnityApp/webtest.php";
    // Start is called before the first frame update
    IEnumerator Start()
    {
        UnityWebRequest www = UnityWebRequest.Get(GetURL);

        yield return www.SendWebRequest();
        string request = www.downloadHandler.text;
        string[] webResults = request.Split('\t');
        foreach( string s in webResults)
        {
            Debug.Log(s);
        }
        
    }

}
