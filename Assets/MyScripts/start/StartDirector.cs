using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartDirector : MonoBehaviour {

    public GameObject AToggle;
    public GameObject VRToggle;
    public GameObject VARToggle;
    public GameObject WarningText;
    public GameObject MyCameraRig;

    public static int experiment_index = 0;

    public static int Experiment_index{
        get{
            return experiment_index;
        }
        set{
            experiment_index = value;
        }
    }

	// Use this for initialization
	void Start () {
        AToggle.GetComponent<Toggle>().isOn = false;
        VRToggle.GetComponent<Toggle>().isOn = false;
        VARToggle.GetComponent<Toggle>().isOn = false;
        experiment_index = 0;
        WarningText.GetComponent<Text>().enabled = false;
        //MyCameraRig.transform.position = new Vector3(AdjustDirector.CameraRigPos.x, AdjustDirector.CameraRigPos.y, AdjustDirector.CameraRigPos.z);
	}
	
	// Update is called once per frame
	void Update () {
        if (AToggle.GetComponent<Toggle>().isOn)
        {
            experiment_index = 1;
        }
        else if (VRToggle.GetComponent<Toggle>().isOn)
        {
            experiment_index = 2;
        }
        else if (VARToggle.GetComponent<Toggle>().isOn)
        {
            experiment_index = 3;
        } 
        else
        {
            experiment_index = 0;
        }
        //Debug.Log("experiment_index : " + experiment_index);
	}

    public void OnClick()
    {
        SceneManager.LoadScene("Adjust");
    }
}
