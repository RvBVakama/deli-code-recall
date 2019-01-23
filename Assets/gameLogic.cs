using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
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
    public SpriteRenderer bg = null;

    // the number of rounds in a session
    private int m_nRounds = 0;

    // the current round
    private int m_nRoundCount = 0;
    
    // list of items in list.txt
    List<Items> lstItems;
    List<float> lstItemCodes;

    private void OnEnable()
    {
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
    }

    int m_nGotCorrect = 0;

    public void Button1Clicked()
    {
    }

    public void Button2Clicked()
    {
    }

    public void Button3Clicked()
    {
    }

    public bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        bool isOk = true;
        // If there are errors in the certificate chain, look at each error to determine the cause.
        if (sslPolicyErrors != SslPolicyErrors.None)
        {
            for (int i = 0; i < chain.ChainStatus.Length; i++)
            {
                if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
                {
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2)certificate);
                    if (!chainIsValid)
                    {
                        isOk = false;
                    }
                }
            }
        }
        return isOk;
    }

    public void LoadList()
    {
        string[] items;

        // if connected to internet
        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            // download latest list
            WebClient client = new WebClient();
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            Stream data = client.OpenRead(@"https://pastebin.com/raw/yAcmeGhv");
            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd();
            string[] lines = s.Split(new[] { System.Environment.NewLine }, options: System.StringSplitOptions.None);

            data.Close();
            reader.Close();

            items = lines;
            File.WriteAllLines(Application.persistentDataPath + "/list.txt", items);
        }

        // read in the list file
        items = File.ReadAllLines(Application.persistentDataPath + "/list.txt");

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
        // show the results
        GetComponent<CanvasManager>().m_canResults.gameObject.SetActive(true);
        GameObject.Find("GotCorrect").GetComponentInChildren<Text>().text = "Got correct: " + m_nGotCorrect.ToString() + "/" + m_nRounds;

        // close the games canvas
        GetComponent<CanvasManager>().m_canGame.gameObject.SetActive(false);

        ResetGame();
    }

    // reset all game values
    public void ResetGame()
    {
        m_nRounds = 0;
        m_nRoundCount = 0;
        m_nGotCorrect = 0;
    }

    private void Normalbgcolor()
    {
        bg.color = Color.white;
    }
}

