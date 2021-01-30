using System;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    void Start()
    {
        CameraSetup();
        LightSetup();
    }

    private void LightSetup()
    {
        GameObject dirLight = GameObject.Find("Directional Light"); // Grabs the light from the scene and stores it as a reference.
        dirLight.transform.eulerAngles = new Vector3(50, -30, 0); // It sets the rotation of the light with EulerAngles.
        dirLight.GetComponent<Light>().color = new Color32(152, 204, 255, 255); // Finally, it changes the light's color.
    }

    private void CameraSetup()
    {
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera").gameObject;
        camera.transform.position = new Vector3(0, 0, -300f);
        camera.transform.eulerAngles = new Vector3(0, 0, 0);

        camera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor; // removes the skybox and set a solid color
        camera.GetComponent<Camera>().backgroundColor = new Color(0, 0, 0, 255); // change the solid color to black
    }
}
