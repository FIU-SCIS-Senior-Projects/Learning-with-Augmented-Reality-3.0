/*
Copyright(c) "<2017>, by <Aberrate LLC>
		Contributors: <Albert Elias>
		Affiliation: <Florida International University>
		URL: <www.albertelias.com> <www.aberrate.net>
		Citation: <OpenClose. Albert Elias (Version 1.0) [Computer software: Unity Asset]. (2016). Retrieved from https://github.com/aje0827/SKOPE-VR>"

All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the
following conditions are met:

	Redistributions of source code must retain the above copyright notice, this list of conditions and the
    following disclaimer.
    Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the
    following disclaimer in the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUISNESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY
WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE
USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/


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
