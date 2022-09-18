using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Fake_Programming : MonoBehaviour
{
    public string fakeCode;
    public Text inputf;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void AddFake(int times)
    {
        for (int i = 0; i < times; i++)
        {
            if (inputf.text.Length < fakeCode.Length)
                inputf.text += fakeCode[inputf.text.Length];
            else
            {
                inputf.text = "";
            }
        }
    }
    // Update is called once per frame
    public void OnType(string s)
    {
        print("works!");
        AddFake(30);
    }
}
