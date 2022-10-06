using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    GameObject selectedDirection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if statement to move camera by mouse click on button
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit = CastRay();
            if(hit.collider!=null)
            {
                if(hit.collider.CompareTag("Down"))
                {
                    selectedDirection = hit.collider.gameObject;
                    Vector3 camlocation = Camera.main.transform.position;
                    Vector3 objectlocation = selectedDirection.transform.position;
                    camlocation.z=camlocation.z-1;
                    objectlocation.z = objectlocation.z-1;
                    Camera.main.transform.position = camlocation;
                    selectedDirection.transform.position = objectlocation;
                    updateOtherDirectionObjects("Down");


                }
                else if(hit.collider.CompareTag("Up"))
                {
                    selectedDirection = hit.collider.gameObject;
                    Vector3 camlocation = Camera.main.transform.position;
                    Vector3 objectlocation = selectedDirection.transform.position;
                    camlocation.z=camlocation.z+1;
                    objectlocation.z = objectlocation.z+1;
                    Camera.main.transform.position = camlocation;
                    selectedDirection.transform.position = objectlocation;
                    updateOtherDirectionObjects("Up");


                }
                else if(hit.collider.CompareTag("Right"))
                {
                    selectedDirection = hit.collider.gameObject;
                    Vector3 camlocation = Camera.main.transform.position;
                    Vector3 objectlocation = selectedDirection.transform.position;
                    camlocation.x=camlocation.x+1;
                    objectlocation.x = objectlocation.x+1;
                    Camera.main.transform.position = camlocation;
                    selectedDirection.transform.position = objectlocation;
                    updateOtherDirectionObjects("Right");


                }
                else if(hit.collider.CompareTag("Left"))
                {
                    selectedDirection = hit.collider.gameObject;
                    Vector3 camlocation = Camera.main.transform.position;
                    Vector3 objectlocation = selectedDirection.transform.position;
                    camlocation.x=camlocation.x-1;
                    objectlocation.x = objectlocation.x-1;
                    Camera.main.transform.position = camlocation;
                    selectedDirection.transform.position = objectlocation;
                    updateOtherDirectionObjects("Left");


                }

            }
        }
        //DOWN
        else if(Input.GetKey(KeyCode.S))
        {
            selectedDirection = GameObject.FindWithTag("Down");
            Vector3 camlocation = Camera.main.transform.position;
            Vector3 objectlocation = selectedDirection.transform.position;
            camlocation.z=camlocation.z-1;
            objectlocation.z = objectlocation.z-1;
            Camera.main.transform.position = camlocation;
            selectedDirection.transform.position = objectlocation;
            updateOtherDirectionObjects("Down");
        }
        //UP
        else if(Input.GetKey(KeyCode.W))
        {
            selectedDirection = GameObject.FindWithTag("Up");
            Vector3 camlocation = Camera.main.transform.position;
            Vector3 objectlocation = selectedDirection.transform.position;
            camlocation.z=camlocation.z+1;
            objectlocation.z = objectlocation.z+1;
            Camera.main.transform.position = camlocation;
            selectedDirection.transform.position = objectlocation;
            updateOtherDirectionObjects("Up");
        }
        //RIGHT
        else if(Input.GetKey(KeyCode.D))
        {
            selectedDirection = GameObject.FindWithTag("Right");
            Vector3 camlocation = Camera.main.transform.position;
            Vector3 objectlocation = selectedDirection.transform.position;
            camlocation.x=camlocation.x+1;
            objectlocation.x = objectlocation.x+1;
            Camera.main.transform.position = camlocation;
            selectedDirection.transform.position = objectlocation;
            updateOtherDirectionObjects("Right");
        }
        //LEFT
        else if(Input.GetKey(KeyCode.A))
        {
            selectedDirection = GameObject.FindWithTag("Left");
            Vector3 camlocation = Camera.main.transform.position;
            Vector3 objectlocation = selectedDirection.transform.position;
            camlocation.x=camlocation.x-1;
            objectlocation.x = objectlocation.x-1;
            Camera.main.transform.position = camlocation;
            selectedDirection.transform.position = objectlocation;
            updateOtherDirectionObjects("Left");
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

    public void updateOtherDirectionObjects(string direction)
    {
        GameObject one;
        GameObject two;
        GameObject three;

        if(direction == "Down")
        {
            one = GameObject.FindWithTag("Up");
            two = GameObject.FindWithTag("Left");
            three = GameObject.FindWithTag("Right");
            //now move everyone down
            Vector3 up = one.transform.position;
            up.z = up.z-1;
            one.transform.position = up;

            Vector3 left = two.transform.position;
            left.z = left.z-1;
            two.transform.position = left;

            Vector3 right = three.transform.position;
            right.z = right.z-1;
            three.transform.position = right;


        }
        else if(direction == "Up")
        {
            one = GameObject.FindWithTag("Down");
            two = GameObject.FindWithTag("Left");
            three = GameObject.FindWithTag("Right");
            //now move everyone down
            Vector3 down = one.transform.position;
            down.z = down.z+1;
            one.transform.position = down;

            Vector3 left = two.transform.position;
            left.z = left.z+1;
            two.transform.position = left;

            Vector3 right = three.transform.position;
            right.z = right.z+1;
            three.transform.position = right;   
        }
        else if(direction == "Right")
        {
            one = GameObject.FindWithTag("Down");
            two = GameObject.FindWithTag("Left");
            three = GameObject.FindWithTag("Up");
            //now move everyone down
            Vector3 down = one.transform.position;
            down.x = down.x+1;
            one.transform.position = down;

            Vector3 left = two.transform.position;
            left.x = left.x+1;
            two.transform.position = left;

            Vector3 up = three.transform.position;
            up.x = up.x+1;
            three.transform.position = up;   
        }
        else if(direction == "Left")
        {
            one = GameObject.FindWithTag("Down");
            two = GameObject.FindWithTag("Right");
            three = GameObject.FindWithTag("Up");
            //now move everyone down
            Vector3 down = one.transform.position;
            down.x = down.x-1;
            one.transform.position = down;

            Vector3 right = two.transform.position;
            right.x = right.x-1;
            two.transform.position = right;

            Vector3 up = three.transform.position;
            up.x = up.x-1;
            three.transform.position = up;   
        }

    }
}
