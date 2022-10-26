using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DropDownHandler : MonoBehaviour
{
    // Start is called before the first frame update

    public Text textbox;

    public TMP_InputField inputfield;

    public GameObject gameobject;

    public Vector3 MultipleChoiceLocation = new Vector3(-1500, 0 ,0);

    public Vector3 Fill_in_BlankLocation = new Vector3(-2000, 0,0);

    public Vector3 MatchingLocation = new Vector3(-3000,0,0);

    public TMP_Dropdown dropdown;
    
    void Start()
    {
        dropdown = transform.GetComponent<TMP_Dropdown>();
        DropdownItemSelected(dropdown);
        dropdown.onValueChanged.AddListener(delegate {DropdownItemSelected(dropdown);});   

    }


    public void DropdownItemSelected(TMP_Dropdown dropdown)
    {
        int index = dropdown.value;
        string selectedOption = dropdown.options[index].text;
        Vector3 dropdownLocalLocation = dropdown.transform.localPosition;
        Vector3 dropdownLocation = dropdown.transform.position;
        PlayerPrefs.SetString("QuestionType", selectedOption);

        if(selectedOption == "Multiple Choice")
        {
            DeleteObjectsByChunk(dropdownLocalLocation);
            gameobject = GameObject.FindWithTag("MultipleChoice");
            // find the correct position to spawn in object
            dropdownLocation.y -= 50;
            dropdownLocation.x -= 100;
            //spawn new multiple choice quesiton
            //GameObject newObject = Instantiate(gameobject, dropdownLocation ,Quaternion.identity);
            //set parent
            setParent(gameobject, GameObject.Find("MainAssessment"));

            //move other objects back to unloaded objects
            gameobject = GameObject.FindWithTag("Fill_in_Blank");
            setParent(gameobject,GameObject.Find("UnloadedObjects"));
            gameobject = GameObject.FindWithTag("Matching");
            setParent(gameobject,GameObject.Find("UnloadedObjects"));
            

            PlayerPrefs.DeleteKey("TypeOfQuestion");
            PlayerPrefs.SetString("TypeOfQuestion", "MultipleChoice");

            // start spawning the different objects
        }
        else if(selectedOption == "Fill_in_Blank")
        {
            DeleteObjectsByChunk(dropdownLocalLocation);
            gameobject = GameObject.FindWithTag("Fill_in_Blank");
            dropdownLocation.y -= 100;
            //spawn new multiple choice quesiton
            //GameObject newObject = Instantiate(gameobject, dropdownLocation,Quaternion.identity);
            //set new parent
            setParent(gameobject, GameObject.Find("MainAssessment"));
            
            //move other objects back to unloaded objects
            gameobject = GameObject.FindWithTag("MultipleChoice");
            setParent(gameobject,GameObject.Find("UnloadedObjects"));
            gameobject = GameObject.FindWithTag("Matching");
            setParent(gameobject,GameObject.Find("UnloadedObjects"));


            PlayerPrefs.DeleteKey("TypeOfQuestion");
            PlayerPrefs.SetString("TypeOfQuestion", "Fill_in_Blank");

        }
        else if(selectedOption == "Matching")
        {
            DeleteObjectsByChunk(dropdownLocalLocation);
            gameobject = GameObject.FindWithTag("Matching");
            dropdownLocation.y -=100;
            dropdownLocation.x +=3000;
            //spawn new multiple choice quesiton
            ///GameObject newObject = Instantiate(gameobject, dropdownLocation,Quaternion.identity);
            //set new parent
            setParent(gameobject, GameObject.Find("MainAssessment"));

            //move other objects back to unloaded objects
            gameobject = GameObject.FindWithTag("MultipleChoice");
            setParent(gameobject,GameObject.Find("UnloadedObjects"));
            gameobject = GameObject.FindWithTag("Fill_in_Blank");
            setParent(gameobject,GameObject.Find("UnloadedObjects"));
            

            PlayerPrefs.DeleteKey("TypeOfQuestion");
            PlayerPrefs.SetString("TypeOfQuestion", "Matching");

        }

    }

    public void moveOtherObjects(string selectedOption)
    {
        GameObject tempObject;
        if(selectedOption == "Multiple Choice")
        {
            tempObject = GameObject.FindWithTag("Fill_in_Blank");
            tempObject.transform.position = Fill_in_BlankLocation;
            tempObject = GameObject.FindWithTag("Matching");
            tempObject.transform.position = MatchingLocation;
        }
        else if(selectedOption == "Fill_in_Blank")
        {
            tempObject = GameObject.FindWithTag("MultipleChoice");
            tempObject.transform.position = Fill_in_BlankLocation;
            tempObject = GameObject.FindWithTag("Matching");
            tempObject.transform.position = MatchingLocation;
        }
        else if(selectedOption == "Matching")
        {
            tempObject = GameObject.FindWithTag("Fill_in_Blank");
            tempObject.transform.position = Fill_in_BlankLocation;
            tempObject = GameObject.FindWithTag("MultipleChoice");
            tempObject.transform.position = MatchingLocation;
        }

    }

    public void DeleteObjectsByChunk(Vector3 topPosition)
    {
        //find the new question object
        GameObject newQuestion = GameObject.Find("NewQuestion");
        Vector3 bottomPostion = newQuestion.transform.localPosition;
        Debug.Log(topPosition);
        Debug.Log(bottomPostion);
        // loop through all objects
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("MultipleChoice"))
        {


         //if object is between top location and new question location
          if(go.transform.localPosition.y <= topPosition.y && go.transform.localPosition.y >= bottomPostion.y)
          {
            Destroy(go);

            Debug.Log(go.name);
            Debug.Log(go.transform.localPosition);
            

          }
        }
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Fill_in_Blank"))
        {


         //if object is between top location and new question location
          if(go.transform.localPosition.y < topPosition.y && go.transform.localPosition.y > bottomPostion.y)
          {
            Destroy(go);

            Debug.Log(go.name);
            Debug.Log(go.transform.localPosition);
            

          }
        }
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Matching"))
        {


         //if object is between top location and new question location
          if(go.transform.localPosition.y < topPosition.y && go.transform.localPosition.y > bottomPostion.y)
          {
            Destroy(go);

            Debug.Log(go.name);
            Debug.Log(go.transform.localPosition);
          }
        }
    }

    public void setParent(GameObject childObject, GameObject parentObject)
    {
        // set parent
        childObject.transform.SetParent(parentObject.transform);

    }

    public GameObject FindLowestInputField()
    {
        // need a reference point so find positon of header
        GameObject lowestObject = GameObject.Find("Header");

        GameObject[] inputFields = GameObject.FindGameObjectsWithTag("InputField");
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("InputField") as GameObject[])
        {

            if(lowestObject.transform.position.y > go.transform.position.y)
            {
                lowestObject = go;
            }
            

        }
        return lowestObject;

    }

}
