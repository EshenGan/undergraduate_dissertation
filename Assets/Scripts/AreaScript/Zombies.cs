using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using UnityEngine.AI;
public class Zombies : MonoBehaviour
{
    public TreasureSeekerRLAgent RLAgent;
    private NavMeshAgent nmagent;
    private int initialHP = 100;
    public int currentHP;
    [SerializeField] private int radius4wander =15;
    private float timer;
    private float wandertimer = 10;

    public int wandercountdown;
    public int chasecountdown;
    public bool isWander;

   
    private void Start()
    {
        currentHP = initialHP;
        nmagent = GetComponent<NavMeshAgent>();
        nmagent.speed = (float)Mathf.RoundToInt(Academy.Instance.EnvironmentParameters.GetWithDefault("zombiesspeed", 4f));
        timer = wandertimer;
        wandercountdown = Mathf.RoundToInt(Academy.Instance.EnvironmentParameters.GetWithDefault("zombieswanderingtime", 300f));
        chasecountdown = Mathf.RoundToInt(Academy.Instance.EnvironmentParameters.GetWithDefault("zombieschasingtime", 300f));
        isWander = true;

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

                if (timer>= wandertimer && isWander == true)  
                {
                    nmagent.speed = (float)Mathf.RoundToInt(Academy.Instance.EnvironmentParameters.GetWithDefault("zombiesspeed", 4f));
                    Vector3 nextDest = randNav(transform.position, radius4wander, -1);
                    nmagent.SetDestination(nextDest);
                    timer = 0;

                }
                else if(isWander == false) {
                    nmagent.speed = (float)Mathf.RoundToInt(Academy.Instance.EnvironmentParameters.GetWithDefault("zombiesspeed", 4f)) + 1f;
                    nmagent.SetDestination(RLAgent.gameObject.transform.position);


                }

    }
    private void FixedUpdate()
    {
        if (isWander) {
            
            wandercountdown--;
            if (wandercountdown <= 0)
            {
                isWander = false;
                wandercountdown = Mathf.RoundToInt(Academy.Instance.EnvironmentParameters.GetWithDefault("zombieswanderingtime", 300f));

            }
        }
        else if (isWander == false) {
            
            
            chasecountdown--;
            if (chasecountdown <= 0) {
                isWander = true;
                chasecountdown = Mathf.RoundToInt(Academy.Instance.EnvironmentParameters.GetWithDefault("zombieschasingtime", 300f));
            }
        }
        

        
    }

    public static Vector3 randNav(Vector3 oriPos, float distance, int layer)
    {
        Vector3 randDir = Random.insideUnitSphere * distance;

        randDir += oriPos;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDir, out navHit, distance, layer);

        return navHit.position;
    }

    public void registerDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            RLAgent.AddReward(1f);
            RLAgent.cumulativeRewards+=1f;
            gameObject.SetActive(false);
           
            RLAgent.score4fail++;
        }
    }
}
