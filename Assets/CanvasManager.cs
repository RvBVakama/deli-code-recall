using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public Canvas m_canMainMenu = null;
    public Canvas m_canGame = null;

    
    public void StartButton()
    {
        // switch to the game
        m_canGame.gameObject.SetActive(true);
        m_canMainMenu.gameObject.SetActive(false);
        GetComponent<gameLogic>().enabled = true;            
    }

    public void ExitButton()
    {
        // switch to the main menu
        m_canGame.gameObject.SetActive(false);
        m_canMainMenu.gameObject.SetActive(true);
        GetComponent<gameLogic>().enabled = false;
    }
}

