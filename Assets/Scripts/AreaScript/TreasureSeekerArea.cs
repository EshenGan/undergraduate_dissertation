using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using TMPro;

public class TreasureSeekerArea : MonoBehaviour
{
    private int treasurecount = 5;
    [SerializeField]private TreasureSeekerRLAgent RLAgent;
    [SerializeField] private TreasureBox treasurebox;
    [SerializeField] private List<MazeWalls> walllist;
    public List<GameObject> treasureboxList;
    public List<Zombies> zombieslist;
    private List<int> randomIndexList;

    public void ResetTSArea() // called every new episode 
    {
        removealltreasurebox();
        removeallZombies(zombieslist.Count);
        removeMaze(walllist.Count);
        randomIndexList = new List<int>();
        generateMaze(Mathf.RoundToInt(Academy.Instance.EnvironmentParameters.GetWithDefault("mazeactivewalls", 28f)));
        SpawnAgent();
        randomlySpawnZombie(Mathf.RoundToInt(Academy.Instance.EnvironmentParameters.GetWithDefault("zombiescount", 5f)));
        randomlySpawnNewSetOfTreasureBox(treasurecount);
    }

    public void removeOnetreasurebox(GameObject treasurebox)
    {

        Destroy(treasurebox);
        treasureboxList.Remove(treasurebox);

    }


    private void removealltreasurebox()
    {
        if (treasureboxList != null)
        {
            for (int i = 0; i < treasureboxList.Count; i++)
            {
                if (treasureboxList[i] != null)
                {
                    Destroy(treasureboxList[i]);
                    
                   
                }
            }
        }
        
        treasureboxList = new List<GameObject>();
        
    }


    private void removeallZombies(int totalzombiescount) {
        for(int i=0; i< totalzombiescount; i++){
            var currentzombies = zombieslist[i];
            currentzombies.gameObject.SetActive(false);
        }

    }

    private void removeMaze(int totalmazewallcount) {
        for (int i = 0; i < totalmazewallcount; i++)
        {
            var currentwall = walllist[i];
            currentwall.gameObject.SetActive(false);
        }

    }


    private void randomlySpawnNewSetOfTreasureBox(int BoxMaxNum)
    {
        
        List<Vector3> treasurepositions;
        treasurepositions = new List<Vector3>();
        setpos(BoxMaxNum,-10f,18f, treasurepositions, false,0);

        for (int i = 0; i < BoxMaxNum; i++)
        {
            
            GameObject treasureboxObject = Instantiate<GameObject>(treasurebox.gameObject, treasurepositions[i],Quaternion.identity);
            
            checkOverlap(treasureboxObject,BoxMaxNum, treasurepositions, -10f,18f,i);
            treasureboxList.Add(treasureboxObject);
            treasureboxObject.transform.SetParent(transform);
        }

    }


    private void randomlySpawnZombie(int MaxZombieNum) {
        List<Vector3> zombiespositions;
        zombiespositions = new List<Vector3>();
        setpos(MaxZombieNum, -10f, 18f, zombiespositions, false, 0);
        for (int i = 0; i < MaxZombieNum; i++)
        {
            var currentZombie = zombieslist[i];
            currentZombie.gameObject.transform.position = zombiespositions[i];
            currentZombie.isWander = true;
            currentZombie.wandercountdown = Mathf.RoundToInt(Academy.Instance.EnvironmentParameters.GetWithDefault("zombieswanderingtime", 300f));
            currentZombie.chasecountdown = Mathf.RoundToInt(Academy.Instance.EnvironmentParameters.GetWithDefault("zombieschasingtime", 300f));
            currentZombie.gameObject.SetActive(true);
            currentZombie.currentHP = 100;
            
        }

    }
    private void SpawnAgent() {
        Rigidbody rlagent_rb = RLAgent.GetComponent<Rigidbody>();
        rlagent_rb.velocity = Vector3.zero;
        rlagent_rb.angularVelocity = Vector3.zero;
        
        RLAgent.transform.position = new Vector3(UnityEngine.Random.Range(-10f,18f) + transform.position.x, 0.5f, 31f + transform.position.z);
        RLAgent.transform.rotation = Quaternion.Euler(new Vector3(0f, UnityEngine.Random.Range(0, 360)));
        
    }


    private void generateMaze(int wallnum)
    {
        
        for (int j = 0; j < wallnum; j++)
        {
            UniqueRandNum();
            var currentwall = walllist[randomIndexList[j]];
            currentwall.gameObject.SetActive(true);

        }



    }

    private void UniqueRandNum() 
    {
        

        
        
            var randomIndex = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 27f));

            if (!randomIndexList.Contains(randomIndex))
            {
            randomIndexList.Add(randomIndex);
            }
            else
            {
                UniqueRandNum();
            }

        


    }

    public void checkOverlap(GameObject Object,int amount, List<Vector3> vector3array, float min, float max, int overlapindex)
    {
        bool isOverlap = Physics.CheckBox(Object.transform.localPosition, Object.transform.localScale / 2, Quaternion.identity);
        
        if (isOverlap) {
            
            setpos(amount, min, max, vector3array, isOverlap, overlapindex);
            Object.transform.position = vector3array[overlapindex];
            checkOverlap(Object,amount,vector3array,min,max,overlapindex);
        }  
    }

    private void setpos(int amount,float min, float max, List<Vector3> vector3array, bool overlapflag, int overlapindex) {

        float offset = (max - min) / amount;
        float initialx = UnityEngine.Random.Range(min, (max - (offset * (amount - 1))));
        if (overlapflag == false) {
            for (int j = 0; j < amount; j++)
            {

                vector3array.Add(new Vector3(initialx + (offset*j) + transform.position.x, 0.5f, UnityEngine.Random.Range(0f, 27f) + transform.position.z));
            }
        }

        if (overlapflag == true) {
            vector3array[overlapindex] = new Vector3(initialx + (offset*overlapindex) + transform.position.x, 0.5f, UnityEngine.Random.Range(0f, 27f) + transform.position.z);

        }
    }
}