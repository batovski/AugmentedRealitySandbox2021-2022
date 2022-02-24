using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager Instance;

    [SerializeField]
    private GameObject UI_GO;

    // Create stativ reference
    void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
    }

   public void HideMainMenuUI()
    {
        if (UI_GO)
            UI_GO.SetActive(false);
    }
    public void OpenMainMenuUI()
    {
        if (UI_GO)
            UI_GO.SetActive(true);
    }
}
