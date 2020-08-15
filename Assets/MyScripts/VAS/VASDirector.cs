using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class VASDirector : MonoBehaviour {

    public GameObject MySphereVision;
    public GameObject MySphereAudition;
    public GameObject CamLeftEye;
    public GameObject CamRightEye;
    public GameObject MyCameraRig;

    int stimulus_flag; // 0 : ready for the first stimulus, 1 : ready for the second stimulus, 2 : do not generate stimulus
    bool noise_flag; // false : wait (do not change), true : ok to change
    float delta = 0; // store the elapsed time since the begin of a trial
    List<bool> trial_candidate = new List<bool> { false, true }; //the list from which the trial is selected. false for standard, true for comparision
    int ns_index;
    int ns_index_index;

    const float wait_span = 0.5f;
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
    // 0th for 10%, 1st for 23%, 2nd for 36%, 3rd for 49%, 4th for 62%
    public static int[] trial_times = Enumerable.Repeat<int>(0, 35).ToArray();
    public static List<float> noise_candidate = new List<float> { 0.1f, 0.23f, 0.36f, 0.49f, 0.62f };
    public static List<int> ns_index_candidate = new List<int>(Enumerable.Range(0, 35).ToArray());
    public static int noise_index;
    public static int trial_index;
    public static int spot_index;

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

    void Run_trial(){
        int random_dot_seed;
        if (this.noise_flag)
        {
            ns_index_index = Random.Range(0, ns_index_candidate.Count);
            ns_index = ns_index_candidate[ns_index_index];
            noise_index = ns_index % 5;
            float noise_rate = noise_candidate[(int)noise_index];
            //Debug.Log(noise_rate);
            this.CamRightEye.GetComponent<RenderRandomdotRight>().SwitchNoise(noise_rate);
            noise_flag = false;
        }

        if (this.delta > wait_span && stimulus_flag == 0){
            GameObject InsSphereVision = Instantiate(this.MySphereVision) as GameObject;
            GameObject InsSphereAudition = Instantiate(this.MySphereAudition) as GameObject;

            spot_index = ns_index / 5;
            float px = spot_candidate[spot_index];

            trial_index = Random.Range(0, trial_candidate.Count);
            if (trial_candidate[trial_index])
            {
                InsSphereVision.transform.position = new Vector3(px, stimulus_height, stimulus_distance);
                InsSphereAudition.transform.position = new Vector3(px, stimulus_height, stimulus_distance);
            }
            else
            {
                InsSphereVision.transform.position = new Vector3(spot_candidate[2], stimulus_height, stimulus_distance);
                InsSphereAudition.transform.position = new Vector3(spot_candidate[4], stimulus_height, stimulus_distance);
            }
            trial_candidate.RemoveAt(trial_index);

            random_dot_seed = Random.Range(0, 100);
            this.CamLeftEye.GetComponent<RenderRandomdotLeft>().SwitchRDSeed(random_dot_seed);
            this.CamRightEye.GetComponent<RenderRandomdotRight>().SwitchRDSeed(random_dot_seed);
            this.CamRightEye.GetComponent<RenderRandomdotRight>().SwitchNSSeed();
            this.CamRightEye.GetComponent<RenderRandomdotRight>().SwitchRSeed();
            this.CamLeftEye.GetComponent<RenderRandomdotLeft>().Enable_my_self();
            this.CamRightEye.GetComponent<RenderRandomdotRight>().Enable_my_self();

            stimulus_flag += 1;
        }
        if (this.delta > stimuli_interval + wait_span && stimulus_flag == 1){
            GameObject InsSphereVision = Instantiate(this.MySphereVision) as GameObject;
            GameObject InsSphereAudition = Instantiate(this.MySphereAudition) as GameObject;

            float px = spot_candidate[spot_index];

            if (trial_candidate[0])
            {
                InsSphereVision.transform.position = new Vector3(px, stimulus_height, stimulus_distance);
                InsSphereAudition.transform.position = new Vector3(px, stimulus_height, stimulus_distance);
            }
            else
            {
                InsSphereVision.transform.position = new Vector3(spot_candidate[2], stimulus_height, stimulus_distance);
                InsSphereAudition.transform.position = new Vector3(spot_candidate[4], stimulus_height, stimulus_distance);
            }

            random_dot_seed = Random.Range(0, 100);
            this.CamLeftEye.GetComponent<RenderRandomdotLeft>().SwitchRDSeed(random_dot_seed);
            this.CamRightEye.GetComponent<RenderRandomdotRight>().SwitchRDSeed(random_dot_seed);
            this.CamRightEye.GetComponent<RenderRandomdotRight>().SwitchNSSeed();
            this.CamRightEye.GetComponent<RenderRandomdotRight>().SwitchRSeed();
            this.CamLeftEye.GetComponent<RenderRandomdotLeft>().Enable_my_self();
            this.CamRightEye.GetComponent<RenderRandomdotRight>().Enable_my_self();

            stimulus_flag += 1;
        }

    }

	// Use this for initialization
	void Start () {
        if (this.CamLeftEye == null)
            Debug.Log("camlefteye is null");
        if (this.CamRightEye == null)
            Debug.Log("camrighteye is null");

        this.delta = 0;
        this.stimulus_flag = 0;
        this.noise_flag = true;
        this.trial_candidate = new List<bool> { false, true };
        //this.MyCameraRig.transform.position = new Vector3(AdjustDirector.CameraRigPos.x, AdjustDirector.CameraRigPos.y, AdjustDirector.CameraRigPos.z);
    }
	
	// Update is called once per frame
    void Update () {
        this.delta += Time.deltaTime;

        //all trials under each noise_rate over
        if (ns_index_candidate.Count == 0)
        {
            SceneManager.LoadScene("Start");
        }else
        {
            Run_trial();

            if (this.delta > trial_span)
            {
                trial_times[(int)ns_index] += 1;

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
