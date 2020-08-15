using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySphereController : MonoBehaviour {

    float lifetime = 0.5f;
    float delta = 0;

	// Use this for initialization
	void Start () {
        VRDirector.sphere_flag = true;
        ADirector.sphere_flag = true;
        VARDirector.sphere_flag = true;
	}
	
	// Update is called once per frame
	void Update () {
        this.delta += Time.deltaTime;
        if (this.delta > this.lifetime) {
            ADirector.sphere_flag = false;
            VRDirector.sphere_flag = false;
            VARDirector.sphere_flag = false;

            Destroy(gameObject);
        }
	}
}
