﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class VRDirector : MonoBehaviour
{

    public GameObject mysphere;
    public GameObject SteamVrCamera;
    public GameObject MyCameraRig;
    public GameObject GuideLinetest;

    int stimulus_flag; // 0 : ready for the first stimulus, 1 : ready for the second stimulus, 2 : do not generate stimulus
    bool noise_flag; // false : wait (do not change), true : ok to change
    List<bool> trial_candidate; //the list from which the trial is selected. false for standard, true for comparision
    float delta; // store the elapsed time since the begin of a trial
    int ns_index_index;
    int ns_index;
    float distance;
    float user_move;
    GameObject InsSphere;

    const float wait_span = 0.5f;
    const float freewalk = 1f;
    const float stimuli_interval = 1.0f; // the interval between the two stimuli in one trial
    const float trial_span = stimuli_interval * 2 + wait_span; // the whole span for a single trial
    public const int trial_bound = 15;
    const float stimulus_distance = 5f;
    const float stimulus_height = 1f;

    //spots where stimulus should appear
    List<float> spot_candidate = new List<float> {
        stimulus_distance * Mathf.Tan(Mathf.Deg2Rad * -4.5f),
        stimulus_distance * Mathf.Tan(Mathf.Deg2Rad * -3f),
        stimulus_distance * Mathf.Tan(Mathf.Deg2Rad * -1.5f),
        stimulus_distance * Mathf.Tan(Mathf.Deg2Rad * 0),
        stimulus_distance * Mathf.Tan(Mathf.Deg2Rad * 1.5f),
        stimulus_distance * Mathf.Tan(Mathf.Deg2Rad * 3f),
        stimulus_distance * Mathf.Tan(Mathf.Deg2Rad * 4.5f)
    };

    // how many trials have been run
    // 0th for 0.9, 1st for 0.45, 2nd for 0.23, 3rd for 0.12, 4th for 0.06
    public static int[] trial_times = Enumerable.Repeat<int>(0, 35).ToArray();
    //public static int[] trial_times = {10 ,5 ,8 ,10 ,5 ,9 ,9 ,12 ,9 ,8 ,7 ,3 ,9 ,2 ,7 ,7 ,8 ,8 ,4 ,5 ,8 ,8 ,8 ,11 ,3 ,9 ,7 ,8 ,8 ,7 ,5 ,2 ,9 ,9 ,5  };
    //public static List<float> noise_candidate = new List<float> { 0.06f, 0.06f, 0.06f, 0.06f, 0.06f };
    //public static List<float> noise_candidate = new List<float> { 0.06f, 0.06f, 0.06f, 0.06f, 0.06f };
    public static List<float> noise_candidate = new List<float> { 0.14f, 0.12f, 0.10f, 0.08f, 0.06f };
    public static List<int> ns_index_candidate = new List<int>(Enumerable.Range(0, 35).ToArray());
    public static int spot_index;
    public static int trial_index = 0;
    public static int noise_index = 0;
    public static bool sphere_flag = false;
    public static Vector3 user_last;
    static Vector3 user_current;

    public static int Spot_index
    {
        get
        {
            return spot_index;
        }
        set
        {
            spot_index = value;
        }
    }

    public static int Trial_index
    {
        get
        {
            return trial_index;
        }
        set
        {
            trial_index = value;
        }
    }

    public static int Noise_index
    {
        get
        {
            return noise_index;
        }
        set
        {
            noise_index = value;
        }
    }

    void Run_trial()
    {
        if (this.noise_flag)
        {
            ns_index_index = Random.Range(0, ns_index_candidate.Count);
            ns_index = ns_index_candidate[ns_index_index];
            while (trial_times[(int)ns_index] >= trial_bound)
            {
                ns_index_candidate.RemoveAt((int)ns_index_index);
                ns_index_index = Random.Range(0, ns_index_candidate.Count);
                ns_index = ns_index_candidate[ns_index_index];
            }
            noise_index = ns_index % 5;
            float noise_rate = noise_candidate[(int)noise_index];
            //Debug.Log("noise_rate = " + noise_rate);
            this.SteamVrCamera.GetComponent<DynamicFogAndMist.DynamicFog>().ChangeNoiseStrength(noise_rate);
            noise_flag = false;
        }

        if (sphere_flag)
        {
            InsSphere.transform.Translate(0, 0, user_move);
        }

        if (this.delta > wait_span && stimulus_flag == 0)
        {
            InsSphere = Instantiate(this.mysphere) as GameObject;

            spot_index = ns_index / 5;
            float px = spot_candidate[spot_index];
            //Debug.Log("spot_index : " + px);

            trial_index = Random.Range(0, trial_candidate.Count);
            if (trial_candidate[trial_index])
            {
                InsSphere.transform.position = new Vector3(AdjustDirector.reset_x, stimulus_height, AdjustDirector.reset_z);
                InsSphere.transform.rotation = AdjustDirector.reset_q;
                InsSphere.transform.Translate(px, 0, stimulus_distance + Mathf.Sqrt(Mathf.Pow(SteamVrCamera.transform.position.z - AdjustDirector.reset_z, 2) + Mathf.Pow(SteamVrCamera.transform.position.x - AdjustDirector.reset_x, 2)));
                //InsSphere.transform.position = new Vector3(px, stimulus_height, stimulus_distance);
            }
            else
            {
                //InsSphere.transform.position = new Vector3(0, stimulus_height, stimulus_distance);
                InsSphere.transform.position = new Vector3(AdjustDirector.reset_x, stimulus_height, AdjustDirector.reset_z);
                InsSphere.transform.rotation = AdjustDirector.reset_q;
                InsSphere.transform.Translate(0, 0, stimulus_distance + Mathf.Sqrt(Mathf.Pow(SteamVrCamera.transform.position.z - AdjustDirector.reset_z, 2) + Mathf.Pow(SteamVrCamera.transform.position.x - AdjustDirector.reset_x, 2)));
            }
            trial_candidate.RemoveAt(trial_index);

            //Debug.Log("trial index : " + trial_index);

            this.SteamVrCamera.GetComponent<DynamicFogAndMist.DynamicFog>().Enable_my_self();

            stimulus_flag += 1;
        }
        if (this.delta > stimuli_interval + wait_span && stimulus_flag == 1)
        {
            InsSphere = Instantiate(this.mysphere) as GameObject;

            float px = spot_candidate[spot_index];

            if (trial_candidate[0])
            {
                //InsSphere.transform.position = new Vector3(px, stimulus_height, stimulus_distance);
                InsSphere.transform.position = new Vector3(AdjustDirector.reset_x, stimulus_height, AdjustDirector.reset_z);
                InsSphere.transform.rotation = AdjustDirector.reset_q;
                InsSphere.transform.Translate(px, 0, stimulus_distance + Mathf.Sqrt(Mathf.Pow(SteamVrCamera.transform.position.z - AdjustDirector.reset_z, 2) + Mathf.Pow(SteamVrCamera.transform.position.x - AdjustDirector.reset_x, 2)));
            }
            else
            {
                //InsSphere.transform.position = new Vector3(0, stimulus_height, stimulus_distance);
                InsSphere.transform.position = new Vector3(AdjustDirector.reset_x, stimulus_height, AdjustDirector.reset_z);
                InsSphere.transform.rotation = AdjustDirector.reset_q;
                InsSphere.transform.Translate(0, 0, stimulus_distance + Mathf.Sqrt(Mathf.Pow(SteamVrCamera.transform.position.z - AdjustDirector.reset_z, 2) + Mathf.Pow(SteamVrCamera.transform.position.x - AdjustDirector.reset_x, 2)));
            }

            //Debug.Log("trial index : " + trial_index);

            this.SteamVrCamera.GetComponent<DynamicFogAndMist.DynamicFog>().Enable_my_self();

            stimulus_flag += 1;
        }

    }

    // Use this for initialization
    void Start()
    {
        if (this.SteamVrCamera == null)
            Debug.Log("steamcamera is null");

        this.SteamVrCamera.GetComponent<DynamicFogAndMist.DynamicFog>().Initialize();
        this.delta = 0;
        this.stimulus_flag = 0;
        this.noise_flag = true;
        this.trial_candidate = new List<bool> { false, true };
        distance = 0;
        //this.MyCameraRig.transform.position = new Vector3(AdjustDirector.CameraRigPos.x, AdjustDirector.CameraRigPos.y, AdjustDirector.CameraRigPos.z);
        user_last = SteamVrCamera.transform.position;

        GuideLinetest.transform.position = new Vector3(AdjustDirector.reset_x, 0, AdjustDirector.reset_z);
        GuideLinetest.transform.rotation = AdjustDirector.reset_q;
    }

    // Update is called once per frame
    void Update()
    {
        user_current = SteamVrCamera.transform.position;
        user_move = Mathf.Sqrt(Mathf.Pow((user_current - user_last).x, 2) + Mathf.Pow((user_current - user_last).z, 2));
        distance += user_move;
        user_last = user_current;
        if (distance >= freewalk)
        {
            this.delta += Time.deltaTime;
        }

        //all trials under each noise_rate over
        if (ns_index_candidate.Count == 0)
        {
            SceneManager.LoadScene("Start");
        }else
        {
            Run_trial();

            //called when a whole trial (with a standard & a comparison trial) is over
            if (this.delta > trial_span)
            {
                trial_times[(int)ns_index] += 1; //trial times plus 1

                //certain noise_rate's trial get to the trial_bound
                if (trial_times[(int)ns_index] >= trial_bound)
                {
                    ns_index_candidate.RemoveAt((int)ns_index_index);
                }
                SceneManager.LoadScene("Interface");
            }
        }
    }
}
