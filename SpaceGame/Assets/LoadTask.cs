using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTask : MonoBehaviour
{

    public string SceneName;
    public GameObject NDTaskName;
    public bool DisableCamera = true;
    int countdown = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTaskExit()
    {

    }

    // Update is called once per frame
    private void OnTriggerStay(Collider other)
    {
       // print(countdown);
        if (countdown-- < 0)
        {

            if (SceneName.Length > 0)
            {
                SceneManager.LoadScene(SceneName);
            }
            else if (NDTaskName != null)
            {
                if (DisableCamera)
                    other.gameObject.SetActive(false);

                NDTaskName.SetActive(true);
            }

            gameObject.SetActive(false);
        }
    }
}
