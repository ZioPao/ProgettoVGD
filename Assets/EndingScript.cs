using System;
using System.Collections;
using System.Collections.Generic;
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
            // backgroundColor.color = Color.Lerp(Color.clear, Color.red, 15);
            // endingText.color = Color.Lerp(Color.clear, Color.white, 15);
            backgroundColor.color = new Color(backgroundColor.color.r, backgroundColor.color.g, backgroundColor.color.b, 0.01f + backgroundColor.color.a);
            endingText.color = new Color(endingText.color.r, endingText.color.g, endingText.color.b, 0.01f + endingText.color.a);


            if (backgroundColor.color.a >= 255 && endingText.color.a >= 255)
                enabled = false;
        }
    }

    private void RunTrigger()
    {
        print("gioco finito");
        
        
        //Apre un canvas?
        
        //Fa partire porno con nani?
        
        // Foto di palle
        
        //fade in
        
        //Spawna un canvas
        var canvas = Resources.Load("Prefabs/Levels/Level3/Objects/EndingCanvas");
        var obj = Instantiate(canvas) as GameObject;
        obj.SetActive(true);

        endingCanvas = obj.GetComponent<Canvas>();
        backgroundColor = obj.GetComponentInChildren<Image>();
        endingText = obj.GetComponentInChildren<Text>();
        // var obj = new GameObject();
        // endingCanvas = obj.AddComponent<Canvas>();
        // endingCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        // obj.AddComponent<CanvasScaler>();
        // obj.AddComponent<GraphicRaycaster>();
        //
        // var backgroundObject = new GameObject("Panel");
        // backgroundObject.transform.parent = obj.transform;
        // backgroundObject.AddComponent<CanvasRenderer>();
        // backgroundColor = backgroundObject.AddComponent<Image>();
        //
        // // Text
        // var textObj = new GameObject();
        // textObj.transform.parent = obj.transform;
        // textObj.name = "TextEnding";
        // endingText = textObj.AddComponent<Text>();
        //
        // endingText.font = Resources.Load<Font>("Common/Fonts/DisposableDroidBB");
        // endingText.resizeTextForBestFit = true;
        // endingText.text = "Finito";
        // // Text position
        // // var rectTransform = textObj.GetComponent<RectTransform>();
        // // rectTransform.localPosition = new Vector3(0, 0, 0);
        // // rectTransform.sizeDelta = new Vector2(400, 200);
        //
        // endingCanvas.overrideSorting = true;
        //
        // //enabled = false;
    }

    
}
