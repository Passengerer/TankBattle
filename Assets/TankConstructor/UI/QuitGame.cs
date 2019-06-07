using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        onHide();

        GameObject YesButton = GameObject.Find("Panel/YesButton");
        GameObject NoButton = GameObject.Find("Panel/NoButton");
        Button quit = YesButton.GetComponent<Button>();
        Button cancel = NoButton.GetComponent<Button>();

        quit.onClick.AddListener(onYesClick);
        cancel.onClick.AddListener(onNoClick);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Quit game?");
            onShow();
        }
    }

    void onHide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    void onShow()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    void onYesClick()
    {
        Debug.Log("onYesClick");
        Application.Quit();
    }

    void onNoClick()
    {
        Debug.Log("onNoClick");
        onHide();
    }
}
