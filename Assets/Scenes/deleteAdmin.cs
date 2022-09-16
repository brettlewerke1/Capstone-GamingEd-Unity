using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class deleteAdmin : MonoBehaviour
{
    readonly string deleteTeacherURL = "http://localhost/UnityApp/deleteATeacher.php";
    public InputField UsernameField;
    public Text AdminUsername;

    public Button SubmitButton;
    // Start is called before the first frame update
    void Start()
    {
        AdminUsername.text = DbManager.AdminInformation.Split(' ')[1];
    }
    public void DeleteButton()
    {
        StartCoroutine(DeleteAnAdmin());
    }

    IEnumerator DeleteAnAdmin()
    {
        if(UsernameField.text == AdminUsername.text)
        {
            List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
            wwwForm.Add(new MultipartFormDataSection("Username", UsernameField.text));
            using (UnityWebRequest www = UnityWebRequest.Post(deleteTeacherURL, wwwForm))
            {
                yield return www.SendWebRequest();
                string text = www.downloadHandler.text;
                Debug.Log(text);
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene(5);
        }
        else
        {
            AdminUsername.text = "Usernames do not match";
        }

       


    }

    public void GoBack()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(7);
    }
    
    public void VerifyInputs()
    {
        SubmitButton.interactable = (UsernameField.text.Length >= 1);
    }

}
