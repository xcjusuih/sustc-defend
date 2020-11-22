using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MoneyScript : MonoBehaviour
{
    int money;
    Text text;

    void changeMoney(int change){
        money+=change;
    }

    // Start is called before the first frame update
    void Start()
    {
        text=GetComponent<Text>();
        
    }

    // Update is called once per frame
    void Update()
    {
        changeMoney(1);
        text.text="$"+money;
    }
}
