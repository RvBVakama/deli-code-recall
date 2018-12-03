using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

// to store the name of the product and the value it is
public struct Items
{
    public string itemName;
    public float itemCode;
}

public class gameLogic : MonoBehaviour
{
    public GameObject go;

    // the number of rounds in a session
    private int m_nRounds = 0;

    // list of items in list.txt
    Items[] Items;

    // access to the three buttons
    Button button1, button2, button3 = null;

    private void OnEnable()
    {
        // listereners on each button
        button1 = GameObject.Find("Game").transform.GetChild(1).GetComponent<Button>();
        button1.onClick.AddListener(Button1Clicked);
        button2 = GameObject.Find("Game").transform.GetChild(2).GetComponent<Button>();
        button2.onClick.AddListener(Button2Clicked);
        button3 = GameObject.Find("Game").transform.GetChild(3).GetComponent<Button>();
        button3.onClick.AddListener(Button3Clicked);

        LoadList();
    }

    public void Button1Clicked()
    {
        Debug.Log("1");
    }

    public void Button2Clicked()
    {
        Debug.Log("2");
    }

    public void Button3Clicked()
    {
        Debug.Log("3");
    }

    public void LoadList()
    {
        // read in the list file
        string[] items = File.ReadAllLines("Assets\\list.txt");

        // list of items
        Items = new Items[items.Length / 2];

        // convert to item struct array
        for (int i = 0; i < items.Length; i += 2)
        {
            // product name
            Items[i / 2].itemName = items[i];
            // respective code
            Items[i / 2].itemCode = float.Parse(items[i + 1]);
        }

        Color col = new Color();
        col.r = Random.Range(0.0f, 1.0f);
        col.g = Random.Range(0.0f, 1.0f);
        col.b = Random.Range(0.0f, 1.0f);

        go.GetComponent<MeshRenderer>().materials[0].color = col;

    }
}

