using UnityEngine;
using System.Collections;

public class OpenClose : MonoBehaviour {

    //GLOBAL VARIABLES//
    

    //panel variable for the class OpenClose
    private GameObject panel;
    public GameObject obj;

            /*public GameObject panel;
	        public GameObject panel1;
	        public GameObject panel2;
	        public GameObject panel3;
	        public GameObject panel4;
            */

    //FUNCTIONS//

    //toggles off and on a button depending on its state
    public void toggle(GameObject pan)
    {
        panel = pan;

        if (panel.activeSelf == false)
        {
            open(panel);
        }

        else
        {
            close(panel);
        }
    }

    public void close(GameObject panelNum)
    {
        panel = panelNum;
        panel.SetActive(false);
	}
	
	public void open(GameObject panelNum)
    {

        panel = panelNum;
        panel.SetActive(true);
		
	}
	
	/*public void close1()
        {
            panel1.SetActive(false);
        }
	
	public void open1(){
		
		panel1.SetActive(true);
		
	}
	
	public void close2(){
		
		panel2.SetActive(false);
		
	}
	
	public void open2(){
		
		panel2.SetActive(true);
		
	}
	
	public void close3(){
		
		panel3.SetActive(false);
		
	}
	
	public void open3(){
		
		panel3.SetActive(true);
		
	}
	
	public void close4(){
		
		panel4.SetActive(false);
		
	}
	
	public void open4(){
		
		panel4.SetActive(true);


		
	}
    */
}
