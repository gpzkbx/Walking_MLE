using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine;

public class SelectionInspector : MonoBehaviour {

    public GameObject LeftCheck;
    public GameObject RightCheck;
    public GameObject user_cam;
    public GameObject MyCanvas;
    public Text Progress;

    public static int check_index = -1;

    public static int Check_index {
        get{
            return check_index;
        }
        set{
            check_index = value;
        }
    }

    string GetProgress()
    {
        string progress = string.Empty;
        switch (StartDirector.Experiment_index)
        {
            case 1:
                progress = "実験" + StartDirector.Experiment_index + ": " + ADirector.trial_times.Sum() + "/" + ADirector.trial_bound * ADirector.trial_times.Length;
                break;
            case 2:
                progress = "実験" + StartDirector.Experiment_index + ": " + VRDirector.trial_times.Sum() + "/" + (VRDirector.trial_bound * VRDirector.trial_times.Length);
                break;
            case 3:
                progress = "実験" + StartDirector.Experiment_index + ": " + VARDirector.trial_times.Sum() + "/" + (VARDirector.trial_bound * VARDirector.trial_times.Length);
                break;
            default:
                progress = "0 / 0";
                break;
        }
        return progress;
    }

    // Use this for initialization
    void Start () {
        MyCanvas.transform.position = new Vector3(user_cam.transform.position.x, 2, user_cam.transform.position.z);
        MyCanvas.transform.rotation = AdjustDirector.reset_q;
        MyCanvas.transform.Translate(0, 0, 6);
        LeftCheck.GetComponent<Toggle>().isOn = false;
        RightCheck.GetComponent<Toggle>().isOn = false;
        check_index = -1;
        //this.MyCameraRig.transform.position = new Vector3(AdjustDirector.CameraRigPos.x, AdjustDirector.CameraRigPos.y, AdjustDirector.CameraRigPos.z);
        Progress.text = GetProgress();
	}

    // Update is called once per frame
    void Update()
    {
        if (LeftCheck.GetComponent<Toggle>().isOn == true)
        {
            Check_index = 0;
            //Debug.Log("left on");
        }
        if (RightCheck.GetComponent<Toggle>().isOn == true)
        {
            Check_index = 1;
            //Debug.Log("right on");
        }
        if (LeftCheck.GetComponent<Toggle>().isOn == false && RightCheck.GetComponent<Toggle>().isOn == false)
            Check_index = -1;
    }
}
