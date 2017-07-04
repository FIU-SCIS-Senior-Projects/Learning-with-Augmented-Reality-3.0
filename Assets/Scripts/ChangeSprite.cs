using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ChangeSprite : MonoBehaviour
{
    //public Sprite diagram;
    public GameObject panel;
    public Image fromPanel;
    private Sprite newSprite;

    public void UpdateSprite(Sprite current)
    {
        newSprite = current;
        Sprite old = panel.GetComponent<Image>().sprite;
        fromPanel.GetComponent<Image>().sprite = newSprite;

        //fromPanel.GetComponent<Image>().sprite = current;

        //Sprite old = current.GetComponent<Image>().sprite;
        //current.GetComponent<Image>().sprite = diagram;
    }
}



//panel = GetComponent<GameObject>().gameObject;








//Archives//

//public Sprite[] diagramList;
//public GameObject panel;
////public Image TargetImage;

//panel.GetComponent<Image>().sprite = tex0;
//panel.GetComponent<Image>().sprite = tex0;
//panel = pan;
//int diagramIndex = index;
//Sprite I = panel.GetComponent<Image>().sprite;
//Image.defaultGraphicMaterial.SetTexture("AHC4 - South Section(NaturalLighting+Heat)Sprite", tex0);
//panel.GetComponent<Image>().sprite = Resources.Load("AHC4 - South Section(NaturalLighting+Heat)Sprite") as Sprite;
//material0 = GetComponent<GUITexture>().guiTexture;