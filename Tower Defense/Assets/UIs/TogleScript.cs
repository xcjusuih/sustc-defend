using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogleScript : MonoBehaviour
{
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        //找到组件，动态添加监听，Lambda表达式，精简！

        GameObject.Find("Toggle").GetComponent<Toggle>().onValueChanged.AddListener(isOn => print(isOn ? "开" : "关"));
    }

    public void ListenInFunction()
    {
        text.fontSize=32;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
