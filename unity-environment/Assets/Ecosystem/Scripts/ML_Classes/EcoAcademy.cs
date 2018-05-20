using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcoAcademy : Academy {

    public int CurrentLevel { get; set; }

    public override void AcademyReset()
    {
        this.CurrentLevel = (int)resetParameters["CurrentLevel"];
        Debug.Log("Academy Reset!");
    }

    public override void AcademyStep()
    {


    }

}
