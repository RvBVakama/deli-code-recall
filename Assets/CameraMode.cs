using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMode : MonoBehaviour
{
    static WebCamTexture tex;
    private RawImage display;
    WebCamDevice device;
    private int whichCam = 0;

    void Start()
    {
        display = GetComponent<RawImage>();
        device = WebCamTexture.devices[whichCam];
        tex = new WebCamTexture(device.name, 1080, 1920, 60);
        display.texture = tex;
        tex.Play();
    }

    public void FlipCamera()
    {
        tex.Stop();

        if (whichCam > 1)
            whichCam = 0;
        else
            ++whichCam;

        if (whichCam == 1)
        {
            GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 90);
            GetComponent<RectTransform>().localScale = new Vector3(1, -1, 1);
        }
        else
        {
            GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -90);
            GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }

        device = WebCamTexture.devices[whichCam];
        tex = new WebCamTexture(device.name, 1080, 1920, 60);
        display.texture = tex;
        tex.Play();
    }
}