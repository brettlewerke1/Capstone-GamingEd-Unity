using UnityEngine;

public class Grabber : MonoBehaviour {

    private GameObject selectedObject;

    public Icon iconObject;

    private void Start()
    {
        iconObject = new Icon();
    }
    
    private void Update() {

        if (Input.GetMouseButtonDown(0)) {
            if(selectedObject == null) {
                RaycastHit hit = CastRay();

                if(hit.collider != null) {
                    if (hit.collider.CompareTag("Gate")) 
                    {
                        selectedObject = hit.collider.gameObject;
                        iconObject = iconObject.getSpawnCoords("Gate");
                        Debug.Log(iconObject.x);
                        Cursor.visible = false;
                    }
                    else if (hit.collider.CompareTag("Assignment")) 
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

    public void spawnObject(GameObject Object, int x, int y, int z)
    {
        var newSquare = Instantiate(
            Object, 
            new Vector3(x, y, z), 
            Quaternion.identity);
    }
}
