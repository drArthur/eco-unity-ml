using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace eco
{
    public class SheepBehavior : MonoBehaviour
    {
        public SheepAgent Agent;
        public Spawner Spawner;
        
        public float MoveSpeed;
        public float MaxSight;
        public float MaxHealth;
        public float MaxHunger;
        public float DigestionTime;
        public int MinPlants;
        public bool Dummy;

        //public int PlantVision;
        //public int WallVision;
        //public int SheepVision;
        //public int WolfVision;

        public int PlantsEaten;

        private float currHealth;
        private float currHunger;
        private float distanceToReward;
        private Vector3 Movement;


        // Use this for initialization
        void Start()
        {
            PlantsEaten = 0;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (Agent != null)
            {
                transform.Translate(Movement);
                Movement = Vector3.zero;


                //float newDist = Vector3.Distance(FindClosestTargetPointsByName("PLANT(Clone)", 1, MaxSight)[0], transform.position);
                ////if(spawnScript.RewardLevel < 2)
                ////{
                //if (newDist < distanceToReward)
                //{
                //    distanceToReward = newDist;
                //    Agent.AddReward((1 - (newDist / MaxSight)) * 2.5f * Time.deltaTime);
                //    //reward += 0.2f * Time.deltaTime;
                //}
                //else if (newDist > distanceToReward)
                //{
                //    distanceToReward = newDist;
                //    Agent.AddReward(-0.5f * Time.deltaTime);
                //}
                //Agent.AddReward(-0.3f * Time.deltaTime);
                ////}

                if (currHunger < MaxHunger)
                    currHunger += (MaxHunger / 10f) * Time.deltaTime;
                currHealth -= currHunger * Time.deltaTime;
                if (currHealth < 0)
                {
                    this.Die();
                }


                //Debugging/monitoring

                Monitor.Log("Reward", Agent.GetReward(), MonitorType.slider, this.gameObject.transform);
                Monitor.Log("Cumulative Reward", Agent.GetCumulativeReward(), MonitorType.text, this.gameObject.transform);
                Monitor.Log("Health", this.currHealth, MonitorType.text, this.gameObject.transform);
                Monitor.Log("Hunger", this.currHunger, MonitorType.text, this.gameObject.transform);
                Monitor.Log("Distance", this.distanceToReward / this.MaxSight, MonitorType.text, this.gameObject.transform);
            }
        }

        public void MoveUp()
        {
            Movement = Vector3.forward * MoveSpeed * Time.deltaTime;
        }
        public void MoveDown()
        {
            Movement = Vector3.back * MoveSpeed * Time.deltaTime;
        }
        public void MoveLeft()
        {
            Movement = Vector3.left * MoveSpeed * Time.deltaTime;
        }
        public void MoveRight()
        {
            Movement = Vector3.right * MoveSpeed * Time.deltaTime;
        }

        public void Eat()
        {
            //TODO: Implement EAT function
        }

        /// <summary>
        /// The sheep dies:
        ///  - Agent counts as dead
        ///  - Gameobject is disabled
        /// </summary>
        public void Die()
        {
            if (!this.Dummy)
            {
                Agent.Dead = true;
                this.Spawner.SheepDeath(this.gameObject);
                this.currHealth = this.MaxHealth;
                this.currHunger = 0;

                if (PlantsEaten == 0)
                    Agent.AddReward(-0.4f);
                this.PlantsEaten = 0;
                //Agent.Done();
            }
            else            
                this.Spawner.DummyDeath(this.gameObject);
        }

        public void Live()
        {
            Agent.Dead = false;            
        }

        /// <summary>
        /// Gets all the observations the sheep makes.
        /// </summary>
        /// <returns>List of observations</returns>
        public List<float> GetObservations()
        {
            //Initializing the state list
            List<float> state = new List<float>();

            //Calculating the rotation
            var rotation = transform.rotation.eulerAngles.y / 180.0f - 1.0f;

            //Calculating the X and Z velocities
            var xVelocity = transform.GetComponent<Rigidbody>().velocity.x;
            var zVelocity = transform.GetComponent<Rigidbody>().velocity.z;

            var tmp = transform.position;
            //Detecting the 2 closest walls
            Vector3[] walls = FindClosestTargetPoints("wall", 8, MaxSight);

            //Detecting the closest plant
            Vector3[] rewards = FindClosestObjectPosByName("PLANT(Clone)", 2, MaxSight);

            GameObject[] sheep = FindClosestObjectByName("Sheep(Clone)", 1, MaxSight);

            GameObject[] wolf = FindClosestObjectByName("Wolf(Clone)", 2, MaxSight);
            //Temp variables for assigning distance values
            float tmpX = 0, tmpZ = 0;


            //Adding eaten plant data
            state.Add(this.PlantsEaten * 0.01f);

            //Adding rotation and velocities
            state.Add(rotation);
            state.Add(xVelocity);
            state.Add(zVelocity);

            //Filling wall data
            foreach (var e in walls)
            {
                tmpX = (e.x - transform.position.x) / MaxSight;
                tmpZ = (e.z - transform.position.z) / MaxSight;
                if (e == Vector3.zero)
                    state.Add(-1);
                else
                    state.Add(1);
                //state.Add(Vector3.Distance(transform.position, e) / MaxSight);
                state.Add(tmpX);
                state.Add(tmpZ);

                Debug.DrawLine(e, tmp, Color.blue);
            }

            //Filling reward data
            foreach (var e in rewards)
            {
                tmpX = (e.x - transform.position.x) / MaxSight;
                tmpZ = (e.z - transform.position.z) / MaxSight;

                if (e == Vector3.zero)
                    state.Add(-1);
                else
                    state.Add(1);

                //state.Add(Vector3.Distance(transform.position, e) / MaxSight);
                //Monitor.Log("Dist", (Vector3.Distance(e, transform.position) / MaxSight).ToString(), MonitorType.text, this.gameObject.transform);
                state.Add(tmpX);
                state.Add(tmpZ);
                
                Debug.DrawLine(e, tmp, Color.green);
            }

            //Adding position of the other sheep
            //and the closest 
            foreach(var e in sheep)
            {
                if (e != null)
                {
                    tmpX = (e.transform.position.x - transform.position.x) / MaxSight;
                    tmpZ = (e.transform.position.z - transform.position.z) / MaxSight;
                    state.Add(1);
                    //state.Add(Vector3.Distance(transform.position, e.transform.position) / MaxSight);
                    state.Add(tmpX);
                    state.Add(tmpZ);

                    Debug.DrawLine(e.transform.position, tmp, Color.magenta);

                    var behavior = (SheepBehavior)e.GetComponent(typeof(SheepBehavior));
                    var plant = behavior.FindClosestObjectByName("PLANT(Clone)", 1, behavior.MaxSight);
                    foreach (var p in plant)
                        if (p != null && InSight(p))
                        {
                            tmpX = (p.transform.position.x - transform.position.x) / MaxSight;
                            tmpZ = (p.transform.position.z - transform.position.z) / MaxSight;
                            //state.Add(Vector3.Distance(transform.position, p.transform.position) / MaxSight);
                            state.Add(1);
                            state.Add(tmpX);
                            state.Add(tmpZ);

                            Debug.DrawLine(p.transform.position, tmp, Color.cyan);
                        }
                        else
                        {
                            //for (int i = 0; i < 3; i++)
                            state.Add(-1);
                            state.Add(0);
                            state.Add(0);
                        }
                            
                            
                }
                else
                {
                    //for (int i = 0; i < 6; i++)
                    //    state.Add(0);
                    state.Add(-1);
                    state.Add(0);
                    state.Add(0);
                    state.Add(-1);
                    state.Add(0);
                    state.Add(0);
                }
                    

            }


            //Adding position of the wolf
            //and the closest 
            foreach (var e in wolf)
            {
                if (e != null)
                {
                    tmpX = (e.transform.position.x - transform.position.x) / MaxSight;
                    tmpZ = (e.transform.position.z - transform.position.z) / MaxSight;
                    //state.Add(Vector3.Distance(transform.position, e.transform.position) / MaxSight);
                    state.Add(1);
                    state.Add(tmpX);
                    state.Add(tmpZ);

                    Debug.DrawLine(e.transform.position, tmp, Color.red);

                    var behavior = (WolfBehavior)e.GetComponent(typeof(WolfBehavior));
                    var plant = behavior.FindClosestObjectByName("Sheep(Clone)", 1, behavior.MaxSight);
                    foreach (var p in plant)
                        if (p != null && InSight(p))
                        {
                            tmpX = (p.transform.position.x - transform.position.x) / MaxSight;
                            tmpZ = (p.transform.position.z - transform.position.z) / MaxSight;
                            //state.Add(Vector3.Distance(transform.position, p.transform.position) / MaxSight);
                            state.Add(1);
                            state.Add(tmpX);
                            state.Add(tmpZ);

                            Debug.DrawLine(p.transform.position, tmp, Color.cyan);
                        }
                        else
                        {
                            //for (int i = 0; i < 3; i++)
                            //    state.Add(0);
                            state.Add(-1);
                            state.Add(0);
                            state.Add(0);
                        }

                }
                else
                {
                    //for (int i = 0; i < 6; i++)
                    //    state.Add(0);
                    state.Add(-1);
                    state.Add(0);
                    state.Add(0);
                    state.Add(-1);
                    state.Add(0);
                    state.Add(0);
                }


            }


            state.Add(currHealth / (MaxHealth * 4));
            return state;
        }
        

        private void OnTriggerEnter(Collider other)
        {
            //Handling hitting a plant
            if (other.transform.gameObject.name == "PLANT(Clone)")
            {
                this.Spawner.PlantDeath(other.gameObject);
                Agent.AddReward(0.7f);

                currHealth += MaxHealth / 5f;
                if (currHealth > MaxHealth * 4)
                    currHealth = MaxHealth * 4;

                currHunger = 0;
                this.PlantsEaten++;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            //Handling hitting a wall
            if (Agent != null && collision.gameObject.tag == "wall")
            {
                Agent.AddReward(-0.5f);
                this.Die();
            }

            if (Agent != null && collision.gameObject.tag == "Wolf")
            {
                Agent.AddReward(-0.7f);
                this.Die();
            }

            if(this.Dummy && collision.gameObject.tag == "Wolf")
                this.Die();

            if (Agent != null && collision.gameObject.tag == "Sheep")
            {
                Agent.AddReward(-0.3f);
            }
        }

        /// <summary>
        /// Checks if the object is within sight boundaries
        /// </summary>
        /// <param name="g">the object</param>
        /// <returns>true if is in sight, false otherwise</returns>
        bool InSight(GameObject g)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, MaxSight);
            return hitColliders.FirstOrDefault(a => a == g) != null;
        }



        //Courtesy of batu
        //https://github.com/batu
        public Vector3[] FindClosestTargetPoints(string tag, int count, float maxDist)
        {
            Vector3 position = transform.position;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, maxDist);

            var closests = hitColliders
                .Where(o => o.gameObject.tag == tag)
                //&& Vector3.Distance(o.ClosestPoint(position), position) <= maxDist)
                .OrderBy(o => Vector3.Distance(o.ClosestPoint(position), position))
                .Take(count);

            Vector3[] closest_points = new Vector3[count];

            int i = 0;
            foreach (Collider col in closests)
            {
                closest_points[i] = col.ClosestPoint(position);
                i++;
            }
            if (closests.Count() < count)
                for (int a=0; i < count; i++)
                    closest_points[i] = Vector3.zero;
            return closest_points;
        }

        public Vector3[] FindClosestTargetPointsByName(string name, int count, float maxDist)
        {
            Vector3 position = transform.position;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, maxDist);

            var closests = hitColliders
                .Where(o => o.gameObject.name == name)
                //&& Vector3.Distance(o.ClosestPoint(position), position) <= maxDist)
                .OrderBy(o => Vector3.Distance(o.ClosestPoint(position), position))
                .Take(count);

            Vector3[] closest_points = new Vector3[count];

            int i = 0;
            foreach (Collider col in closests)
            {
                closest_points[i] = col.ClosestPoint(position);
                i++;
            }
            if (closests.Count() < count)
                for (int a = 0; i < count; i++)
                    closest_points[i] = Vector3.zero;
            return closest_points;
        }

        public Vector3[] FindClosestObjectPosByName(string name, int count, float maxDist)
        {
            Vector3 position = transform.position;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, maxDist);

            var closests = hitColliders
                .Where(o => o.gameObject.name == name)
                //&& Vector3.Distance(o.ClosestPoint(position), position) <= maxDist)
                .OrderBy(o => Vector3.Distance(o.transform.position, position))
                .Take(count);

            Vector3[] closest_points = new Vector3[count];

            int i = 0;
            foreach (Collider col in closests)
            {
                closest_points[i] = col.transform.position;
                i++;
            }
            if (closests.Count() < count)
                for (int a = 0; i < count; i++)
                    closest_points[i] = Vector3.zero;
            return closest_points;
        }

        public GameObject[] FindClosestObjectByName(string name, int count, float maxDist)
        {
            Vector3 position = transform.position;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, maxDist);

            var closests = hitColliders
                .Where(o => o.gameObject.name == name)
                //&& Vector3.Distance(o.ClosestPoint(position), position) <= maxDist)
                .OrderBy(o => Vector3.Distance(o.transform.position, position))
                .Take(count);

            GameObject[] closest_points = new GameObject[count];

            int i = 0;
            foreach (Collider col in closests)
            {
                closest_points[i] = col.gameObject;
                i++;
            }
            if (closests.Count() < count)
                for (int a = 0; i < count; i++)
                    closest_points[i] = null;
            return closest_points;
        }

    }
}
