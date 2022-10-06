using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Grabber : MonoBehaviour {

    private GameObject selectedObject;

    public GameObject highlightObject;

    public GameObject gateObject;

    public GameObject assignmentObject;

    public GameObject testObject;

    public Material tempMaterial;

    public Icon iconObject;

    public GameObject tempObject;

    public GameObject objectToBeEdited;

    //public int StopStartFunction = 0;

    private void Awake()
    {
        iconObject = gameObject.AddComponent<Icon>();
        DontDestroyOnLoad(iconObject);


        //something = validLocationList;
        Debug.Log("Awake() is called");

    }
    
    private void Update() {
        if (Input.GetMouseButtonDown(0)) {

            //--------------------------------------------------------//

            if(selectedObject == null) {
                RaycastHit hit = CastRay();


                if(hit.collider != null) {
                    if (hit.collider.gameObject.name == "EditButtonIcon") 
                    {
                        Debug.Log("Im hiot");
                        //tempObject = hit.collider.gameObject;
                        flashWithHighlight("EditButtonIcon");
                        //SceneManager.LoadScene(11, LoadSceneMode.Additive);
                        selectedObject = null;
                    }
                    else if (hit.collider.gameObject.name == "DeleteButtonIcon") 
                    {
                        flashWithHighlight("DeleteButtonIcon");
                        selectedObject = null;
                    }
                    else if (hit.collider.CompareTag("Gate Legend")) 
                    {
                        selectedObject = hit.collider.gameObject;
                        iconObject = iconObject.getSpawnCoords("Gate");
                        highlightAreas();
                        Cursor.visible = false;
                        //SceneManager.UnloadSceneAsync(11);
                        Debug.Log("Im hiot");

                    }  
                    else if (hit.collider.CompareTag("Assignment Legend")) 
                    {
                        selectedObject = hit.collider.gameObject;
                        iconObject = iconObject.getSpawnCoords("Assignment");
                        highlightAreas();
                        Cursor.visible = false;
                    }
                    else if (hit.collider.CompareTag("Test Legend")) 
                    {
                        selectedObject = hit.collider.gameObject;
                        iconObject = iconObject.getSpawnCoords("Test");
                        highlightAreas();
                        Cursor.visible = false;
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
            // this else statement happens to place an object
            }
            else if(checkWithinBounds(selectedObject)) {
                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
                spawnHighlight(findHighlightWithinBounds(selectedObject).transform.position, selectedObject);
                changeHighlight(selectedObject);
                //this moves it back into legend//
                // u must find tag of selected object u just dragged...and move accordingly
                selectedObject.transform.position = checkTagLocation(selectedObject);
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

        // this if statement moves the selected object around
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


            changeMaterial(highlight,Resources.Load("Materials/Assignment_Icon/books-book", typeof(Material)) as Material);

            //change it to book
            // Mesh mesh = Resources.Load("Materials/Cube.046", typeof(Mesh))as Mesh;
            // highlight.GetComponent<MeshFilter>().mesh = mesh;
            //highlight.transform.localScale = new Vector3(8000f,1200f,6500f);
            Destroy(highlight.GetComponent<Highlighter>());
            // change it to book
            highlight.GetComponent<MeshFilter>().mesh = Resources.Load("Materials/Assignment_Mesh", typeof(GameObject)) as Mesh;
            //scale and rotate
            highlight.transform.localScale = new Vector3(1000f,1000f,1000f);
            highlight.transform.Rotate(0f,0f,83f);
            //add new mesh collider
            highlight.GetComponent<MeshCollider>().sharedMesh = Resources.Load("Materials/Assignment_Mesh", typeof(GameObject)) as Mesh;
            //add new script
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

    public void changePosition(GameObject gameobject, float x, float y, float z)
    {
        
    }
}
