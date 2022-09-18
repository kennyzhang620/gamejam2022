using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReactorTaskSpawner : MonoBehaviour
{
    public float Min;
    public float Max;

    public GameObject prefab;

    public bool Spawned = false;
    int State = 0;
    float intv = 0.05f;
    float init = 0.01f;

    float safety = 5;

    public Slider temps;
    public ParticleSystem[] parts;
    public GameObject success;
    public GameObject fail;
    public GameObject_States gs;

    float t = 0;
    // Start is called before the first frame update
    void OnEnable()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (t > intv)
        {
          //  print(State + "   " + transform.position.z + "  " +  Min + "  " +  Max);
            if (transform.position.z <= Min)
            {
                State = 0;
            }
            else if (transform.position.z >= Max)
            {
                State = 1;
            }

            if (State == 0)
            {
                transform.Translate(0, 0, 0.025f*(temps.value/15));
            }

            if (State == 1)
            {
                transform.Translate(0, 0, -0.025f * (temps.value / 10));
            }

            t = 0;
        }
        else
        {
            t += Time.deltaTime;
            temps.value += init * (temps.value / 30);


            if (temps.value <= 25)
            {
                safety--;
            }
            else if (safety <= 0)
            {
                safety = 5;
                success.SetActive(true);

                // exit
            }

            if (temps.value >= 100)
            {
                fail.SetActive(true);
                gameObject.SetActive(false);

                // game over
            }

            if (Input.GetAxis("Cancel") != 0)
            {
                if (safety <= 0)
                {
                    safety = 5;
                    gs.Activate();

                    // exit
                }
                else
                {
                    safety -= 1;
                }

            }

            int i = 0;
            foreach (ParticleSystem p in parts)
            {
                var emission = p.emission;
                emission.rateOverTime = 800 * (temps.value / 100);

                if (i == 0 && temps.value > 5)
                {
                    p.gameObject.SetActive(true);
                }
                else if (i == 1 && temps.value > 50)
                {
                    p.gameObject.SetActive(true);
                }
                else if (i == 2 && temps.value > 75)
                {
                    p.gameObject.SetActive(true);
                }
                else if (i == 3 && temps.value > 85)
                {
                    p.gameObject.SetActive(true);
                }
                else
                {
                    p.gameObject.SetActive(false);
                }
                i++;
            }
        }

        if (Input.GetAxis("Jump") != 0 && !Spawned)
        {
            Instantiate(prefab, transform.position, transform.rotation, null);
            Spawned = true;
        }
    }
}