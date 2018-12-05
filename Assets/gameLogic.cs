using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    List<Items> lstItems;
    List<float> lstItemCodes;
    
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
    int m_nCorrectProductIndex = 0;
    int m_nCorrectCode = 0;

    public void NewSet()
    {
        if (lstItems.Count <= 0)
        {
            EndGame();
            return;
        }


        // the product to guess
        m_nCorrectProductIndex = Random.Range(0, lstItems.Count);


        // set the name of the product
        GameObject.Find("ProductName").GetComponent<Text>().text = lstItems[m_nCorrectProductIndex].itemName;

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
        btnarr[num].transform.GetComponentInChildren<Text>().text = lstItems[m_nCorrectProductIndex].itemCode.ToString();
        // convert string to float and remember the correct code
        m_nCorrectCode = int.Parse(lstItems[m_nCorrectProductIndex].itemCode.ToString());

        // the number to pick out
        n = Mathf.RoundToInt(Random.Range(0.0f, numlist.Count - 1));
        num = numlist[n];
        // remove this num from the list
        numlist.RemoveAt(n);

        btnarr[num].transform.GetComponentInChildren<Text>().text = lstItemCodes[Random.Range(0, lstItems.Count)].ToString();

        // the number to pick out
        num = numlist[0];
        // remove the last value from the list
        numlist.RemoveAt(0);

        btnarr[num].transform.GetComponentInChildren<Text>().text = lstItems[Random.Range(0, lstItems.Count)].itemCode.ToString();

        // make sure not to use this product again
        lstItems.RemoveAt(m_nCorrectProductIndex);
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
        lstItems = new List<Items>();

        // convert to item struct array
        for (int i = 0; i < items.Length; i += 2)
        {
            Items im = new Items();
            // product name
            im.itemName = items[i];
            // respective code
            im.itemCode = float.Parse(items[i + 1]);
            // add it
            lstItems.Add(im);
        }

        // list of item names
        lstItemCodes = new List<float>();

        for (int i = 0; i < lstItems.Count; i++)
        {
            lstItemCodes.Add(lstItems[i].itemCode);
        }

        // set the rounds length based on how many items are in the list
        m_nRounds = lstItems.Count;
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

