using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //System.IO.FileInfo, System.IO.StreamReader, System.IO.StreamWriter
using System; //Exception
using System.Text; //Encoding

public class AnswerWriter : MonoBehaviour {

    private string guitxt = "";
    private string outputFileName;

    void Awake()
    {
#if Unity_EDITOR
        if (Directory.Exists(Application.dataPath + "/Data/OutputAnswer"))
        {
        Debug.Log("Data Folder Exists");
        }
        else
        {
        Directory.CreateDirectory(Application.dataPath + "/Data/OutputAnswer");
        Debug.Log("Data Folder Create");
        }
#else
        if (Directory.Exists(Application.dataPath + "/Data/OutputAnswer"))
        {
            Debug.Log("Data Folder Exists");
        }
        else
        {
            Directory.CreateDirectory(Application.dataPath + "/Data/OutputAnswer");
            Debug.Log("Data Folder Create");
        }

        if (Directory.Exists(Application.dataPath + "/Data/Rec"))
        {
            Debug.Log("Rec Folder Exists");
        }
        else
        {
            Directory.CreateDirectory(Application.dataPath + "/Data/Rec");
            Debug.Log("Rec Folder Create");
        }
        
#endif
    }


    // Use this for initialization
    void Start()
    {
        string buftxt = "/Data/OutputAnswer/" + "OutputAnswer" + ".csv";
        outputFileName = buftxt;
        ReadFile();
    }

    // Update is called once per frame
    void Update()
    {
        switch (StartDirector.Experiment_index)
        {
            case 1:
                guitxt = StartDirector.Experiment_index + ", " 
                                      + 0 + ", " + ADirector.Trial_index + ", " 
                                      + ADirector.Spot_index + ", " + SelectionInspector.Check_index;
                break;
            case 2:
                guitxt = StartDirector.Experiment_index + ", "
                                      + VRDirector.Noise_index + ", "+ VRDirector.Trial_index + ", "
                                      + VRDirector.Spot_index + ", "+ SelectionInspector.Check_index;
                break;
            case 3:
                guitxt = StartDirector.Experiment_index + ", "
                                      + VARDirector.Noise_index + ", "+ VARDirector.Trial_index + ", "
                                      + VARDirector.Spot_index + ", "+ SelectionInspector.Check_index;
                break;
            default:
                guitxt = "";
                break;
        }
    }

    public void WriteFile()
    {
        FileInfo fi = new FileInfo(Application.dataPath + "/" + outputFileName);
        using (StreamWriter sw = fi.AppendText())
        {
            
            sw.WriteLine(guitxt);
        }

        FileInfo afi = new FileInfo(Application.dataPath + "/" + "/Data/Rec/" + "arec" + ".csv");
        FileInfo vfi = new FileInfo(Application.dataPath + "/" + "/Data/Rec/" + "vrec" + ".csv");
        FileInfo vafi = new FileInfo(Application.dataPath + "/" + "/Data/Rec/" + "varec" + ".csv");
        string rectext = "";

        switch (StartDirector.Experiment_index)
        {
            case 1:
                for (int i = 0; i < ADirector.trial_times.Length; i++)
                    rectext += ADirector.trial_times[i] + " ,";
                using (StreamWriter sw = afi.AppendText())
                {
                    sw.WriteLine(rectext);
                }
                break;
            case 2:
                for (int i = 0; i < VRDirector.trial_times.Length; i++)
                    rectext += VRDirector.trial_times[i] + " ,";
                using (StreamWriter sw = vfi.AppendText())
                {
                    sw.WriteLine(rectext);
                }
                break;
            case 3:
                for (int i = 0; i < VARDirector.trial_times.Length; i++)
                    rectext += VARDirector.trial_times[i] + " ,";
                using (StreamWriter sw = vafi.AppendText())
                {
                    sw.WriteLine(rectext);
                }
                break;
            default:
                break;
        }
    }

    void ReadFile()
    {
        FileInfo fi = new FileInfo(Application.dataPath + "/" + outputFileName);
        try
        {
            using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8))
            {
                guitxt = sr.ReadToEnd();
            }
        }
#pragma warning disable 0168
        catch (Exception e)
        {
#pragma warning disable 0168
            WriteFile();
            guitxt = "experiment_index, noise_index, trial_index, spot_index, user_selection";
            WriteFile();
        }
    }
}
