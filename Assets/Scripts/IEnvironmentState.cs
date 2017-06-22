/*
Copyright(c) "<2017>, by <Aberrate LLC>
		Contributors: <Albert Elias>
		Affiliation: <Florida International University>
		URL: <www.albertelias.com> <www.aberrate.net>
		Citation: <IEnvironmentState. Albert Elias (Version 1.0) [Computer software: Unity Asset]. (2017). Retrieved from https://github.com/aje0827/SKOPE-VR>"

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

//An interface that controls the state of the objects in the Environment
public interface IEnvironmentState
{
    void UpdateState();

    void OnTriggerClicked();
    //Passes System or Floating Button to acivate state

    void ToMainState();
    //Main Panel Activated
    //Path Actiaved
    //Floating Diagram Interface Activated

    void ToDiagramState();
    //Diagram Button that was clicked calls asscoiated event
    //Diagrams Panel Activated
    //Camera Switch to Aerial Camera

    void ToMajorComponentState();
    //System that was triggered becomes transparent
    //Pulsing of this System's MajorComponents
    //System's Major Annotation
    //System's Floating Major Interface Activated

    void ToSubComponentState();
    //Isolation of the System from Environment
    //Disables System's Major Colliders
    //Activaes the MajorAnnotation UI Panel
    //System's Sub Annotation
    //System's Floating Sub Interface Activated

    void ToBuildState();

    void ToMechanicalState();

    void ToMechanicalRoomState();

    void ToIconState();
}
