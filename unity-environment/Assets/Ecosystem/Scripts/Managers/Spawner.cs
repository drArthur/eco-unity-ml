using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace eco
{
    public class Spawner : MonoBehaviour
    {

        public int CurrentPlantCount
        {
            get
            {
                return plantList.Where(p => p.activeInHierarchy).Count();
            }
        }


        public int CurrentDummyCount
        {
            get
            {
                return dummyList.Where(p => p.activeInHierarchy).Count();
            }
        }

        public int CurrentSheepCount
        {
            get
            {
                return sheepList.Where(s => s.activeInHierarchy).Count();
            }
        }

        eco.Grid grid;

        public GameObject AgentControllerObject;
        private AgentController AgentController;

        public GameObject PlantExample;
        public GameObject SheepExample;
        public GameObject WolfExample;
        public GameObject WallExample;
        public GameObject DummyExample;


        public List<GameObject> plantList;
        public List<GameObject> wallList;
        public List<GameObject> sheepList;
        public List<GameObject> wolfList;
        public List<GameObject> dummyList;

        private Vector3 EnvPos
        {
            get
            {
                return transform.position;
            }
        }

        // Use this for initialization
        void Start()
        {
            plantList = new List<GameObject>();
            wallList = new List<GameObject>();
            sheepList = new List<GameObject>();
            wolfList = new List<GameObject>();
            dummyList = new List<GameObject>();

            AgentController = (AgentController)AgentControllerObject.GetComponent(typeof(AgentController));
            grid = (eco.Grid)this.gameObject.GetComponent(typeof(eco.Grid));
        }


        void FixedUpdate()
        {

        }

        /// <summary>
        /// Disables a plant gameobject.
        /// </summary>
        /// <param name="plant">Plant gameobject.</param>
        public void PlantDeath(GameObject plant)
        {
            plant.SetActive(false);
        }

        /// <summary>
        /// Disables a sheep gameobject.
        /// </summary>
        /// <param name="sheep">Sheep gameobject</param>
        public void SheepDeath(GameObject sheep)
        {
            sheep.SetActive(false);
        }

        /// <summary>
        /// Disables a sheep gameobject.
        /// </summary>
        /// <param name="sheep">Sheep gameobject</param>
        public void DummyDeath(GameObject sheep)
        {
            sheep.SetActive(false);
        }



        /// <summary>
        /// Disables a wolf gameobject.
        /// </summary>
        /// <param name="wolf">Wolf gameobject</param>
        public void WolfDeath(GameObject wolf)
        {
            wolf.SetActive(false);
        }

        /// <summary>
        /// Resets the level:
        ///  - Respawns the sheep
        ///  - Respawns the plants
        /// </summary>
        public void ResetLevel()
        {
            PositionSheep();
            PositionPlants();
            PositionWolf();
            PositionDummies();
        }

        /// <summary>
        /// Checks if all the plants are dead.
        /// </summary>
        /// <returns>True if all plants are dead.</returns>
        public bool AllPlantsDead()
        {
            return CurrentPlantCount == 0;
        }
        /// <summary>
        /// Checks if all the plants are dead.
        /// </summary>
        /// <returns>True if all plants are dead.</returns>
        public bool AllDummiesDead()
        {
            return CurrentDummyCount == 0;
        }

        /// <summary>
        /// Changes the lesson:
        ///  - updates the lists
        ///  - spawns the walls
        /// </summary>
        public void ChangeLesson()
        {
            UpdateLists();
            PositionWalls();
        }

        /// <summary>
        /// Updates all the lists with gameobjects.
        /// Lists affected:
        /// plantList
        /// wallList
        /// sheepList
        /// </summary>
        public void UpdateLists()
        {
            //Adding more plant instances if there's not enough
            if (plantList.Count < grid.CurrentLesson.MaxPlants)
            {
                var countToAdd = grid.CurrentLesson.MaxPlants - plantList.Count;
                for (int i = 0; i < countToAdd; i++)
                {
                    plantList.Add(Instantiate(PlantExample));
                    PlantDeath(plantList.Last());
                }
            }
            //If there are too many plant instances, destroy the unnecessary plants
            if (plantList.Count > grid.CurrentLesson.MaxPlants)
            {
                var countToDel = plantList.Count - (plantList.Count - grid.CurrentLesson.MaxPlants);
                for (int i = plantList.Count - 1; i > countToDel - 1; i--)
                {
                    var obj = plantList[i];
                    plantList.RemoveAt(i);
                    Destroy(obj);
                }
            }


            //Adding more dummy instances if there's not enough
            if (dummyList.Count < grid.CurrentLesson.DummyCount)
            {
                var countToAdd = grid.CurrentLesson.DummyCount - dummyList.Count;
                for (int i = 0; i < countToAdd; i++)
                {
                    dummyList.Add(Instantiate(DummyExample));
                    //hooking up behavior with the agent and vice versa
                    var behavior = (SheepBehavior)dummyList.Last().GetComponent(typeof(SheepBehavior));
                    if (behavior != null)
                    {
                        behavior.Spawner = this;
                        behavior.Die();
                    }
                }
            }
            //If there are too many dummy instances, destroy the unnecessary dummys
            if (dummyList.Count > grid.CurrentLesson.DummyCount)
            {
                var countToDel = dummyList.Count - (dummyList.Count - grid.CurrentLesson.DummyCount);
                for (int i = dummyList.Count - 1; i > countToDel - 1; i--)
                {
                    var obj = dummyList[i];
                    dummyList.RemoveAt(i);
                    Destroy(obj);
                }
            }


            //Adding more wall instances if there's not enough
            if (wallList.Count < grid.CurrentLesson.WallCount)
            {
                var countToAdd = grid.CurrentLesson.WallCount - wallList.Count;
                for (int i = 0; i < countToAdd; i++)
                {
                    wallList.Add(Instantiate(WallExample));
                }
            }
            //If there are too many wall instances, destroy the unnecessary walls
            if (wallList.Count > grid.CurrentLesson.WallCount)
            {
                var countToDel = wallList.Count - (wallList.Count - grid.CurrentLesson.WallCount);
                for (int i = wallList.Count - 1; i > countToDel - 1; i--)
                {
                    var obj = wallList[i];
                    wallList.RemoveAt(i);
                    Destroy(obj);
                }
            }



            //Adding more sheep instances if there's not enough
            if (sheepList.Count < grid.CurrentLesson.SheepCount)
            {
                var countToAdd = grid.CurrentLesson.SheepCount - sheepList.Count;
                for (int i = 0; i < countToAdd; i++)
                {
                    sheepList.Add(Instantiate(SheepExample));

                    //hooking up behavior with the agent and vice versa
                    var behavior = (SheepBehavior)sheepList.Last().GetComponent(typeof(SheepBehavior));
                    if (behavior != null)
                    {
                        behavior.Agent = AgentController.sheep.First(a => a.CurrentSheep == null);
                        behavior.Agent.CurrentSheep = behavior;
                        behavior.Spawner = this;
                        behavior.Die();
                    }
                }
            }
            //If there are too many sheep instances, destroy the unnecessary sheeps
            if (sheepList.Count > grid.CurrentLesson.SheepCount)
            {
                var countToDel = sheepList.Count - (sheepList.Count - grid.CurrentLesson.SheepCount);
                for (int i = sheepList.Count - 1; i > countToDel - 1; i--)
                {
                    var obj = sheepList[i];
                    sheepList.RemoveAt(i);
                    Destroy(obj);
                }
            }


            //Adding more wolf instances if there's not enough
            if (wolfList.Count < grid.CurrentLesson.WolfCount)
            {
                var countToAdd = grid.CurrentLesson.WolfCount - wolfList.Count;
                for (int i = 0; i < countToAdd; i++)
                {
                    wolfList.Add(Instantiate(WolfExample));

                    //hooking up behavior with the agent and vice versa
                    var behavior = (WolfBehavior)wolfList.Last().GetComponent(typeof(WolfBehavior));
                    if (behavior != null)
                    {
                        behavior.Agent = AgentController.wolf.First(a => a.CurrentWolf == null);
                        behavior.Agent.CurrentWolf = behavior;
                        behavior.Spawner = this;
                        behavior.Die();
                    }
                }
            }
            //If there are too many wolf instances, destroy the unnecessary wolfs
            if (wolfList.Count > grid.CurrentLesson.WolfCount)
            {
                var countToDel = wolfList.Count - (wolfList.Count - grid.CurrentLesson.WolfCount);
                for (int i = wolfList.Count - 1; i > countToDel - 1; i--)
                {
                    var obj = wolfList[i];
                    wolfList.RemoveAt(i);
                    Destroy(obj);
                }
            }


        }

        /// <summary>
        /// Spawns the walls
        /// called only on level change
        /// </summary>
        public void PositionWalls()
        {
            int j = 0;
            for (int i = 0; i < grid.GridList.Count; i++)
            {
                if (grid.GridList[i] == GridVals.Wall)
                {
                    this.wallList[j].transform.position = grid.Position(i) + EnvPos;
                    j++;
                }

            }
        }

        /// <summary>
        /// Spawns all the plants
        /// </summary>
        public void PositionPlants()
        {
            for (int i = 0; i < this.plantList.Count; i++)
            {
                if (i < plantList.Count)
                {
                    plantList[i].transform.position = grid.Position(grid.AssignID(GridVals.Plant)) + EnvPos;
                    plantList[i].SetActive(true);
                }
            }
        }

        /// <summary>
        /// Spawns all the plants
        /// </summary>
        public void PositionDummies()
        {
            for (int i = 0; i < this.dummyList.Count; i++)
            {
                if (i < dummyList.Count)
                {
                    dummyList[i].transform.position = grid.Position(grid.AssignID(GridVals.Dummy)) + EnvPos;
                    dummyList[i].SetActive(true);
                }
            }
        }


        public void PositionSheep()
        {
            for (int i = 0; i < this.sheepList.Count; i++)
            {
                if (i < sheepList.Count)
                {
                    sheepList[i].transform.position = grid.Position(grid.AssignID(GridVals.Sheep)) + EnvPos;
                    sheepList[i].SetActive(true);
                    ((SheepBehavior)sheepList[i].GetComponent(typeof(SheepBehavior))).Live();
                }
            }
        }

        public void PositionWolf()
        {
            for (int i = 0; i < this.wolfList.Count; i++)
            {
                if (i < wolfList.Count)
                {
                    wolfList[i].transform.position = grid.Position(grid.AssignID(GridVals.Wolf)) + EnvPos;
                    wolfList[i].SetActive(true);
                    ((WolfBehavior)wolfList[i].GetComponent(typeof(WolfBehavior))).Live();
                    ((WolfBehavior)wolfList[i].GetComponent(typeof(WolfBehavior))).ResetHunger();

                }
            }
        }
    }
}
