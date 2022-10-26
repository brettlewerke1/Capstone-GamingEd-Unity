using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class Assignment : MonoBehaviour
{

    public InputField nameOfAssessment;
    public InputField startDate;
    public InputField dueDate;
    public InputField Question;
    public InputField Answer;
    public Button saveButton;
    public Button newQuestion;
    readonly string postURL = "http://localhost/UnityApp/assignment.php";
    public Button submitButton;

    string Description;

    
    public float x = 3324;
    public float y = 0;
    public float z = 5435;

    public void Start()
    {
        //DontDestroyOnLoad(this);
    }
    public void addNewAnswer()
    {
    }

    public void changeTag(int val)
    {
        if(val == 0)
        {
           
        }
        else if(val==1)
        {
            
        }
        else 
        {

        }

    }

    // IEnumerator NewAnswer()
    // {
    //     List<IMultipartFormSection> www = new List<IMultipartFormSection>();
    //     yield return www.SendWebRequest();

    // }
// mm/dd/yyyy
    public void ZoomOut()
    {
        Vector3 position = new Vector3(1924, 1095, 4965);
        
        Camera.main.transform.position = position;
        //SceneManager.UnloadSceneAsync(11);

    }

    public void GoBack()
    {
        SceneManager.UnloadSceneAsync("EditAssignmentIcon");
        Vector3 newCameraPosition = new Vector3( Camera.main.transform.position.x, 1095, 4965);
        Camera.main.transform.position = newCameraPosition;
    }

    
}
