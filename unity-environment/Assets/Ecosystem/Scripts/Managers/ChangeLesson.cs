using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLesson : MonoBehaviour {

    //public GameObject academy;
    private EcoAcademy acadScr;
    public GameObject AgentController;
    private AgentController agentScr;
    public bool Enabled;
    // Use this for initialization
	void Start () {
        
		acadScr = (EcoAcademy)this.gameObject.GetComponent(typeof(EcoAcademy));
        agentScr = (AgentController)AgentController.GetComponent(typeof(AgentController));
    }
	
	// Update is called once per frame
	void Update () {
        if(Enabled)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                acadScr.resetParameters["CurrentLevel"] = 1f;
                acadScr.CurrentLevel = 1;
                acadScr.AcademyReset();
                agentScr.Reset();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                acadScr.resetParameters["CurrentLevel"] = 2f;
                acadScr.CurrentLevel = 2;
                acadScr.AcademyReset();
                agentScr.Reset();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                acadScr.resetParameters["CurrentLevel"] = 3f;
                acadScr.CurrentLevel = 3;
                acadScr.AcademyReset();
                agentScr.Reset();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                acadScr.resetParameters["CurrentLevel"] = 4f;
                acadScr.CurrentLevel = 4;
                acadScr.AcademyReset();
                agentScr.Reset();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                acadScr.resetParameters["CurrentLevel"] = 5f;
                acadScr.CurrentLevel = 5;
                acadScr.AcademyReset();
                agentScr.Reset();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                acadScr.resetParameters["CurrentLevel"] = 6f;
                acadScr.CurrentLevel = 6;
                acadScr.AcademyReset();
                agentScr.Reset();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                acadScr.resetParameters["CurrentLevel"] = 7f;
                acadScr.CurrentLevel = 7;
                acadScr.AcademyReset();
                agentScr.Reset();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                acadScr.resetParameters["CurrentLevel"] = 8f;
                acadScr.CurrentLevel = 8;
                acadScr.AcademyReset();
                agentScr.Reset();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                acadScr.resetParameters["CurrentLevel"] = 9f;
                acadScr.CurrentLevel = 9;
                acadScr.AcademyReset();
                agentScr.Reset();
            }
        }
    }
}
