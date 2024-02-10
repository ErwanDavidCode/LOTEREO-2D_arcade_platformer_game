using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_BackgroundScaler : MonoBehaviour
{
    private Image backgroundImage;
    private RectTransform canvasrt;
    float ratio;

    // Start is called before the first frame update
    void Start()
    {
        backgroundImage=GetComponent<Image>();
        canvasrt=GetComponentInParent<Canvas>().GetComponent<RectTransform>();;

        ratio=backgroundImage.sprite.rect.width/backgroundImage.sprite.rect.height;
    }

    // Update is called once per frame
    void Update()
    {
        if (!backgroundImage.rectTransform) 
        {
            return;
        }
        //Met l'image à l'échelle de l'écran du joueur,
        //en conservant ses proportions
        float newWidth = canvasrt.rect.width;
        float newHeight = newWidth / ratio;

        backgroundImage.rectTransform.sizeDelta = new Vector2(newWidth, newHeight);

        // if (canvasrt.height*ratio>=Screen.width)
        // {
        //     backgroundImage.rectTransform.sizeDelta= new Vector2(
        //     Screen.height*ratio,Screen.height);
        // }
        // else
        // {
        //     backgroundImage.rectTransform.sizeDelta=new Vector2(
        //     Screen.width,Screen.width/ratio);
        // }
    }
}
