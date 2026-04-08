using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    // ref video: https://www.youtube.com/watch?v=_LRZcmX_xw0 

    private Camera cam;

    public bool useStaticBillboard;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!useStaticBillboard)
        {
            Vector3 lookat = new Vector3(cam.transform.rotation.x, cam.transform.rotation.y + 180, cam.transform.rotation.z);

            transform.LookAt(lookat);
        } else
        {
            transform.rotation = cam.transform.rotation;
        }
        
    }
}
