using eco;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepAgent : AnimalAgent {

    public int AgentID { get; set; }
    public SheepBehavior CurrentSheep { get; set; }
    public AgentController Controller { get; set; }

    public override void CollectObservations()
    {
        AddVectorObs(Dead ? 0f : 1f);

        List<float> obs;
        if (!Dead && CurrentSheep != null)
            obs = CurrentSheep.GetObservations();
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
                    CurrentSheep.MoveLeft();
                    break;
                case 1:
                    CurrentSheep.MoveRight();
                    break;
                case 2:
                    CurrentSheep.MoveUp();
                    break;
                case 3:
                    CurrentSheep.MoveDown();
                    break;
                case 4:
                    CurrentSheep.Eat();
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
        if (CurrentSheep != null)
            CurrentSheep.Die();
        else
            this.Dead = true;
    }



}
