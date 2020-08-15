using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class AdjustDirector : MonoBehaviour {
    public GameObject user_cam;
    public GameObject reset_point;
    public GameObject mycanvas;

    public static float reset_x = 0f;
    public static float reset_z = 0f;
    public static Quaternion reset_q = Quaternion.Euler(0, 0, 0);

    const float reset_offset = 0.5f;
    const float can2reset_x = 0;
    const float can2reset_y = 2f;
    const float can2reset_z = 5f;
    const int turns = 4;
    const int side_length = 4;
    const int reset_turn = turns * 10;

    static int reset_index = 0;

    int CW_OR_ATCW(int tts)
    {
        return (((tts / reset_turn) % 2) * 2 - 1) * -1;
    }

    // Use this for initialization
    void Start () {

        reset_point.transform.position = new Vector3(0, reset_point.transform.position.y, 0);
        reset_point.transform.rotation = Quaternion.Euler(0, 0, 0);
        int indi;

        switch (StartDirector.Experiment_index)
        {
            case 1:
                reset_index = ADirector.trial_times.Sum() % turns;
                indi = CW_OR_ATCW(ADirector.trial_times.Sum());
                if (indi < 0)
                {
                    reset_point.transform.RotateAround(reset_point.transform.position, Vector3.up, 90);
                }
                for (int i = 0; i < reset_index; i++)
                {
                    reset_point.transform.Translate(0, 0, side_length);
                    reset_point.transform.RotateAround(reset_point.transform.position, Vector3.up, 360f / turns * indi);
                }
                break;
            case 2:
                reset_index = VRDirector.trial_times.Sum() % turns;
                indi = CW_OR_ATCW(VRDirector.trial_times.Sum());
                if (indi < 0)
                {
                    reset_point.transform.RotateAround(reset_point.transform.position, Vector3.up, 90);
                }
                for (int i = 0; i < reset_index; i++)
                {
                    reset_point.transform.Translate(0, 0, side_length);
                    reset_point.transform.RotateAround(reset_point.transform.position, Vector3.up, 360f / turns * indi);
                }
                break;
            case 3:
                reset_index = VARDirector.trial_times.Sum() % turns;
                indi = CW_OR_ATCW(VARDirector.trial_times.Sum());
                if (indi < 0)
                {
                    reset_point.transform.RotateAround(reset_point.transform.position, Vector3.up, 90);
                }
                for (int i = 0; i < reset_index; i++)
                {
                    reset_point.transform.Translate(0, 0, side_length);
                    reset_point.transform.RotateAround(reset_point.transform.position, Vector3.up, 360f / turns * indi);
                }
                break;
            default:
                break;
        }

        mycanvas.transform.position = reset_point.transform.position;
        mycanvas.transform.rotation = reset_point.transform.rotation;
        mycanvas.transform.Translate(can2reset_x, can2reset_y, can2reset_z);

        reset_x = reset_point.transform.position.x;
        reset_z = reset_point.transform.position.z;
        reset_q = reset_point.transform.rotation;

    }
	
	// Update is called once per frame
	void Update() { 

	}

    public void OnClick()
    {
        
        switch (StartDirector.Experiment_index)
        {
            case 1:
                SceneManager.LoadScene("ATrial");
                break;
            case 2:
                SceneManager.LoadScene("VRTrial");
                break;
            case 3:
                SceneManager.LoadScene("VARTrial");
                break;
            default:
                SceneManager.LoadScene("Start");
                break;
        }
    }
}
