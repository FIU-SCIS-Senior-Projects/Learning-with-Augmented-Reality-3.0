/*
Copyright(c) "<2017>, by <Florida International University>
		Contributors: <Albert Elias, Jefrey Perez, Luis Perera>
		Affiliation: <P.I. Shahin Vassigh>
		URL: <http://skope.fiu.edu/> <http://www.albertelias.com/>
		Citation: <DisplayLayer. Albert Elias, Jefrey Perez, Luis Perera. Florida International University (Version 1.0) [Computer software: Unity Asset]. (2017). Retrieved from https://github.com/aje0827/SKOPE-VR>"

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

public class DisplayLayer : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        //turns on gameObject this script is attached too
        gameObject.SetActive(true);
    }

    //FUNCTIONS//

    //toggles off and on a layer depending on its state
    public void toggle(GameObject model)
    {
        GameObject mod = model;

        if (mod.activeSelf == false)
        {
            open(mod);
        }

        else
        {
            close(mod);
        }
    }

    //turn on layer
    public void close(GameObject model)
    {
        GameObject mod = model;
        mod.SetActive(false);
    }

    //turn off layer
    public void open(GameObject model)
    {

        GameObject mod = model;
        mod.SetActive(true);

    }
}
