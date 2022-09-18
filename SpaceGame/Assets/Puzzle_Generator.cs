using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Generator : MonoBehaviour
{
    int a = 0;
    int b = 0;

    public int Stages = 8;
    public int DamageFactor = 0;
    public UnityEngine.UI.InputField textF;
    public UnityEngine.UI.Text question;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        a = Random.Range(-99, 99);
        b = Random.Range(-99, 99);

        question.text = a.ToString() + " + " + b.ToString() + " = ";
    }

    // Update is called once per frame
    public void CheckAnswer(string ans)
    {
        bool status = int.TryParse(ans, out int answD);

        print("ss");
        if (a + b == answD && Stages > 0)
        {
            Stages--;

            if (DamageFactor > 0)
            {
                DamageFactor--;
            }

            Start();
        }
        else if (Stages > 0)
        {
            textF.text = "ERROR!";
            DamageFactor++;
        }
        else
        {
            textF.text = "UPDATE COMPLETE!";
            anim.Play("BIOS_C");
        }
    }
}
