using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


public class Grabber : MonoBehaviour {

    public GameObject selectedObject;

    public GameObject objectToBeEdited;

    public GameObject highlightObject;

    public GameObject gateObject;

    public GameObject tempObject;

    public GameObject objFromLoad;

    public Material tempMaterial;

    public Icon iconObject;

    public TMP_Text CourseName;

    public string LocationString;

    public Mesh tempMesh;

    private void Start()
    {
        iconObject = gameObject.AddComponent<Icon>();

        CourseName.text = PlayerPrefs.GetString("ClassTag");
        LoadObjectsFromJson();
        PlayerPrefs.SetString("ModuleSelect", "false");

        ///DELETE ME
        

        //something = validLocationList;
        Debug.Log("Awake() is called");

    }
    
    private void Update() 
    {
        if (Input.GetMouseButtonDown(0)) {
            if(selectedObject == null) {
                RaycastHit hit = CastRay();

                if(hit.collider != null) {
                    if (hit.collider.gameObject.name == "EditButtonIcon") 
                    {
                        //tempObject = hit.collider.gameObject;
                        flashWithHighlight("EditButtonIcon");
                        //-----------Assignment or Assessment----------//
                        if(objectToBeEdited.tag == "Assessment" || objectToBeEdited.tag == "Assignment")
                        {
                            // conver the location to string to be looked up in JSON
                            LocationString = ConvertLocationToString(objectToBeEdited.transform.position);
                            //save playerprefs of the location of the objectToBeEdited
                            PlayerPrefs.SetString("ObjectToBeEditedLocation", LocationString);
                            //save the level number in player prefs in order to be passed into next scene
                            if(objectToBeEdited.tag == "Assessment")
                            {
                                PlayerPrefs.SetInt("LevelNumber", objectToBeEdited.GetComponent<AssessmentAttributes>().LevelNum);

                            }
                            else if(objectToBeEdited.tag == "Assignment")
                            {
                                PlayerPrefs.SetInt("LevelNumber", objectToBeEdited.GetComponent<AssignmentAttributes>().LevelNum);
                            }

                            //now load the correct scene
                            LoadCorrectScene(hit.collider.gameObject, objectToBeEdited);
                        }
                        // --------------Gate Icon--------------------//
                        else
                        {
                            // we need to move the edit and delete objects away and move confirm over
                            PlayerPrefs.SetString("ModuleSelect", "true");
                            GameObject tmpObject = GameObject.Find("EditButtonIcon");
                            tmpObject.transform.position = new Vector3(0,0,10000);
                            changeMaterial(tmpObject, Resources.Load("Materials/Transparent_Material", typeof(Material)) as Material);
                            tmpObject = GameObject.Find("DeleteButtonIcon");
                            tmpObject.transform.position = new Vector3(0,0,10300);
                            changeMaterial( tmpObject,Resources.Load("Materials/Transparent_Material", typeof(Material)) as Material);
                            tmpObject = GameObject.Find("ConfirmButtonIcon");
                            // do some positioning now for the confirm button
                            Vector3 locationOfGate = objectToBeEdited.transform.position;
                            locationOfGate.z += 50;
                            locationOfGate.y +=300;
                            locationOfGate.x -= 550;
                            tmpObject.transform.position = locationOfGate;
                            Debug.Log("Module select is true");

                        }
                        selectedObject = null;
                    }
                    else if (hit.collider.gameObject.name == "DeleteButtonIcon") 
                    {
                        flashWithHighlight("DeleteButtonIcon");
                        // change the object to be selected back to highlight
                        // remove all highlight nodes around it
                        changeMaterial(objectToBeEdited, Resources.Load("Materials/Transparent_Material", typeof(Material)) as Material);
                        changeMesh(objectToBeEdited);
                        //strip the script that has the object attributes in it
                        StripAttributeScript(objectToBeEdited);
                        objectToBeEdited.tag = "Highlight";
                        de_spawnHighlight(objectToBeEdited.transform.position);
                        // convert the location to string to be looked up in JSON
                        LocationString = ConvertLocationToString(objectToBeEdited.transform.position);
                        DeleteFileAtLocation(LocationString);

                        selectedObject = null;
                    }
                    else if(hit.collider.CompareTag("GateConfirm"))
                    {
                        //flashWithHighlight("ConfirmButtonIcon");
                        //TODO: do stuff here
                        tempObject = GameObject.Find("ConfirmButtonIcon");
                        tempObject.transform.position = new Vector3(0,300,10700);
                        UnHighlightIcons();
                        PlayerPrefs.SetString("ModuleSelect", "false");
                        SaveGate(gateObject);
                    }
                    else if (hit.collider.CompareTag("Gate Legend")) 
                    {
                        selectedObject = hit.collider.gameObject;
                        iconObject = iconObject.getSpawnCoords("Gate");
                        //Debug.Log(iconObject.x);
                        Cursor.visible = false;
                        highlightAreas();

                    }  
                    else if (hit.collider.CompareTag("Assignment Legend")) 
                    {
                        selectedObject = hit.collider.gameObject;
                        iconObject = iconObject.getSpawnCoords("Assignment");
                        Cursor.visible = false;      
                        highlightAreas();                
                    }
                    else if (hit.collider.CompareTag("Test Legend")) 
                    {
                        selectedObject = hit.collider.gameObject;
                        iconObject = iconObject.getSpawnCoords("Assessment");
                        Cursor.visible = false;    
                        highlightAreas();                  
                    }                   
                    else if(hit.collider.CompareTag("Upload"))
                    {
                        Debug.Log("UploadClicked");
                        //UploadToDB();

                    }
                    else if(hit.collider.CompareTag("ViewCourse"))
                    {
                        if(PlayerPrefs.GetString("LoggedIn") == "true")
                        {
                            SceneManager.LoadScene("ViewCourse");

                        }
                        else if(PlayerPrefs.GetString("LoggedIn") == "false")
                        {
                            SceneManager.LoadScene("logInMenu");
                        }
                        
                    }
                    // this else statement is for choosing modules inside of a gate
                    else if(hit.collider.CompareTag("Assignment") || hit.collider.CompareTag("Assessment"))
                    {
                        if(PlayerPrefs.GetString("ModuleSelect") == "true")
                        {
                            // variable assignment
                            GameObject hitObject = hit.collider.gameObject;
                            Material hitMaterial = hitObject.GetComponent<MeshRenderer>().sharedMaterial;
                            Material glowMaterial = Resources.Load("Materials/Glow_Material", typeof(Material)) as Material;
                            Material assignmentMaterial = Resources.Load("Materials/Assignment_Material", typeof(Material)) as Material;
                            Material assessmentMaterial = Resources.Load("Materials/Test_Material", typeof(Material)) as Material;
                            // turn off the lights
                            Light[] ligths = FindObjectsOfType(typeof(Light)) as Light[];
                            foreach (Light ligth in ligths) 
                            {
                                ligth.intensity = 1;
                            }
                            RenderSettings.ambientLight = Color.black;
                            // make material adjustments
                            //selecting object
                            if(hitMaterial != glowMaterial)
                            {
                                
                                hitObject.GetComponent<MeshRenderer>().sharedMaterial = glowMaterial;
                                Debug.Log(hitObject.GetComponent<MeshRenderer>().sharedMaterial);
                                // do stuff here
                                if(hitObject.tag == "Assignment")
                                {
                                    Debug.Log("Changing " + hitObject.tag + " to glowing");
                                    hitObject.GetComponent<AssignmentAttributes>().LevelNum = objectToBeEdited.GetComponent<Gate>().LevelNum;
                                }
                                else if(hitObject.tag == "Assessment")
                                {
                                    Debug.Log("Changing " + hitObject.tag + " to glowing");
                                    hitObject.GetComponent<AssessmentAttributes>().LevelNum = objectToBeEdited.GetComponent<Gate>().LevelNum;

                                }


                            }
                            //deselecting object
                            else if(hitMaterial == glowMaterial)
                            {
                                if(hitObject.tag == "Assignment")
                                {
                                    changeMaterial(hitObject, assignmentMaterial);
                                    hitObject.GetComponent<AssignmentAttributes>().LevelNum = 0;
                                    
                                }
                                else if(hitObject.tag == "Assessment")
                                {
                                    changeMaterial(hitObject, assessmentMaterial);
                                    hitObject.GetComponent<AssessmentAttributes>().LevelNum = 0;
                                }

                            }

                        }
                    }
                    else
                    {
                     //if you click anywhere...the edit/delete should go away
                    tempObject = GameObject.Find("EditButtonIcon");

                    tempObject.transform.position = new Vector3(0,0,10000);
                    changeMaterial(tempObject, Resources.Load("Materials/Transparent_Material", typeof(Material)) as Material);
                    tempObject = GameObject.Find("DeleteButtonIcon");
                    tempObject.transform.position = new Vector3(0,0,10300);
                    changeMaterial( tempObject,Resources.Load("Materials/Transparent_Material", typeof(Material)) as Material);

                    }


                }
            } else if(checkWithinBounds(selectedObject)) {
                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
                spawn3HighlightsAroundPositon(findHighlightWithinBounds(selectedObject).transform.position, selectedObject);
                changeHighlight(selectedObject);
                //this moves it back into legend//
                // u must find tag of selected object u just dragged...and move accordingly
                selectedObject.transform.position = new Vector3(iconObject.x,iconObject.y,iconObject.z);
                //-----------------------------//
                selectedObject = null;
                Cursor.visible = true;
                unHighlightAreas();
            }
            else{
                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
                selectedObject.transform.position = new Vector3(iconObject.x , 0f, iconObject.z);
                selectedObject = null;
                Cursor.visible = true;
                unHighlightAreas();
            }

        }

        if(selectedObject != null) {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            selectedObject.transform.position = new Vector3(worldPosition.x, .25f, worldPosition.z);
        }
        //-----------------EDIT ICON----------------//
        //
        //------------------------------------------//

        if(Input.GetMouseButtonDown(1))
        {
            RaycastHit hit = CastRay();
            if(hit.collider != null) 
            {
                    PlayerPrefs.SetString("FilePath", Application.persistentDataPath + "/" + PlayerPrefs.GetString("ClassTag") +"/"
                    + hit.collider.gameObject.transform.position.x.ToString() +"." 
                    + hit.collider.gameObject.transform.position.y.ToString() + "."
                    + hit.collider.gameObject.transform.position.z.ToString() +".json");
                //find the edit button first
                tempObject = GameObject.Find("EditButtonIcon");
                if (hit.collider.CompareTag("Gate")) 
                {
                    selectedObject = hit.collider.gameObject;
                    //get cam position
                    Vector3 position = selectedObject.transform.position;
                    Vector3 newPosition = new Vector3(position.x -287 , position.y+ 102, position.z + 208);
                    //move edit button
                    tempObject.transform.position = newPosition;
                    Material newMaterial = Resources.Load("Materials/Gate_Material", typeof(Material)) as Material;
                    tempObject.GetComponent<Renderer>().material = newMaterial;

                    //find the delete button
                    tempObject = GameObject.Find("DeleteButtonIcon");
                    newPosition = new Vector3(position.x - 479, position.y+102, position.z -440);
                    //move delete button
                    tempObject.transform.position = newPosition;
                    newMaterial = Resources.Load("Materials/Gate_Material", typeof(Material)) as Material;
                    tempObject.GetComponent<Renderer>().material = newMaterial;

                    objectToBeEdited = selectedObject;
                    // we need to save the gate object just in case
                    // while the user is clicking the icons to be added to the level 
                    // they can right click on anything and it would mess up saving the gate
                    gateObject = objectToBeEdited;

                    selectedObject = null;
                }
                else if(hit.collider.CompareTag("Assignment"))
                {
                    selectedObject = hit.collider.gameObject;
                    //get cam position
                    Vector3 position = selectedObject.transform.position;
                    
                    Vector3 newPosition = new Vector3(position.x -287 , position.y+102, position.z + 208);
                    //move edit button
                    tempObject.transform.position = newPosition;
                    Material newMaterial = Resources.Load("Materials/Assignment_Material", typeof(Material)) as Material;
                    tempObject.GetComponent<Renderer>().material = newMaterial;

                    //find the delete button
                    tempObject = GameObject.Find("DeleteButtonIcon");
                    newPosition = new Vector3(position.x - 479, position.y+102, position.z -440);
                    //move delete button
                    tempObject.transform.position = newPosition;
                    newMaterial = Resources.Load("Materials/Assignment_Material", typeof(Material)) as Material;
                    tempObject.GetComponent<Renderer>().material = newMaterial;

                    objectToBeEdited = selectedObject;

                    selectedObject = null;
                }
                else if(hit.collider.CompareTag("Assessment"))
                {
                    selectedObject = hit.collider.gameObject;
                    //get cam position
                    Vector3 position = selectedObject.transform.position;
                    Vector3 newPosition = new Vector3(position.x -287 , position.y + 102, position.z + 208);
                    //move edit button
                    tempObject.transform.position = newPosition;
                    Material newMaterial = Resources.Load("Materials/Test_Material", typeof(Material)) as Material;
                    tempObject.GetComponent<Renderer>().material = newMaterial;

                    //find the delete button
                    tempObject = GameObject.Find("DeleteButtonIcon");
                    newPosition = new Vector3(position.x - 479, position.y + 102, position.z -440);
                    //move delete button
                    tempObject.transform.position = newPosition;
                    newMaterial = Resources.Load("Materials/Test_Material", typeof(Material)) as Material;
                    tempObject.GetComponent<Renderer>().material = newMaterial;

                    objectToBeEdited = selectedObject;

                    selectedObject = null;
                }
        }
    }
    }

    private RaycastHit CastRay() {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);


        return hit;
    }
    public void spawnObject(GameObject Object, float x, float y, float z)
    {
        var newSquare = Instantiate(
            Object, 
            new Vector3(x, y, z), 
            Quaternion.identity);
    } 
    public void spawn3HighlightsAroundPositon(Vector3 position, GameObject selectedObject)
    {
        Vector3 left = new Vector3(position.x - 250, position.y, position.z);
        Vector3 right = new Vector3(position.x + 250, position.y, position.z);
        Vector3 down = new Vector3(position.x, position.y, position.z- 250);
        if(getObjectByLocation(left) == null)
        {
        createHighlightedObject(position.x - 250, position.y, position.z);
        }
        if(getObjectByLocation(right) == null)
        {
        createHighlightedObject(position.x + 250, position.y, position.z);
        }
        if(getObjectByLocation(down) == null)
        {
        createHighlightedObject(position.x, position.y, position.z- 250);
        }



    }

    public void de_spawnHighlight(Vector3 position)
    {
        Vector3 left = new Vector3(position.x - 250, position.y, position.z);
        Vector3 right = new Vector3(position.x + 250, position.y, position.z);
        Vector3 down = new Vector3(position.x, position.y, position.z- 250);
        if(getObjectByLocation(left) == null)
        {
            Destroy(getObjectByLocation(left));
        }
        if(getObjectByLocation(right) == null)
        {
            Destroy(getObjectByLocation(right));
        }
        if(getObjectByLocation(down) == null)
        {
            Destroy(getObjectByLocation(down));
        }
    }


    public void createHighlightedObject(float x, float y, float z)
    {
        highlightObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        tempMaterial = Resources.Load("Materials/Transparent_Material", typeof(Material)) as Material;
        highlightObject.GetComponent<Renderer>().material = tempMaterial;
        highlightObject.transform.localScale = new Vector3 (100f, 100f, 100f);
        highlightObject.gameObject.tag = "Highlight";
        highlightObject.GetComponent<BoxCollider>().isTrigger = true;
        highlightObject.AddComponent<Rigidbody>().isKinematic = true;
        highlightObject.AddComponent<MeshCollider>();
        highlightObject.transform.position = new Vector3(x, y, z);
        highlightObject.AddComponent<Highlighter>();
        highlightObject.GetComponent<Highlighter>().x = x;
        highlightObject.GetComponent<Highlighter>().y = y;
        highlightObject.GetComponent<Highlighter>().z = z;
    }

    //this unhighlights the highlight objects
    public void highlightAreas()
    {
        Light[] ligths = FindObjectsOfType(typeof(Light)) as Light[];
        foreach (Light ligth in ligths) 
        {
            ligth.intensity = 1;
        }
        RenderSettings.ambientLight = Color.black;

        // add loop to change all highlight material
        /*GameObject[]*/ var objects = GameObject.FindGameObjectsWithTag("Highlight");
        foreach (var obj in objects)
        {
            tempMaterial = Resources.Load("Materials/Glow_Material", typeof(Material)) as Material;
            obj.GetComponent<Renderer>().material = tempMaterial;
        }
    }

    public void unHighlightAreas()
    {
        Light[] ligths = FindObjectsOfType(typeof(Light)) as Light[];
        foreach (Light ligth in ligths) 
        {
            ligth.intensity = 2;
        }
        RenderSettings.ambientLight = Color.white;
        /*GameObject[]*/ var objects = GameObject.FindGameObjectsWithTag("Highlight");
        foreach (var obj in objects)
        {
            Material transparentMaterial = Resources.Load("Materials/Transparent_Material", typeof(Material)) as Material;
            obj.GetComponent<Renderer>().material = transparentMaterial;
        }
    }

    public void UnHighlightIcons()
    {
        Light[] ligths = FindObjectsOfType(typeof(Light)) as Light[];
        foreach (Light ligth in ligths) 
        {
            ligth.intensity = 2;
        }
        RenderSettings.ambientLight = Color.white;
        //unhighlight assignments
        /*GameObject[]*/ var assignmentObjects = GameObject.FindGameObjectsWithTag("Assignment");
        foreach (var obj in assignmentObjects)
        {
            Material transparentMaterial = Resources.Load("Materials/Assignment_Material", typeof(Material)) as Material;
            obj.GetComponent<Renderer>().material = transparentMaterial;
        }
        //unhighlight assessments
        /*GameObject[]*/ var assessmnetObjects = GameObject.FindGameObjectsWithTag("Assessment");
        foreach (var obj in assessmnetObjects)
        {
            Material transparentMaterial = Resources.Load("Materials/test_Material", typeof(Material)) as Material;
            obj.GetComponent<Renderer>().material = transparentMaterial;
        }


    }


    public void changeHighlight(GameObject selectedObject)
    {
        // loop through highlights to find the one were merging
        GameObject highlight = findHighlightWithinBounds(selectedObject);
        
        string tag = selectedObject.tag;
        if(tag == "Gate Legend")
        {
            changeMaterial(highlight,Resources.Load("Materials/BRICK", typeof(Material)) as Material);
            //strip the old model attached
            Destroy(highlight.GetComponent<Highlighter>());
            //change it into a Gate
            highlight.GetComponent<MeshFilter>().mesh = Resources.Load("Materials/Fortress_Gate", typeof(Mesh)) as Mesh;
            //scale and rotate
            highlight.transform.localScale = new Vector3(10f,10f,10f);
            highlight.transform.rotation = Quaternion.Euler(new Vector3(-90f,0f,0f));
            //add a new script
            highlight.AddComponent<Gate>();
            // add a new mesh collider
            highlight.GetComponent<MeshCollider>().sharedMesh = Resources.Load("Materials/Fortress_Gate", typeof(Mesh)) as Mesh;
            //update the position in the model (only need to do x,z)
            Vector3 position = highlight.transform.position;
            highlight.GetComponent<Gate>().x = position.x;
            highlight.GetComponent<Gate>().z = position.z;
            highlight.tag = "Gate";  
            //now assign the level number to the gate
            highlight.GetComponent<Gate>().LevelNum = GetGateLevelNumber();         
        }
        else if(tag == "Test Legend")
        {
            changeMaterial(highlight,Resources.Load("Materials/Test_Material", typeof(Material)) as Material);
            Destroy(highlight.GetComponent<Highlighter>());
            highlight.AddComponent<AssessmentAttributes>();
            Vector3 position = highlight.transform.position;
            highlight.GetComponent<AssessmentAttributes>().x = position.x;
            highlight.GetComponent<AssessmentAttributes>().z = position.z;
            highlight.tag = "Assessment";          
        }
        else if(tag == "Assignment Legend")
        {
            // Material[] materials = {Resources.Load("Materials/ThickBook", typeof(Material)) as Material,
            // Resources.Load("Materials/ThickBookPages", typeof(Material)) as Material};
            changeMaterial(highlight,Resources.Load("Materials/Assignment_Material", typeof(Material)) as Material);

            //change it to book
            Destroy(highlight.GetComponent<Highlighter>());
            highlight.AddComponent<AssignmentAttributes>();
            Vector3 position = highlight.transform.position;
            highlight.GetComponent<AssignmentAttributes>().x = position.x;
            highlight.GetComponent<AssignmentAttributes>().z = position.z;
            highlight.tag = "Assignment";            
        }
    }

    //simple function that loops through gates and gets the appropriate level number
    // in order to assign it to a newly placed gate
    public int GetGateLevelNumber()
    {
        int index = 0;
        /*GameObject[]*/ var objects = GameObject.FindGameObjectsWithTag("Gate");
        foreach (var highlights in objects)
        {
            index++;
        }
        return index;
    }

    public int GetLevelNumber(GameObject iconObj)
    {
        if(iconObj.tag == "Assignment")
        {

        }
        else if(iconObject.tag == "Assessment")
        {

        }
        return 0;
    }

    // simple fuynction that loops through highlighted objects to check if
    // selected object is in a highlighters(x,y,z)
    // Returns: highlight gameObject that is found
    public GameObject findHighlightWithinBounds(GameObject selectedObject)
    {
        /*GameObject[]*/ var objects = GameObject.FindGameObjectsWithTag("Highlight");
        foreach (var highlights in objects)
        {
                //increase number to widen search range for highlight object-------------------------____<
              if(Vector3.Distance(highlights.transform.position, selectedObject.transform.position) < 75)
              {
                return highlights;
              }
        }     
        return null;   

    }
    // simple fuynction that loops through highlighted objects to check if 
    // selected object is in the highlighters(x,yz)
    // Returns: BOOLEAN
    public bool checkWithinBounds(GameObject selectedObject)
    {
        /*GameObject[]*/ var objects = GameObject.FindGameObjectsWithTag("Highlight");
        foreach (var highlights in objects)
        {
                //increase number to widen search range for highlight object-------------------------____<
              if(Vector3.Distance(highlights.transform.position, selectedObject.transform.position) < 75)
              {
                return true;
              }
        }
        return false;
    }

    public GameObject getObjectByLocation(Vector3 location)
    {
        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
          if(Vector3.Distance(go.transform.position, location) < 75)
          {
            return go;

          }
        }
        return null;
    }

    //this function is to return the location of the object you selected
    public Vector3 checkTagLocation(GameObject selectedObject)
    {
        if(selectedObject.gameObject.tag == "Gate Legend")
        {

           return new Vector3(selectedObject.GetComponent<Gate>().x, 0,selectedObject.GetComponent<Gate>().z ) ;

        }

        else if(selectedObject.gameObject.tag == "Assignment Legend")
        {
             return new Vector3(selectedObject.GetComponent<Assignment>().x, 0,selectedObject.GetComponent<Assignment>().z) ;

        }
        else if(selectedObject.gameObject.tag == "Test Legend")
        {
             return new Vector3(selectedObject.GetComponent<Test>().x, 0,selectedObject.GetComponent<Test>().z) ;

        }
        return new Vector3(0,0,0);
    }

    public void changeMaterial(GameObject gameobject, Material material)
    {
        gameobject.GetComponent<MeshRenderer>().sharedMaterial = material;

    }

    public void flashWithHighlight(string name)
    {
        GameObject someObj = GameObject.Find(name);
       
        StartCoroutine(pauseTime(.09f,someObj));


       
        
    }

    private IEnumerator pauseTime(float time, GameObject gameObject)
    {
        Material original = gameObject.GetComponent<Renderer>().material;
        changeMaterial(gameObject, Resources.Load("Materials/Glow_Material", typeof(Material)) as Material);
        yield return new WaitForSeconds(time);
        changeMaterial(gameObject, original);

    }

    public void LoadCorrectScene(GameObject gameObject, GameObject otherObject)
    {
        if(gameObject.tag == "EditButtonIcon" && otherObject.tag == "Assignment")
        {
            // get object location to move camera over
            Vector3 position = gameObject.transform.position;
            position.y = position.y-100;
            Camera.main.transform.position = position;
            SceneManager.LoadScene("EditAssignmentIcon", LoadSceneMode.Additive);

        }
        else if(gameObject.tag == "EditButtonIcon" && otherObject.tag == "Gate")
        {

        }
        else if(gameObject.tag == "EditButtonIcon" && otherObject.tag == "Assessment")
        {
            // get object location to move camera over
            Vector3 position = gameObject.transform.position;
            position.y = position.y-100;
            Camera.main.transform.position = position;
            SceneManager.LoadScene("AssessmentPage", LoadSceneMode.Additive);
        }
    }

    public void changeMesh(GameObject objectToBeEdited)
    {
        if(objectToBeEdited.tag == "Gate")
        {
            objectToBeEdited.GetComponent<MeshFilter>().mesh = tempMesh;
            objectToBeEdited.GetComponent<MeshCollider>().sharedMesh = null;
            objectToBeEdited.GetComponent<MeshCollider>().sharedMesh = tempMesh;
            objectToBeEdited.transform.localScale = new Vector3 (100f, 100f, 100f);
            Debug.Log(tempMesh.name);
        }
    }

    public void StripAttributeScript(GameObject objectToBeEdited)
    {
        if(objectToBeEdited.tag == "Gate")
        {
            Destroy(objectToBeEdited.GetComponent<Gate>());
        }
        else if(objectToBeEdited.tag == "Assignment")
        {
            Destroy(objectToBeEdited.GetComponent<AssignmentAttributes>());
        }
        else if(objectToBeEdited.tag == "Assessment")
        {
            Destroy(objectToBeEdited.GetComponent<AssessmentAttributes>());
        }
    }

    public void DeleteFileAtLocation(string location)
    {
        string FilePath = Application.persistentDataPath + "/" + PlayerPrefs.GetString("ClassTag")+ "/" + location + ".json";
        Debug.Log(FilePath);
        File.Delete(FilePath);
    }

    // public void CallUpload()
    // {
    //     StartCoroutine(UploadToDB());
    // }

    // IEnumerator UploadToDB()
    // {
    //     //JsonObject.MODULE.ASSESSMENTS.QUESTION.questions[PlayerPrefs.GetInt("QuestionNumber") - 1];
    //     string postURL = "http://localhost/UnityApp/Upload.php";
    //     string FilePath = Application.persistentDataPath + "/UploadData/";
    //     foreach(string file in files)
    //     {
    //         FilePath += file;
    //         using( var fs = File.Open(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
    //         {
    //             using(var sr = new StreamReader(fs))
    //             {
    //             var json = sr.ReadToEnd();                                                                                                                                                                                                                                                                                                                                  
    //             var JsonObject = JsonUtility.FromJson<CourseData>(json);
    //     List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
    //     wwwForm.Add(new MultipartFormDataSection("NameOfAssessment", JsonObject.MODULE.ASSESSMENT.AssessmentName));
    //     wwwForm.Add(new MultipartFormDataSection("DueDate", JsonObject.MODULE.ASSESSMENT.DueDate));
    //     foreach(QuestionObj question in JsonObject.MODULE.ASSESSMENT.QUESTION.questions)
    //     {
    //         wwwForm.Add(new MultipartFormDataSection("QuestionNumber", question));
    //         wwwForm.Add(new MultipartFormDataSection("QuestionName", question));
    //         wwwForm.Add(new MultipartFormDataSection("QuestionType", question));

    //         foreach(MultipleChoice choice in question.multipleChoices)
    //         {
    //             wwwForm.Add(new MultipartFormDataSection("AnswerNumber", choice.AnswerNumber));
    //             wwwForm.Add(new MultipartFormDataSection("AnswerNumber", choice.AnswerName));
    //             wwwForm.Add(new MultipartFormDataSection("AnswerNumber", choice.CorrectAnswer));
    //             wwwForm.Add(new MultipartFormDataSection("AnswerNumber", choice.Points));
    //         }
    //         foreach(Fill_in_Blank choice in question.fillBlankChoices)
    //         {
    //             wwwForm.Add(new MultipartFormDataSection("AnswerNumber", choice.QuestionName));
    //             wwwForm.Add(new MultipartFormDataSection("AnswerNumber", choice.QuestionAnswer));
               
    //         }
    //     }
    //     using (UnityWebRequest www = UnityWebRequest.Post(postURL, wwwForm))
    //     {
    //         yield return www.SendWebRequest();
    //         //string text = www.downloadHandler.text;
    //         // string[] response = text.Split(' ');
    //         // string Role = response[0];
    //         // string Username = response[1];
    //         // string Password = response[2];
    //         // Debug.Log(Role);
    //         // if (www.result == UnityWebRequest.Result.Success)
    //         // {
    //         //     if(nameField.text == Username && passwordField.text == Password)
    //         //     {
    //         //         DbManager.username = Username;

    //         //         DbManager.Role = Role;

    //         //         if(Role == "superadmin")
    //         //         {
    //         //             UnityEngine.SceneManagement.SceneManager.LoadScene(4);
    //         //         }
    //         //         else if(Role == "admin")
    //         //         {
    //         //             UnityEngine.SceneManagement.SceneManager.LoadScene(6);
    //         //         }
    //         //         else
    //         //         {
    //         //         UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    //         //         }

    //         //     }

    //         // }
    //         // else
    //         // {
    //         //     Debug.Log(nameField.text);
    //         //     Debug.Log(passwordField.text);
    //         //     Debug.Log("User Log in failed:" + text);
    //         // }
    //     }
                


    //             }
    //         }
    //     }
    // }

   

    public void viewCourseButton()
    {
        
    }
    //-----------------------FILE DOWNLOAD SECTION---------------------//
    //-----------------------------------------------------------------//
    public void SaveGate(GameObject gateObject)
    {
        SaveDataHandler save = new SaveDataHandler();

        save._Course.CourseName = PlayerPrefs.GetString("ClassTag");

        save._Course.Type = "Gate";

        save._Course.MODULE.moduleNumber = gateObject.GetComponent<Gate>().LevelNum;

        PlayerPrefs.SetString("FilePath", Application.persistentDataPath + "/" + PlayerPrefs.GetString("ClassTag") +"/"
        + gateObject.transform.position.x.ToString() +"." 
        + gateObject.transform.position.y.ToString() + "."
        + gateObject.transform.position.z.ToString() +".json");

        save.SaveGateIntoJson();
        Debug.Log("Save Gate Called");


    }


    //------------------------FILE UPLOAD SECTION-----------------------//
    //------------------------------------------------------------------//

    //this function loads objects if they exist in your local folder already
    public void LoadObjectsFromJson()
    {

        Vector3 location;
        string folderPath = Application.persistentDataPath + "/" + PlayerPrefs.GetString("ClassTag");
        string[] locationArray;
        string[] nameOfFiles = LoopThroughFolder(folderPath);
        GameObject parent;

        //this is a long loop that calls multiple methods
        // it spawns in the objects in the scene based on the file location
        foreach(string file in nameOfFiles)
        {
            locationArray = SplitNameToGetLocation(file);
            using (var fs = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var sr = new StreamReader(fs))
                {
                    var json = sr.ReadToEnd();
                    var JsonObject = JsonUtility.FromJson<SaveDataHandler.CourseData>(json);
                    string typeOfObject = JsonObject.Type;
                    int levelNum = JsonObject.MODULE.moduleNumber;
                    location = new Vector3(int.Parse(locationArray[0]),int.Parse(locationArray[1]), int.Parse(locationArray[2]));
                    GameObject newObject =  Instantiate(objFromLoad, location, Quaternion.identity);
                    newObject.tag = typeOfObject;
                    ChangeNewObjectSpawned(newObject, levelNum);
                    newObject.transform.position = location;
                    spawn3HighlightsAroundPositon(location, newObject);
                    //now check to see if it spawned on a highlight
                    foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                    {
                        if (Vector3.Distance(go.transform.position, location) < 75)
                        {
                            if( go.tag == "Highlight")
                            {
                                //GameObject highlight = getObjectByLocation(location);
                                Destroy(go);
                            }
                        }
                    }
                }
            }
        }
    }

    public string[] LoopThroughFolder(string folderPath)
    {
        //Debug.Log(PlayerPrefs.GetString("ClassTag"));
        string[] files = Directory.GetFiles(folderPath, "*.json", SearchOption.AllDirectories);
        return files;
        

    }

    //this will return the file name split with location
    public string[] SplitNameToGetLocation(string FileName)
    {
        string[] returnArray;
        string stringWithJson = FileName.Split('/').Last();
        stringWithJson = stringWithJson.Split('\\')[1];
        string locationName = stringWithJson.Split(new string[] { ".json" }, StringSplitOptions.None)[0];
        returnArray = locationName.Split('.');
        return returnArray;
    }


    // This function is like change highlight, but it doesn't check
    // for highlight to be replaced
    public void ChangeNewObjectSpawned(GameObject newObject, int levelNum)
    {
        string tag = newObject.tag;
        if (tag == "Gate")
        {
            changeMaterial(newObject, Resources.Load("Materials/BRICK", typeof(Material)) as Material);
            //strip the old model attached
            Destroy(newObject.GetComponent<Highlighter>());
            //change it into a Gate
            newObject.GetComponent<MeshFilter>().mesh = Resources.Load("Materials/Fortress_Gate", typeof(Mesh)) as Mesh;
            //scale and rotate
            newObject.transform.localScale = new Vector3(10f, 10f, 10f);
            newObject.transform.rotation = Quaternion.Euler(new Vector3(-90f,0f,0f));
            //add a new script
            newObject.AddComponent<Gate>();
            newObject.GetComponent<Gate>().LevelNum = levelNum;
            // add a new mesh collider
            newObject.GetComponent<MeshCollider>().sharedMesh = Resources.Load("Materials/Fortress_Gate", typeof(Mesh)) as Mesh;
            //update the position in the model (only need to do x,z)
            Vector3 position = newObject.transform.position;
            newObject.GetComponent<Gate>().x = position.x;
            newObject.GetComponent<Gate>().z = position.z;
        }
        else if (tag == "Assessment")
        {
            changeMaterial(newObject, Resources.Load("Materials/Test_Material", typeof(Material)) as Material);
            Destroy(newObject.GetComponent<Highlighter>());
            newObject.AddComponent<AssessmentAttributes>();
            Vector3 position = newObject.transform.position;
            newObject.GetComponent<AssessmentAttributes>().x = position.x;
            newObject.GetComponent<AssessmentAttributes>().z = position.z;
            newObject.GetComponent<AssessmentAttributes>().LevelNum = levelNum;
        }
        else if (tag == "Assignment")
        {
            // Material[] materials = {Resources.Load("Materials/ThickBook", typeof(Material)) as Material,
            // Resources.Load("Materials/ThickBookPages", typeof(Material)) as Material};
            changeMaterial(newObject, Resources.Load("Materials/Assignment_Material", typeof(Material)) as Material);

            //change it to book
            Destroy(newObject.GetComponent<Highlighter>());
            newObject.AddComponent<AssignmentAttributes>();
            Vector3 position = newObject.transform.position;
            newObject.GetComponent<AssignmentAttributes>().x = position.x;
            newObject.GetComponent<AssignmentAttributes>().z = position.z;
            newObject.GetComponent<AssignmentAttributes>().LevelNum = levelNum;
        }
    }

    public string ConvertLocationToString(Vector3 location)
    {
        string returnString = "";
        returnString = location.x +"."+location.y+"."+location.z;
        return returnString;
    }
}

    


