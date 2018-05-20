using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace eco
{

    public class Lesson
    {

        public int ID { get; set; }
        public int Cols { get; set; }
        public int Rows { get; set; }
        public int SheepCount { get; set; }
        public int DummyCount { get; set; }
        public bool RandomSheep { get; set; }
        public int WolfCount { get; set; }
        public bool RandomWolf { get; set; }
        public List<GridVals> Map { get; set; }
        public bool PlantRespawn { get; set; }


        public int WallCount
        {
            get
            {
                return _wallCount;
            }
        }
        private int _wallCount;

        public int MaxPlants
        {
            get
            {
                return _maxPlants;
            }
        }
        private int _maxPlants;

        public Lesson()
        {
            setDefaultLesson();
        }

        public bool LoadMap(string sMap)
        {
            var splitStr = sMap.Split(';');
            try
            {
                int i = 0;
                ID = int.Parse(splitStr[i++].Split(':')[1]);
                //RandomSheep = splitStr[i++].Split(':')[1] == "1";
                SheepCount = int.Parse(splitStr[i++].Split(':')[1]);
                //RandomWolf = splitStr[i++].Split(':')[1] == "1";
                WolfCount = int.Parse(splitStr[i++].Split(':')[1]);
                _maxPlants = int.Parse(splitStr[i++].Split(':')[1]);
                DummyCount = int.Parse(splitStr[i++].Split(':')[1]);
                PlantRespawn = splitStr[i++].Split(':')[1] == "1";
                Cols = int.Parse(splitStr[i++].Split(':')[1]);
                Rows = int.Parse(splitStr[i++].Split(':')[1]);
                
                Debug.Log("Lesson ID: " + ID.ToString());
                Map = new List<GridVals>(); 
                fillEmptyList();
                char[] map = splitStr[i].Replace(Environment.NewLine, string.Empty).ToCharArray();
                for (int j = 0; j < Cols * Rows; j++)
                {
                    if (map[j] == '#')
                    {
                        Map[j] = GridVals.Wall;
                        _wallCount++;
                    }    
                    else if (map[j] == '0' )
                        Map[j] = GridVals.NoSpawn;
                    else if (map[j] == 's')
                        Map[j] = GridVals.SheepSpawn;
                    else if (map[j] == 'w')
                        Map[j] = GridVals.WolfSpawn;
                    else if (map[j] == 'd')
                        Map[j] = GridVals.DummySpawn;
                    else if (map[j] == 'p')
                        Map[j] = GridVals.PlantSpawn;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void fillEmptyList()
        {
            for (int i = 0; i < Cols * Rows; i++)
            {
                Map.Add(GridVals.Empty);
            }
        }

        private void setDefaultLesson()
        {
            Map = new List<GridVals>();
            _wallCount = 0;
            Cols = 4;
            Rows = 3;
            RandomSheep = true;
            RandomWolf = true;
            ID = 0;
            SheepCount = 0;
            WolfCount = 0;
            _maxPlants = 0;
            PlantRespawn = false;
            fillEmptyList();

        }
    }
}