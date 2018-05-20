using eco;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAgent : AnimalAgent {

    public int AgentID { get; set; }
    public WolfBehavior CurrentWolf { get; set; }
    public AgentController Controller { get; set; }

    public override void CollectObservations()
    {
        AddVectorObs(Dead ? 0f : 1f);

        List<float> obs;
        if (!Dead && CurrentWolf != null)
            obs = CurrentWolf.GetObservations();
        else
        {
            obs = new List<float>();
            for (int i = 0; i < brain.brainParameters.vectorObservationSize-1; i++)
            {
                obs.Add(0);
            }
        }
            
        foreach (var e in obs)
            AddVectorObs(e);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
	{
        int action = (int)vectorAction[0];

        if (!Dead)
        {
            switch(action)
            {
                case 0:
                    CurrentWolf.MoveLeft();
                    break;
                case 1:
                    CurrentWolf.MoveRight();
                    break;
                case 2:
                    CurrentWolf.MoveUp();
                    break;
                case 3:
                    CurrentWolf.MoveDown();
                    break;
                case 4:
                    CurrentWolf.Eat();
                    break;
            }
        }
            
    }

    public override void InitializeAgent()
    {
        this.Die();
    }

    public override void AgentReset()
    {

    }

    public override void AgentOnDone()
    {

    }

    public void Die()
    {
        if (CurrentWolf != null)
            CurrentWolf.Die();
        else
            this.Dead = true;
    }



}
