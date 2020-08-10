using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Player;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class EndingScript : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isInitCanvasDone;
    private Canvas endingCanvas;
    private Text endingText;
    private Image backgroundColor;

    private void Start()
    {
        isInitCanvasDone = false;
    }

    private void FixedUpdate()
    {
        if (!isInitCanvasDone)
        {
            try
            {
                var x = Values.GetCurrentBoss().transform.position;
            }
            catch (Exception)
            {
                RunTrigger();
                isInitCanvasDone = true;
            }
        }
        else
        {
            backgroundColor.color = new Color(backgroundColor.color.r, backgroundColor.color.g, backgroundColor.color.b,
                0.01f + backgroundColor.color.a);
            endingText.color = new Color(endingText.color.r, endingText.color.g, endingText.color.b,
                0.01f + endingText.color.a);

            SoundManager.ModifySoundtrackValue(-0.0003f);

            if (SoundManager.GetSoundtrackVolume() <= 0)
            {
                StartCoroutine(WaitAndCloseGame());
                enabled = false;
            }
        }
    }

    private void RunTrigger()
    {
        print("gioco finito");


        Values.SetIsFrozen(true);
        Values.SetCanPause(false);
        Values.SetCanSave(false);

        var canvas = Resources.Load("Prefabs/Levels/Level3/Objects/EndingCanvas");
        var obj = Instantiate(canvas) as GameObject;
        obj.SetActive(true);

        endingCanvas = obj.GetComponent<Canvas>();
        backgroundColor = obj.GetComponentInChildren<Image>();
        endingText = obj.GetComponentInChildren<Text>();
    }

    private IEnumerator WaitAndCloseGame()
    {
        yield return new WaitForSeconds(15);

        Application.Quit();
    }
}