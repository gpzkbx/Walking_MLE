using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerDirector : MonoBehaviour {

    public GameObject MyCameraRig;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var trackedObject = GetComponent<SteamVR_TrackedObject>();
        var device = SteamVR_Controller.Input((int)trackedObject.index);

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector3 ControllerPos = this.transform.position; //new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            Vector3 GoalPos = new Vector3(0, 1, 0);
            Vector3 MyCameraRigPos = MyCameraRig.transform.position; // new Vector3(MyCameraRig.transform.position.x, MyCameraRig.transform.position.y, MyCameraRig.transform.position.z);

            //AdjustDirector.CameraRigPos = new Vector3(0, 0, 0);
            Debug.Log("タッチパッドを深く押した");
        }
	}
}
