using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainmenuControl : MonoBehaviour
{
    [SerializeField] private GameObject Mainmenu;
    [SerializeField] private GameObject[] MenuPannels;
    private bool[] PannelsTrueorFalse;
    private bool Escapeispressed;
    private int BoolIndex = 0;
    private bool ClickedToF = false;
    private bool isMenuOpen = false; // New state variable

    // Start is called before the first frame update
    void Start()
    {
        PannelsTrueorFalse = new bool[MenuPannels.Length]; // Initialize array
        //for (int i = 0; i < PannelsTrueorFalse.Length; i++)
        //{
        //    PannelsTrueorFalse[i] = false;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        ClickCheck();
        Escapeispressed = Input.GetKeyUp(KeyCode.E);
        Debug.Log("Escapeispressed : " + Escapeispressed);

        // Check if the menu is open
        if (isMenuOpen)
        {
            // If the menu is currently open and Escape is pressed, close it
            if (Escapeispressed)
            {
                CloseMenu();
            }
        }
        else
        {
            // If the menu is not open and Escape is pressed, open it
            if (Escapeispressed)
            {
                OpenMenu();
            }
        }

        // Update active states of menu panels
        for (int index = 0; index < MenuPannels.Length; index++)
        {
            SetactiveObj(MenuPannels[index], PannelsTrueorFalse[index]);
        }
    }

    private void OpenMenu()
    {
        SetactiveObj(Mainmenu, true);
        isMenuOpen = true; // Set menu open state
        PannelsTrueorFalse[0] = true; // Activate the first panel
        for (int i = 1; i < PannelsTrueorFalse.Length; i++)
        {
            PannelsTrueorFalse[i] = false; // Deactivate other panels
        }
    }

    private void CloseMenu()
    {
        SetactiveObj(Mainmenu, false);
        isMenuOpen = false; // Set menu closed state
        for (int i = 0; i < PannelsTrueorFalse.Length; i++)
        {
            PannelsTrueorFalse[i] = false; // Deactivate all panels
        }
    }

    private void ClickCheck()
    {
        PannelsTrueorFalse[BoolIndex] = ClickedToF;
    }

    public void Clickedtrue(bool trueorfalse)
    {
        ClickedToF = trueorfalse;
    }

    public void Clickedindex(int Index)
    {
        BoolIndex = Index;
    }

    private void SetactiveObj(GameObject gameObject, bool trueorfalse)
    {
        Debug.Log(gameObject.name + " : " + trueorfalse);
        gameObject.SetActive(trueorfalse);
    }

    //[SerializeField] private GameObject Mainmenu;
    //[SerializeField] private GameObject[] MenuPannels;
    //private bool[] PannelsTrueorFalse;
    //private bool Escapeispressed;
    //private int BoolIndex = 0;
    //private bool ClickedToF = false;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    PannelsTrueorFalse = new bool[MenuPannels.Length]; // Initialize array
    //    for (int i = 0; i < PannelsTrueorFalse.Length; i++)
    //    {
    //        PannelsTrueorFalse[i] = false;
    //    }
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    ClickCheck();
    //    Escapeispressed = Input.GetKeyUp(KeyCode.E);
    //    Debug.Log("Escapeispressed : " + Escapeispressed);

    //    if (Mainmenu.activeSelf)
    //    {
    //        if (Escapeispressed)
    //        {
    //            SetactiveObj(Mainmenu, false); // Toggle to inactive
    //            for (int i = 0; i < PannelsTrueorFalse.Length; i++)
    //            {
    //                PannelsTrueorFalse[i] = false;
    //                Debug.Log(i + " : " + PannelsTrueorFalse[i]);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (Escapeispressed)
    //        {
    //            SetactiveObj(Mainmenu, true); // Toggle to active
    //            PannelsTrueorFalse[0] = true;
    //            for (int i = 1; i < PannelsTrueorFalse.Length; i++)
    //            {
    //                PannelsTrueorFalse[i] = false;
    //            }
    //        }
    //    }

    //    // Update active states of menu panels
    //    for (int index = 0; index < MenuPannels.Length; index++)
    //    {
    //        SetactiveObj(MenuPannels[index], PannelsTrueorFalse[index]);
    //    }
    //}

    //private void ClickCheck()
    //{
    //    PannelsTrueorFalse[BoolIndex] = ClickedToF;
    //}

    //public void Clickedtrue(bool trueorfalse)
    //{
    //    ClickedToF = trueorfalse;
    //}

    //public void Clickedindex(int Index)
    //{
    //    BoolIndex = Index;
    //}

    //private void SetactiveObj(GameObject gameObject, bool trueorfalse)
    //{
    //    Debug.Log(gameObject.name + " : " + trueorfalse);
    //    gameObject.SetActive(trueorfalse);
    //}

    //[SerializeField] private GameObject Mainmenu;
    //private bool[] PannelsTrueorFalse = new bool[4];
    //[SerializeField] private GameObject[] MenuPannels;
    //private bool Escapeispressed;
    //private int BoolIndex = 0;
    //private bool ClickedToF = false;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    for (int i = 0; i < PannelsTrueorFalse.Length + 1; i++)
    //    {
    //        PannelsTrueorFalse[i] = false;
    //    }
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    ClickCheck();
    //    Escapeispressed = Input.GetKeyUp(KeyCode.E);
    //    Debug.Log("Escapeispressed : " + Escapeispressed);
    //    if (Mainmenu.activeSelf)
    //    {
    //        if (Escapeispressed)
    //        {
    //            SetactiveObj(Mainmenu, true);
    //            for (int i = 0; i < PannelsTrueorFalse.Length; i++)
    //            {
    //                PannelsTrueorFalse[i] = false;
    //                Debug.Log(i + " : " + PannelsTrueorFalse[i]);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (Escapeispressed)
    //        {
    //            SetactiveObj(Mainmenu, true);
    //            PannelsTrueorFalse[0] = true;
    //            for (int i = 1; i < PannelsTrueorFalse.Length; i++)
    //            {
    //                PannelsTrueorFalse[i] = false;
    //            }
    //        }
    //    }
    //    int index = 0;
    //    foreach (var menu in MenuPannels)
    //    {
    //        SetactiveObj(menu, PannelsTrueorFalse[index]);
    //        index++;
    //    }
    //}
    //private void ClickCheck()
    //{
    //    PannelsTrueorFalse[BoolIndex] = ClickedToF;
    //}
    //public void Clickedtrue( bool trueorfalse)
    //{
    //    ClickedToF = trueorfalse;
    //}
    //public void Clickedindex(int Index)
    //{
    //    BoolIndex = Index;
    //}
    //private void SetactiveObj(GameObject gameObject, bool trueorfalse)
    //{
    //    Debug.Log(gameObject + " : " + trueorfalse);
    //    gameObject.SetActive(trueorfalse);
    //}
}
