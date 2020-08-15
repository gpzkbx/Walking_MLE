using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ADirector : MonoBehaviour {

    public GameObject mysphere;
    public GameObject SteamVrCamera;
    public GameObject MyCameraRig;
    public GameObject GuideLine;

    float delta; // store the elapsed time since the begin of a trial
    int stimulus_flag;
    List<bool> trial_candidate; //the list from which the trial is selected. false for standard, true for comparision
    int spot_index_index;
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

    public static int[] trial_times = { 0, 0, 0, 0, 0, 0, 0}; // how many trials have been run
    //public static int[] trial_times = { 3, 5, 3, 7, 7, 4, 6 }; // how many trials have been run for interrupted scene
    public static List<int> spot_index_candidate = new List<int> { 0, 1, 2, 3, 4, 5, 6 };
    public static int spot_index;
    public static int trial_index;
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

    void Run_trial(){
        if (sphere_flag)
        {
            InsSphere.transform.Translate(0, 0, user_move);
        }
        if (this.delta > wait_span && this.stimulus_flag == 0){
            InsSphere = Instantiate(this.mysphere) as GameObject;

            spot_index_index = Random.Range(0, spot_index_candidate.Count);
            spot_index = spot_index_candidate[spot_index_index];
            while (trial_times[spot_index] >= trial_bound)
            {
                spot_index_candidate.RemoveAt((int)spot_index_index);
                spot_index_index = Random.Range(0, spot_index_candidate.Count);
                spot_index = spot_index_candidate[spot_index_index];
            }


            float px = spot_candidate[spot_index];

            trial_index = Random.Range(0, this.trial_candidate.Count);
            if (this.trial_candidate[trial_index])
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
            this.trial_candidate.RemoveAt(trial_index);

            this.stimulus_flag += 1;
        }
        if (this.delta > stimuli_interval + wait_span && this.stimulus_flag == 1){
            InsSphere = Instantiate(this.mysphere) as GameObject;

            float px = spot_candidate[spot_index];

            if (this.trial_candidate[0])
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

            this.stimulus_flag += 1;
        }

    }

	// Use this for initialization
    void Start () {
        this.delta = 0;
        this.stimulus_flag = 0;
        this.trial_candidate = new List<bool> {false, true};
        //MyCameraRig.transform.position = new Vector3(AdjustDirector.CameraRigPos.x, AdjustDirector.CameraRigPos.y, AdjustDirector.CameraRigPos.z);
        distance = 0;
        user_last = SteamVrCamera.transform.position;
        GuideLine.transform.position = new Vector3(AdjustDirector.reset_x, 0, AdjustDirector.reset_z);
        GuideLine.transform.rotation = AdjustDirector.reset_q;
    }
	
	// Update is called once per frame
    void Update () {
        user_current = SteamVrCamera.transform.position;
        user_move = Mathf.Sqrt(Mathf.Pow((user_current - user_last).x, 2) + Mathf.Pow((user_current - user_last).z, 2));
        distance += user_move;
        user_last = user_current;
        if (distance > freewalk)
        {
            this.delta += Time.deltaTime;
        }

        if (spot_index_candidate.Count == 0)
        {
            //Debug.Log("trial_times > trial_bound");
            SceneManager.LoadScene("Start");
        }else
        {
            Run_trial();

            if (this.delta > trial_span)
            {
                trial_times[spot_index] += 1;

                //certain spot's trial get tot the trial_bound
                if (trial_times[spot_index] >= trial_bound)
                {
                    spot_index_candidate.RemoveAt((int)spot_index_index);
                }
                //Debug.Log("Atrial times : " + trial_times);
                SceneManager.LoadScene("Interface");
            }
        }
    }
}
