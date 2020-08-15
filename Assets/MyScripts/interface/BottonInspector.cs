using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BottonInspector : MonoBehaviour {

    public GameObject AnswerWriter;
    public GameObject Warning;

	// Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        if (SelectionInspector.Check_index == -1)
        {
            Debug.Log("どっちかと選択してください");
            Warning.GetComponent<Text>().enabled = true;
        }
        else
        {
            AnswerWriter.GetComponent<AnswerWriter>().WriteFile();
            SceneManager.LoadScene("Adjust");
        }
    }
}
