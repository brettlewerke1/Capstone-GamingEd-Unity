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
        //DropdownItemSelected(dropdown);
        dropdown.onValueChanged.AddListener(delegate {DropdownItemSelected(dropdown);});   

    }


    public void DropdownItemSelected(TMP_Dropdown dropdown)
    {
        int index = dropdown.value;
        string selectedOption = dropdown.options[index].text;
        GameObject mainAssessmentObj = GameObject.Find("MainAssessment");
        // if(PlayerPrefs.GetString("QuestionType")== null)
        // {
        //     selectedOption = dropdown.options[index].text;
        // }
        // else
        // {
        //     selectedOption = PlayerPrefs.GetString("QuestionType");
        // }

        Vector3 dropdownLocalLocation = dropdown.transform.localPosition;
        Vector3 dropdownLocation = dropdown.transform.position;

        //SET QUESTION TYPE
        PlayerPrefs.SetString("QuestionType", selectedOption);

        if(selectedOption == "Multiple Choice")
        {
            //DeleteObjectsByChunk(dropdownLocalLocation);
            gameobject = GameObject.FindWithTag("MultipleChoice");
            // find the correct position to spawn in object
            //spawn new multiple choice quesiton
            //GameObject newObject = Instantiate(gameobject, dropdownLocation ,Quaternion.identity);
            //set parent
            setParent(gameobject, mainAssessmentObj);
            Vector3 tempLocation = mainAssessmentObj.transform.localPosition;
            tempLocation.y -= 300;
            gameobject.transform.localPosition = tempLocation;
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
            gameobject = GameObject.FindWithTag("Fill_in_Blank");
            //spawn new multiple choice quesiton
            //GameObject newObject = Instantiate(gameobject, dropdownLocation,Quaternion.identity);
            //set new parent
            setParent(gameobject, mainAssessmentObj);
            Vector3 tempLocation = mainAssessmentObj.transform.localPosition;
            tempLocation.y -= 300;
            gameobject.transform.localPosition = tempLocation;
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
            gameobject = GameObject.FindWithTag("Matching");
            //spawn new multiple choice quesiton
            ///GameObject newObject = Instantiate(gameobject, dropdownLocation,Quaternion.identity);
            //set new parent
            setParent(gameobject, mainAssessmentObj);
            Vector3 tempLocation = mainAssessmentObj.transform.localPosition;
            tempLocation.y -= 300;
            gameobject.transform.localPosition = tempLocation;
            //move other objects back to unloaded objects
            gameobject = GameObject.FindWithTag("MultipleChoice");
            setParent(gameobject,GameObject.Find("UnloadedObjects"));
            gameobject = GameObject.FindWithTag("Fill_in_Blank");
            setParent(gameobject,GameObject.Find("UnloadedObjects"));
            

            PlayerPrefs.DeleteKey("TypeOfQuestion");
            PlayerPrefs.SetString("TypeOfQuestion", "Matching");

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
