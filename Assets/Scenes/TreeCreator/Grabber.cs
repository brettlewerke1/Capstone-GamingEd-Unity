using UnityEngine;
<<<<<<< Updated upstream
=======
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

>>>>>>> Stashed changes

public class Grabber : MonoBehaviour {

    private GameObject selectedObject;

    public Icon iconObject;

    private void Start()
    {
<<<<<<< Updated upstream
        iconObject = new Icon();
=======
        iconObject = gameObject.AddComponent<Icon>();
        DontDestroyOnLoad(iconObject);

        ///DELETE ME
        PlayerPrefs.SetString("CourseName", "CS486");

        //something = validLocationList;
        Debug.Log("Awake() is called");

>>>>>>> Stashed changes
    }
    
    private void Update() {

        if (Input.GetMouseButtonDown(0)) {
            if(selectedObject == null) {
                RaycastHit hit = CastRay();

                if(hit.collider != null) {
<<<<<<< Updated upstream
                    if (hit.collider.CompareTag("Gate")) 
=======

                    Debug.Log(hit.collider.gameObject.transform.position.x.ToString());
                    if (hit.collider.gameObject.name == "EditButtonIcon") 
                    {
                        //tempObject = hit.collider.gameObject;
                        flashWithHighlight("EditButtonIcon");
                        LoadCorrectScene(hit.collider.gameObject, objectToBeEdited);
                        selectedObject = null;
                    }
                    else if (hit.collider.gameObject.name == "DeleteButtonIcon") 
                    {
                        flashWithHighlight("DeleteButtonIcon");
                        // change the object to be selected back to highlight
                        // remove all highlight nodes around it
                        changeMaterial(objectToBeEdited, Resources.Load("Materials/Transparent_Material", typeof(Material)) as Material);
                        objectToBeEdited.tag = "Highlight";
                        de_spawnHighlight(objectToBeEdited.transform.position);

                        selectedObject = null;
                    }
                    else if (hit.collider.CompareTag("Gate Legend")) 
>>>>>>> Stashed changes
                    {
                        selectedObject = hit.collider.gameObject;
                        iconObject = iconObject.getSpawnCoords("Gate");
                        Debug.Log(iconObject.x);
                        Cursor.visible = false;
<<<<<<< Updated upstream
                    }
                    else if (hit.collider.CompareTag("Assignment")) 
=======

                        Debug.Log("Im hiot");

                    }  
                    else if (hit.collider.CompareTag("Assignment Legend")) 
>>>>>>> Stashed changes
                    {
                        selectedObject = hit.collider.gameObject;
                        iconObject = iconObject.getSpawnCoords("Assignment");
                        Cursor.visible = false;                      
                    }
                    else if (hit.collider.CompareTag("Test")) 
                    {
                        selectedObject = hit.collider.gameObject;
                        iconObject = iconObject.getSpawnCoords("Test");
                        Cursor.visible = false;                      
                    }                   
                    else{
                        return;
                    }
<<<<<<< Updated upstream
=======
                    else if(hit.collider.CompareTag("Upload"))
                    {
                        selectedObject = hit.collider.gameObject;
                        UploadToDB();

                    }
                    else
                    {
                    //if you click anywhere...the edit/delete should go away
                    tempObject = GameObject.Find("EditButtonIcon");
>>>>>>> Stashed changes


                }
            } else {
                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
                spawnObject(selectedObject, iconObject.x, iconObject.y, iconObject.z);
                                    
                selectedObject.transform.position = new Vector3(worldPosition.x, 0f, worldPosition.z);

                selectedObject = null;
                Cursor.visible = true;
            }
        }

        if(selectedObject != null) {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            selectedObject.transform.position = new Vector3(worldPosition.x, .25f, worldPosition.z);
<<<<<<< Updated upstream
=======
        }
        //-----------------EDIT ICON----------------//
        //
        //------------------------------------------//

        if(Input.GetMouseButtonDown(1))
        {
            RaycastHit hit = CastRay();
            if(hit.collider != null) 
            {
                    PlayerPrefs.SetString("FilePath", Application.persistentDataPath + "/UploadData/" 
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
                else if(hit.collider.CompareTag("Test"))
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
>>>>>>> Stashed changes


            if (Input.GetMouseButtonDown(1)) {
                selectedObject.transform.rotation = Quaternion.Euler(new Vector3(
                    selectedObject.transform.rotation.eulerAngles.x,
                    selectedObject.transform.rotation.eulerAngles.y + 90f,
                    selectedObject.transform.rotation.eulerAngles.z));
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
<<<<<<< Updated upstream

    public void spawnObject(GameObject Object, int x, int y, int z)
    {
        var newSquare = Instantiate(
            Object, 
            new Vector3(x, y, z), 
            Quaternion.identity);
=======
    public void spawnHighlight(Vector3 position, GameObject selectedObject)
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
        Debug.Log("highlight spawned");

    }

    public void highlightAreas()
    {
        Light[] ligths = FindObjectsOfType(typeof(Light)) as Light[];
        foreach (Light ligth in ligths) 
        {
            ligth.enabled = false;
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
            ligth.enabled = true;
        }
        RenderSettings.ambientLight = Color.white;
        /*GameObject[]*/ var objects = GameObject.FindGameObjectsWithTag("Highlight");
        foreach (var obj in objects)
        {
            Material transparentMaterial = Resources.Load("Materials/Transparent_Material", typeof(Material)) as Material;
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
            highlight.transform.Rotate(-90f,0f,0f);
            //add a new script
            highlight.AddComponent<Gate>();
            // add a new mesh collider
            highlight.GetComponent<MeshCollider>().sharedMesh = Resources.Load("Materials/Fortress_Gate", typeof(Mesh)) as Mesh;
            //update the position in the model (only need to do x,z)
            Vector3 position = highlight.transform.position;
            highlight.GetComponent<Gate>().x = position.x;
            highlight.GetComponent<Gate>().z = position.z;
            highlight.tag = "Gate";           
        }
        else if(tag == "Test Legend")
        {
            changeMaterial(highlight,Resources.Load("Materials/Test_Material", typeof(Material)) as Material);
            Destroy(highlight.GetComponent<Highlighter>());
            highlight.AddComponent<Test>();
            Vector3 position = highlight.transform.position;
            highlight.GetComponent<Test>().x = position.x;
            highlight.GetComponent<Test>().z = position.z;
            highlight.tag = "Test";          
        }
        else if(tag == "Assignment Legend")
        {
            // Material[] materials = {Resources.Load("Materials/ThickBook", typeof(Material)) as Material,
            // Resources.Load("Materials/ThickBookPages", typeof(Material)) as Material};


            changeMaterial(highlight,Resources.Load("Materials/Assignment_Material", typeof(Material)) as Material);

            //change it to book
            Destroy(highlight.GetComponent<Highlighter>());
            highlight.AddComponent<Assignment>();
            Vector3 position = highlight.transform.position;
            highlight.GetComponent<Assignment>().x = position.x;
            highlight.GetComponent<Assignment>().z = position.z;
            highlight.tag = "Assignment";            
        }
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
        gameobject.GetComponent<Renderer>().material = material;

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
            SceneManager.LoadScene(11, LoadSceneMode.Additive);

        }
        else if(gameObject.tag == "EditButtonIcon" && otherObject.tag == "Gate")
        {

        }
        else if(gameObject.tag == "EditButtonIcon" && otherObject.tag == "Test")
        {
            // get object location to move camera over
            Vector3 position = gameObject.transform.position;
            position.y = position.y-100;
            Camera.main.transform.position = position;
            SceneManager.LoadScene("AssessmentPage", LoadSceneMode.Additive);
        }
    }

    public void CallUpload()
    {
        StartCoroutine(UploadToDB());
    }

    IEnumerator UploadToDB()
    {
        //JsonObject.MODULE.ASSESSMENTS.QUESTION.questions[PlayerPrefs.GetInt("QuestionNumber") - 1];
        string postURL = "http://localhost/UnityApp/Upload.php";
        string FilePath = Application.persistentDataPath + "/UploadData/";
        foreach(string file in files)
        {
            FilePath += file;
            using( var fs = File.Open(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using(var sr = new StreamReader(fs))
                {
                var json = sr.ReadToEnd();                                                                                                                                                                                                                                                                                                                                  
                var JsonObject = JsonUtility.FromJson<CourseData>(json);
        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        wwwForm.Add(new MultipartFormDataSection("NameOfAssessment", JsonObject.MODULE.ASSESSMENT.AssessmentName));
        wwwForm.Add(new MultipartFormDataSection("DueDate", JsonObject.MODULE.ASSESSMENT.DueDate));
        foreach(QuestionObj question in JsonObject.MODULE.ASSESSMENT.QUESTION.questions)
        {
            wwwForm.Add(new MultipartFormDataSection("QuestionNumber", question));
            wwwForm.Add(new MultipartFormDataSection("QuestionName", question));
            wwwForm.Add(new MultipartFormDataSection("QuestionType", question));

            foreach(MultipleChoice choice in question.multipleChoices)
            {
                wwwForm.Add(new MultipartFormDataSection("AnswerNumber", choice.AnswerNumber));
                wwwForm.Add(new MultipartFormDataSection("AnswerNumber", choice.AnswerName));
                wwwForm.Add(new MultipartFormDataSection("AnswerNumber", choice.CorrectAnswer));
                wwwForm.Add(new MultipartFormDataSection("AnswerNumber", choice.Points));
            }
            foreach(Fill_in_Blank choice in question.fillBlankChoices)
            {
                wwwForm.Add(new MultipartFormDataSection("AnswerNumber", choice.QuestionName));
                wwwForm.Add(new MultipartFormDataSection("AnswerNumber", choice.QuestionAnswer));
               
            }
        }
        using (UnityWebRequest www = UnityWebRequest.Post(postURL, wwwForm))
        {
            yield return www.SendWebRequest();
            //string text = www.downloadHandler.text;
            // string[] response = text.Split(' ');
            // string Role = response[0];
            // string Username = response[1];
            // string Password = response[2];
            // Debug.Log(Role);
            // if (www.result == UnityWebRequest.Result.Success)
            // {
            //     if(nameField.text == Username && passwordField.text == Password)
            //     {
            //         DbManager.username = Username;

            //         DbManager.Role = Role;

            //         if(Role == "superadmin")
            //         {
            //             UnityEngine.SceneManagement.SceneManager.LoadScene(4);
            //         }
            //         else if(Role == "admin")
            //         {
            //             UnityEngine.SceneManagement.SceneManager.LoadScene(6);
            //         }
            //         else
            //         {
            //         UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            //         }

            //     }

            // }
            // else
            // {
            //     Debug.Log(nameField.text);
            //     Debug.Log(passwordField.text);
            //     Debug.Log("User Log in failed:" + text);
            // }
        }
                


                }
            }
        }
    }

    public string[] loopThroughFolder()
    {
        string folderPath = Application.persistentDataPath + "/UploadData";
        string[] files = 
        Directory.GetFiles(folderPath, "*ProfileHandler.cs", SearchOption.AllDirectories);
        

>>>>>>> Stashed changes
    }

}
