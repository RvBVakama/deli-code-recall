using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        tex = new WebCamTexture(device.name, 1080, 1920, 5);
        display.texture = tex;
        tex.Play();
    }

    int capCount;

    public void Capture()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/images/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/images/");
        }

        Texture2D shot = new Texture2D(tex.width, tex.height);
        shot.SetPixels(tex.GetPixels());
        shot.Apply();

        System.IO.File.WriteAllBytes(Application.persistentDataPath + "/images/" + capCount.ToString() + ".png", shot.EncodeToPNG());
        ++capCount;
    }

    public void SendPhotoMail()
    {
        SendEmail.SendMail("0");
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
        tex = new WebCamTexture(device.name, 1080, 1920, 5);
        display.texture = tex;
        tex.Play();
    }
}