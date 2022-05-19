using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TestPerformanceRecorder : MonoBehaviour
{
    [SerializeField] private TreasureSeekerRLAgent RLAgent;
    private int counter=0;

    private void Start()
    {
        RLAgent = GetComponent<TreasureSeekerRLAgent>();
    }

    private void Update()
    {


        WriteScores();
    }
    public void WriteScores()
    {
        string path = Application.dataPath + "/models/TestingResultsRecorder.txt";
        StreamWriter writer = new StreamWriter(path, true);
        if (RLAgent.totalEpisode != 0 && RLAgent.totalEpisode % 100 == 0)
        {

            counter++;
            if (counter == 1) {
                writer.WriteLine("Eps: " + RLAgent.totalEpisode.ToString() + "\n");
                writer.WriteLine("Passes: " + RLAgent.sucessfulEpisode.ToString() + "\n");
                writer.WriteLine("missed shots(passes): " + "\n");
                writer.WriteLine("max: " + RLAgent.maxMistakeInSuccess.ToString() + "\n");
                writer.WriteLine("min: " + RLAgent.minMistakeInSuccess.ToString() + "\n");
                writer.WriteLine("avg: " + RLAgent.avgMistakeInSuccess.ToString() + "\n");
                writer.WriteLine("scores(failures): " + "\n");
                writer.WriteLine("max: " + RLAgent.maxScore4fail.ToString() + "\n");
                writer.WriteLine("min: " + RLAgent.minScore4fail.ToString() + "\n");
                writer.WriteLine("avg: " + RLAgent.avgScore4fail.ToString() + "\n");
                writer.WriteLine("Cumulative Reward(Pass & Fail): " + "\n");
                writer.WriteLine("max: " + RLAgent.maxCumulativeRewards.ToString() + "\n");
                writer.WriteLine("min: " + RLAgent.minCumulativeRewards.ToString() + "\n");
                writer.WriteLine("avg: " + RLAgent.avgCumulativeRewards.ToString() + "\n");
                writer.WriteLine("======================================================" + "\n");
            }



        }
        else  {
            counter = 0;
        }

        writer.Close();

    }

}
