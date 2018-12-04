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
    // the bg image sprite
    public Sprite bg = null;

    // the number of rounds in a session
    private int m_nRounds = 0;

    // the current round
    private int m_nRoundCount = 0;

    // list of items in list.txt
    Items[] Items;

    private void OnEnable()
    {
        // listereners on each button
        if (!btnarr[0])
        {
            btnarr[0] = GameObject.Find("Game").transform.GetChild(1).GetComponent<Button>();
            btnarr[0].onClick.AddListener(Button1Clicked);
            btnarr[1] = GameObject.Find("Game").transform.GetChild(2).GetComponent<Button>();
            btnarr[1].onClick.AddListener(Button2Clicked);
            btnarr[2] = GameObject.Find("Game").transform.GetChild(3).GetComponent<Button>();
            btnarr[2].onClick.AddListener(Button3Clicked);
        }

        LoadList();

        NewSet();
    }

    // store all the buttons
    Button[] btnarr = new Button[3];
    int m_nCorrectProduct = 0;
    int m_nCorrectCode = 0;

    public void NewSet()
    {
        if (m_nRoundCount > m_nRounds)
        {
            EndGame();
            return;
        }

        // the product to guess
        m_nCorrectProduct = Random.Range(0, Items.Length);

        // set the name of the product
        GameObject.Find("ProductName").GetComponent<Text>().text = Items[m_nCorrectProduct].itemName;

        // for the random picker logic
        List<int> numlist = new List<int>();

        // add numbers to the list
        for (int i = 0; i < btnarr.Length; i++)
            numlist.Add(i);

        // the number to pick out
        int n = Mathf.RoundToInt(Random.Range(0.0f, numlist.Count - 1));
        int num = numlist[n];
        // remove this num from the list
        numlist.RemoveAt(n);

        // set the possible options from the list of codes
        btnarr[num].transform.GetComponentInChildren<Text>().text = Items[m_nCorrectProduct].itemCode.ToString();
        // convert string to float and remember the correct code
        m_nCorrectCode = int.Parse(Items[m_nCorrectProduct].itemCode.ToString());

        // the number to pick out
        n = Mathf.RoundToInt(Random.Range(0.0f, numlist.Count - 1));
        num = numlist[n];
        // remove this num from the list
        numlist.RemoveAt(n);

        bool cont = true;

        btnarr[num].transform.GetComponentInChildren<Text>().text = Items[Random.Range(0, Items.Length)].itemCode.ToString();

        while (cont)
        {
            if (btnarr[num].transform.GetComponentInChildren<Text>().text == m_nCorrectCode.ToString())
                btnarr[num].transform.GetComponentInChildren<Text>().text = Items[Random.Range(0, Items.Length)].itemCode.ToString();
            else
                cont = false;
        }

        // the number to pick out
        num = numlist[0];
        // remove the last value from the list
        numlist.RemoveAt(0);

        btnarr[num].transform.GetComponentInChildren<Text>().text = Items[Random.Range(0, Items.Length)].itemCode.ToString();

        cont = true;

        while (cont)
        {
            if (btnarr[num].transform.GetComponentInChildren<Text>().text == m_nCorrectCode.ToString())
                btnarr[num].transform.GetComponentInChildren<Text>().text = Items[Random.Range(0, Items.Length)].itemCode.ToString();
            else
                cont = false;
        }

    }

    int m_nGotCorrect = 0;

    public void Button1Clicked()
    {
        // if this button has the correct code log that it was right
        if (m_nCorrectCode == int.Parse(btnarr[0].transform.GetComponentInChildren<Text>().text))
        {
            ++m_nGotCorrect;
        }
        else
        {
            print("wrong");
        }
        NewSet();
        ++m_nRoundCount;
    }

    public void Button2Clicked()
    {
        // if this button has the correct code log that it was right
        if (m_nCorrectCode == int.Parse(btnarr[1].transform.GetComponentInChildren<Text>().text))
        {
            ++m_nGotCorrect;
        }
        else
        {
            print("wrong");
        }
        NewSet();
        ++m_nRoundCount;
    }

    public void Button3Clicked()
    {
        // if this button has the correct code log that it was right
        if (m_nCorrectCode == int.Parse(btnarr[2].transform.GetComponentInChildren<Text>().text))
        {
            ++m_nGotCorrect;
        }
        else
        {
            print("wrong");
        }
        NewSet();
        ++m_nRoundCount;
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

        // set the rounds length based on how many items are in the list
        m_nRounds = Items.Length;
    }

    private void EndGame()
    {
        GetComponent<CanvasManager>().m_canResults.gameObject.SetActive(true);
        GameObject.Find("GotCorrect").GetComponentInChildren<Text>().text = "Got correct: " + m_nGotCorrect.ToString() + "/" + m_nRounds;

        ResetGame();
    }

    // reset all game values
    private void ResetGame()
    {
        m_nRounds = 0;
        m_nRoundCount = 0;
        m_nGotCorrect = 0;
    }
}

