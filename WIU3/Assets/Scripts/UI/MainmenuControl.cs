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
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < PannelsTrueorFalse.Length + 1; i++)
        {
            PannelsTrueorFalse[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ClickCheck();
        Escapeispressed = Input.GetKeyUp(KeyCode.E);
        Debug.Log("Escapeispressed : " + Escapeispressed);
        if (Mainmenu.activeSelf)
        {
            if (Escapeispressed)
            {
                SetactiveObj(Mainmenu, false);
                for (int i = 0; i < PannelsTrueorFalse.Length + 1; i++)
                {
                    PannelsTrueorFalse[i] = false;
                    Debug.Log(i + " : " + PannelsTrueorFalse[i]);
                }
            }
        }
        else
        {
            if (Escapeispressed)
            {
                SetactiveObj(Mainmenu, true);
                PannelsTrueorFalse[0] = true;
                for (int i = 1; i < PannelsTrueorFalse.Length + 1; i++)
                {
                    PannelsTrueorFalse[i] = false;
                }
            }
        }
        int index = 0;
        foreach (var menu in MenuPannels)
        {
            SetactiveObj(menu, PannelsTrueorFalse[index]);
            index++;
        }
    }
    private void ClickCheck()
    {
        PannelsTrueorFalse[BoolIndex] = ClickedToF;
    }
    public void Clickedtrue( bool trueorfalse)
    {
        ClickedToF = trueorfalse;
    }
    public void Clickedindex(int Index)
    {
        BoolIndex = Index;
    }
    private void SetactiveObj(GameObject gameObject, bool trueorfalse)
    {
        Debug.Log(gameObject + " : " + trueorfalse);
        gameObject.SetActive(trueorfalse);
    }
}
