using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using TMPro;
public class TreasureSeekerRLAgent : Agent
{
    private Rigidbody rlagent_rb;
    public bool allTreasuresFound;
    private bool allZombiesDead;
    private float turnSpeed = 200;  
    private float moveSpeed = 4;
    private EnvironmentParameters env_params;
    private TreasureSeekerArea tsarea;
    [SerializeField]private Transform shootorigin;
    [SerializeField] private bool canShoot;
    [SerializeField] private int cantShootcountdown;
    private int bulletmag;
    [SerializeField] private TextMeshPro bullets;
    [SerializeField] private TextMeshPro currentEpisosde;
    [SerializeField] private TextMeshPro success;
    [SerializeField] private TextMeshPro fail;
    [SerializeField] private TextMeshPro avgshotsmissedinsuccess;
    [SerializeField] private TextMeshPro avgscoresinfail;
    [SerializeField] private TextMeshPro avgcumulativereward;
    //private TestPerformanceRecorder recorder;
    public float totalEpisode;
    public float sucessfulEpisode;
    private float numberOfmistakesInSuccess;
    private float totalMistakeInSuccess;
    public float maxMistakeInSuccess;
    public float minMistakeInSuccess;
    public float avgMistakeInSuccess;
    public float score4fail;
    private float totalScore4fail;
    public float minScore4fail;
    public float avgScore4fail;
    public float maxScore4fail;
    public float cumulativeRewards;
    private float totalCumulativeRewards;
    public float maxCumulativeRewards;
    public float minCumulativeRewards;
    public float avgCumulativeRewards;
   




    public override void Initialize()
    {
        rlagent_rb = GetComponent<Rigidbody>();
        tsarea = GetComponentInParent<TreasureSeekerArea>();
        //recorder = GetComponent<TestPerformanceRecorder>();
        env_params = Academy.Instance.EnvironmentParameters;
        totalEpisode = 0;
        sucessfulEpisode = 0;
        numberOfmistakesInSuccess=0;
        totalMistakeInSuccess = 0;
        maxMistakeInSuccess=0;
        minMistakeInSuccess=5;
        avgMistakeInSuccess=0;
        score4fail = 0;
        totalScore4fail = 0;
        minScore4fail = 10;
        avgScore4fail = 0;
        maxScore4fail = 0;
        cumulativeRewards = 0;
        totalCumulativeRewards =0;
        maxCumulativeRewards = 0;
        minCumulativeRewards =10;
        avgCumulativeRewards = 0;
}

    public override void OnEpisodeBegin()
    {

        ResetParameters();
        tsarea.ResetTSArea();
        
        numberOfmistakesInSuccess = 0;
        score4fail = 0;
        cumulativeRewards = 0;
        


    }

    public void ResetParameters()
    {
       
        allTreasuresFound = false;
        allZombiesDead = false;
        canShoot = true;
        cantShootcountdown = Mathf.RoundToInt(env_params.GetWithDefault("agentnotshootingcountdown", 25f));
        bulletmag = Mathf.RoundToInt(env_params.GetWithDefault("agentbulletmagsize",10f));
        bullets.text = "Bullets: "+bulletmag.ToString();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(allTreasuresFound);
        sensor.AddObservation(allZombiesDead);
        sensor.AddObservation(canShoot);
        sensor.AddObservation(tsarea.treasureboxList.Count); //number of treasures left in the map

        
    
    }



    public override void OnActionReceived(ActionBuffers actions) 
    {
        
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;
        var discreteActions = actions.DiscreteActions; 
        var zpos = discreteActions[0];
        var yrotate = discreteActions[1];
        var isShoot = discreteActions[2];

        if (rlagent_rb.velocity.sqrMagnitude > 25f) // slow it down 
        {
            rlagent_rb.velocity *= 0.95f;
        }

        if (zpos == 1) {
            dirToGo = transform.forward * 1f;
        }
        else if (zpos == 2) {
            dirToGo = transform.forward * -1f;
        }

        if (yrotate == 1)
        {
            rotateDir = transform.up * -1f;
        }
        else if (yrotate == 2) 
        {
            rotateDir = transform.up * 1f;
        }

        if (isShoot ==1) 
        {
            shoot();
            
        }

        rlagent_rb.velocity *= 0.75f;   
        rlagent_rb.AddForce(dirToGo * moveSpeed, ForceMode.VelocityChange); //making use of rigidbody physics logic
        transform.Rotate(rotateDir, Time.fixedDeltaTime * turnSpeed); // not concerned with rigidbody, just rotating base on transform
    
    }

    
    
    private void Update()
    {
        currentEpisosde.text = "Current Episode: " + (totalEpisode +1).ToString();
        
/*        if (totalEpisode % 100 == 0) {
            
        }*/

        allZombiesDead = tsarea.zombieslist.All(b => b.gameObject.activeSelf == false);

        if (bulletmag <= 0 && allZombiesDead == false) //fail
        {
            totalEpisode++;
            recordRewardAtEndOfEachEpisode();
            fail.text = "Fail: " + (totalEpisode - sucessfulEpisode).ToString();
            totalScore4fail += score4fail;
            avgScore4fail = totalScore4fail / (totalEpisode - sucessfulEpisode);
            avgscoresinfail.text = "Mean Score(failures): " + avgScore4fail.ToString();
            if (minScore4fail > score4fail) {
                minScore4fail = score4fail;
                if (minScore4fail < 0) {
                    minScore4fail = 0;
                }
            }
            if (maxScore4fail < score4fail)
            {

                maxScore4fail = score4fail;
                if (maxScore4fail > 10) {
                    maxScore4fail = 10;
                }
            }
            
            EndEpisode();

        }

        else if (allTreasuresFound && allZombiesDead) //success
        {
            totalEpisode++;
            recordRewardAtEndOfEachEpisode();
            sucessfulEpisode++;
            success.text = "Pass: " + sucessfulEpisode.ToString();
            totalMistakeInSuccess += numberOfmistakesInSuccess;
            avgMistakeInSuccess = totalMistakeInSuccess / sucessfulEpisode;
            avgshotsmissedinsuccess.text = "Avg. missed shots(Passes): " + avgMistakeInSuccess.ToString();
            if (maxMistakeInSuccess < numberOfmistakesInSuccess) {
               
                maxMistakeInSuccess = numberOfmistakesInSuccess;
                if (maxMistakeInSuccess > 5) {
                    maxMistakeInSuccess = 5;
                }
            }
            if (minMistakeInSuccess > numberOfmistakesInSuccess) {
                minMistakeInSuccess = numberOfmistakesInSuccess;
                if (minMistakeInSuccess < 0) {
                    minMistakeInSuccess = 0;
                }
            }
            EndEpisode();

        }
    }
    private void FixedUpdate()
    {
         if (canShoot == false)
        {


            cantShootcountdown--;
            if (cantShootcountdown <= 0)
            {
                canShoot = true;
                
            }
        }


    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[2] = 0; //not shooting

        if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
        if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[1] = 2;
        }
        if (Input.GetKey(KeyCode.Space)) 
        {
            discreteActionsOut[2] = 1; //shooting
        }

    }
    void shoot()
    {

        if (canShoot == false || bulletmag <= 0) {
            return;
        }
        
        
        var layerMask = 1 << LayerMask.NameToLayer("zombies");
        if (Physics.Raycast(shootorigin.position, shootorigin.transform.forward, out var hit, 10f, layerMask))
        {
            
            Debug.DrawRay(shootorigin.position, shootorigin.transform.forward * 10f, Color.blue);
            hit.transform.GetComponent<Zombies>().registerDamage(100);



        }
        else {
            AddReward(-0.015f);
            cumulativeRewards -= 0.015f;
            Debug.DrawRay(shootorigin.position, shootorigin.transform.forward * 10f, Color.red);
            numberOfmistakesInSuccess++;

        }
        
        bulletmag--;
        bullets.text = "Bullets: "+bulletmag.ToString();
        canShoot = false;
        cantShootcountdown = 25;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall")) {
            AddReward(-0.001f);
            cumulativeRewards -= 0.001f;
        }

        if (collision.gameObject.CompareTag("treasure")) {

            AddReward(1f);
            cumulativeRewards+=1f;
            tsarea.removeOnetreasurebox(collision.gameObject);
            score4fail++;
          if (tsarea.treasureboxList.Count == 0)
            {
                allTreasuresFound = true;
                
          }
        }

        if (collision.gameObject.CompareTag("zombies")) //fail
        {
            totalEpisode++;
            fail.text = "Fail: " + (totalEpisode - sucessfulEpisode).ToString();
            AddReward(-0.5f);
            cumulativeRewards -= 0.5f;
            recordRewardAtEndOfEachEpisode();
            totalScore4fail += score4fail;
            avgScore4fail = totalScore4fail / (totalEpisode - sucessfulEpisode);
            avgscoresinfail.text = "Mean Score(failures): " + avgScore4fail.ToString();
            if (minScore4fail > score4fail)
            {
                minScore4fail = score4fail;
                if (minScore4fail < 0)
                {
                    minScore4fail = 0;
                }
            }
            if (maxScore4fail < score4fail)
            {

                maxScore4fail = score4fail;
                if (maxScore4fail > 10)
                {
                    maxScore4fail = 10;
                }
            }
            EndEpisode();

        }
    }

    private void recordRewardAtEndOfEachEpisode() {
        totalCumulativeRewards += cumulativeRewards;
        avgCumulativeRewards = totalCumulativeRewards / totalEpisode;
        avgcumulativereward.text = "Mean Cumulative Rewards(Pass & Fail): " + avgCumulativeRewards.ToString();
        if (minCumulativeRewards > cumulativeRewards)
        {
            minCumulativeRewards = cumulativeRewards;

        }
        if (maxCumulativeRewards < cumulativeRewards)
        {

            maxCumulativeRewards = cumulativeRewards;
            
            if (maxCumulativeRewards > 10)
            {
                maxCumulativeRewards = 10;
            }
        }

    }
}
