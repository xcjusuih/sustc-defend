using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnUi : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnGUI()
    {
        if (GUILayout.Button("button"))
        {
            this.transform.Translate(0, 0, 1);
            this.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
