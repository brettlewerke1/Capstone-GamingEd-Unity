using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Login : MonoBehaviour
{
    public InputField nameField;
    public InputField passwordField;
    readonly string postURL = "http://localhost/UnityApp/login.php";

    public Button submitButton;

    public void CallLogin()
    {
        StartCoroutine(LoginPlayer());
    }

    IEnumerator LoginPlayer()
    {
        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        wwwForm.Add(new MultipartFormDataSection("Username", nameField.text));
        wwwForm.Add(new MultipartFormDataSection("Password", passwordField.text));
        using (UnityWebRequest www = UnityWebRequest.Post(postURL, wwwForm))
        {
            yield return www.SendWebRequest();
            string text = www.downloadHandler.text;
            string[] response = text.Split(' ');
            string Role = response[0];
            string Username = response[1];
            string Password = response[2];
            Debug.Log(Role);
            if (www.result == UnityWebRequest.Result.Success)
            {
                if(nameField.text == Username && passwordField.text == Password)
                {
                    DbManager.username = Username;

                    DbManager.Role = Role;

                    if(Role == "superadmin")
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene(4);
                    }
                    else if(Role == "admin")
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene(6);
                    }
                    else
                    {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                    }

                }

            }
            else
            {
                Debug.Log(nameField.text);
                Debug.Log(passwordField.text);
                Debug.Log("User Log in failed:" + text);
            }
        }


    }

    public void VerifyInputs()
    {
        submitButton.interactable = (nameField.text.Length >= 1 && passwordField.text.Length >= 1);
    }
}
