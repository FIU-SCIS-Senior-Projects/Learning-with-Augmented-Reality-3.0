using UnityEngine;
using System.Collections;

public class Change2Scene : MonoBehaviour {

	public void Change (string x){
		
		Application.LoadLevel(x);
	}
	
	}

