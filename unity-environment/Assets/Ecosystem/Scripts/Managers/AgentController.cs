using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using eco;
using System;

public class AgentController : MonoBehaviour
{

    public GameObject Game;
    private Spawner spawner;
    private eco.Grid grid;

    public bool PlantRespawn { get; set; }
    public List<SheepAgent> sheep { get; set; }
    public List <WolfAgent> wolf { get; set; }


    private int prevLesson;
    public GameObject Academy;
    private EcoAcademy academy;
    // Use this for initialization
    void Start()
    {
        try
        {
            sheep = new List<SheepAgent>();
            for (int i = 0; i < this.transform.childCount; i++)
            {
                var sh = (SheepAgent)this.transform.GetChild(i).GetComponent(typeof(SheepAgent));
                if (sh != null)
                    sheep.Add(sh);
            }

            wolf = new List<WolfAgent>();
            for (int i = 0; i < this.transform.childCount; i++)
            {
                var w = (WolfAgent)this.transform.GetChild(i).GetComponent(typeof(WolfAgent));
                if (w != null)
                    wolf.Add(w);
            }

            academy = (EcoAcademy)Academy.transform.GetComponent(typeof(EcoAcademy));
            grid = (eco.Grid)Game.transform.GetComponent(typeof(eco.Grid));
            spawner = (Spawner)Game.transform.GetComponent(typeof(Spawner));

            prevLesson = 0;
        }
        catch (Exception ex)
        {

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        try
        {
            //Check if a lesson change is necessary
            if (prevLesson != academy.CurrentLevel)
            {
                ChangeLevel(academy.CurrentLevel);
                prevLesson = academy.CurrentLevel;
            }

            //Check if a game reset is necessary
            bool dummyCase = (grid.CurrentLesson.DummyCount > 0 && spawner.AllDummiesDead() || AllWolvesDead() ) && (AllSheepDead()  || grid.CurrentLesson.MaxPlants > 0 && spawner.AllPlantsDead());

            if ((grid.CurrentLesson.SheepCount > 0 && AllSheepDead() && grid.CurrentLesson.DummyCount == 0) 
                || (grid.CurrentLesson.SheepCount == 0 && AllWolvesDead() && grid.CurrentLesson.DummyCount == 0) 
                || (!grid.CurrentLesson.PlantRespawn && grid.CurrentLesson.MaxPlants > 0 && spawner.AllPlantsDead() && grid.CurrentLesson.DummyCount == 0)
                || dummyCase)
            {
                this.Reset();
            }
        }
        catch (Exception ex)
        {

        }
        
    }

    public void Reset()
    {
        grid.GridReset();
        spawner.ResetLevel();
    }
    public bool AllSheepDead()
    {
        var result = sheep.FirstOrDefault(a => !a.Dead);

        return result == null;
            
    }

    public bool AllWolvesDead()
    {
        var result = wolf.FirstOrDefault(a => !a.Dead);

        return result == null;

    }

    public void ChangeLevel(int levelID = 1)
    {
        grid.ChangeLesson(levelID);
        sheep.ForEach(a =>
        {
            if (a.CurrentSheep != null)
                a.CurrentSheep.Die();
            else
                a.Die();
        });
        spawner.ChangeLesson();
    }
}
