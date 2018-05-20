using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace eco
{
    public class Grid : MonoBehaviour
    {

        public int Columns, Rows;
        public float Distance, Height;
        public List<GridVals> GridList;
        private List<GridVals> OriginalList;
        public TextAsset Curriculum;
        public List<Lesson> Lessons;
        public Lesson CurrentLesson;

        // Use this for initialization

        private void Awake()
        {
            Lessons = new List<Lesson>();
            GridList = new List<GridVals>();
            Debug.Log("Loading curriculum...");
            LoadCurriculum();
            Debug.Log("Curriculum loaded!");
            ChangeLesson(1);
        }

        void Start()
        {


        }

        public bool ChangeLesson(int lessonNum = 1)
        {
            Debug.Log("Changing the lesson to " + lessonNum.ToString());
            if (lessonNum < 1) lessonNum = 1;
            if (CurrentLesson == null || lessonNum != CurrentLesson.ID)
            {
                this.CurrentLesson = this.Lessons.First(a => a.ID == lessonNum);
                this.Columns = this.CurrentLesson.Cols;
                this.Rows = this.CurrentLesson.Rows;
                this.GridList = new List<GridVals>(CurrentLesson.Map);
                this.OriginalList = new List<GridVals>(CurrentLesson.Map);
                return true;
            }
            else
                return false;

        }


        private void LoadCurriculum()
        {
            var stringList = Curriculum.text.Split("--".ToCharArray());
            foreach (var s in stringList)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    var l = new Lesson();
                    l.LoadMap(s);
                    Lessons.Add(l);
                }
            }
        }


        //Spawns fences
        public void SpawnPerimeter()
        {
            //Upper fences
            for (int x = 1; x < this.Columns - 1; x++)
                GridList[x] = GridVals.Wall;

            //Lower fences
            for (int x = Columns * Rows - Columns + 1; x < Columns * Rows - 1; x++)
                GridList[x] = GridVals.Wall;

            //Side fences
            for (int y = 0; y < this.Rows * Columns; y += Columns)
            {
                GridList[y] = GridVals.Wall;
                GridList[y + Columns - 1] = GridVals.Wall;
            }
        }

        /// <summary>
        /// Assigns an ID to an object in the grid.
        /// Adds the object at the appropriate place in the grid.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int AssignID(GridVals obj)
        {
            List<int> empty = new List<int>();

            var sheep = GridList.Where(a => a == GridVals.SheepSpawn);
            var wolf = GridList.Where(a => a == GridVals.WolfSpawn);
            var dummy = GridList.Where(a => a == GridVals.DummySpawn);
            var plant = GridList.Where(a => a == GridVals.PlantSpawn);
            GridVals val = GridVals.Empty;

            if (obj == GridVals.Sheep && sheep.Count() != 0)
                val = GridVals.SheepSpawn;
            else if (obj == GridVals.Plant && plant.Count() != 0)
                val = GridVals.PlantSpawn;
            else if (obj == GridVals.Wolf && wolf.Count() != 0)
                val = GridVals.WolfSpawn;
            else if (obj == GridVals.Dummy )
                if (dummy.Count() != 0)
                    val = GridVals.DummySpawn;
                else
                    val = GridVals.WolfSpawn;
            for (int i = 0; i < GridList.Count; i++)
                    if (GridList[i] == val)
                        empty.Add(i);
            
            int id;
            id = (int)Mathf.Round(UnityEngine.Random.Range(0f, (float)empty.Count - 1));
            GridList[empty[id]] = obj;
            return empty[id];
        }

        /// <summary>
        /// Gets the position of the ID in the environment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Vector3 Position(int id)
        {
            int row = id / Columns - Columns / 2;
            int col = id % Columns - Rows / 2;

            return new Vector3(col * Distance, Height, row * Distance);
        }

        public void FillAll(GridVals obj)
        {
            for (int i = 0; i < GridList.Count; i++)
                GridList[i] = obj;

        }

        public void GridReset()
        {
            GridList = new List<GridVals>(OriginalList);

        }
    }
}
