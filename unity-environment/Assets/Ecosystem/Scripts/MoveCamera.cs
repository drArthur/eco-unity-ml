// Credit to damien_oconnell from http://forum.unity3d.com/threads/39513-Click-drag-camera-movement
// for using the mouse displacement for calculating the amount of camera movement and panning code.

using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{

    public float panSpeed = 4.0f;       // Speed of the camera when being panned
    public bool Enabled;
    //
    // UPDATE
    //

    void Update()
    {
       if(Enabled)
        {

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                this.transform.Translate(Vector3.left * panSpeed * Time.deltaTime);
            }
            else if(Input.GetKey(KeyCode.RightArrow))
            {
                this.transform.Translate(Vector3.right * panSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                this.transform.Translate(Vector3.up * panSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                this.transform.Translate(Vector3.down * panSpeed * Time.deltaTime);
            }
        }
    }
}