using System;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    GameObject mainCamera;
    GameObject dirLight;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").gameObject;
        dirLight = GameObject.Find("Directional Light"); // Grabs the light from the scene and stores it as a reference.
        CameraSetup();
        LightSetup();
    }

    private void LightSetup()
    {
        dirLight.transform.eulerAngles = new Vector3(50, -30, 0); // It sets the rotation of the light with EulerAngles.
        dirLight.GetComponent<Light>().color = new Color32(152, 204, 255, 255); // Finally, it changes the light's color.
    }

    private void CameraSetup()
    {
        //Camera Transform
        mainCamera.transform.position = new Vector3(0, 0, -300f);
        mainCamera.transform.eulerAngles = new Vector3(0, 0, 0);

        //Camera Properties
        mainCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor; // removes the skybox and set a solid color
        mainCamera.GetComponent<Camera>().backgroundColor = new Color(0, 0, 0, 255); // change the solid color to black
    }
}
