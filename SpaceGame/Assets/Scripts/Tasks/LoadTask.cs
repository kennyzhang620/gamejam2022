using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LoadTask : MonoBehaviour
{

    public string SceneName;
    [FormerlySerializedAs("NDTaskName")] public GameObject NDTask;
    private Scene _lastScene;
    private GameObject _triggerPlayer;

    [SerializeField]int countdown = 100;
    // Start is called before the first frame update

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }


    // Update is called once per frame
    private void OnTriggerStay(Collider other)
    {
       // print(countdown);
        if (countdown-- < 0)
        {

            if (SceneName.Length > 0)
            {
                _lastScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(SceneName);
            }
            else if (NDTask != null)
            {
                other.gameObject.SetActive(false);
                _triggerPlayer = other.gameObject;
                NDTask.SetActive(true);
            }

            MainGame.Instance.CurrentTask = this;
        }
    }

    public void ExitTask()
    {
        if (NDTask != null)
        {
            NDTask.SetActive(false);
            _triggerPlayer.SetActive(true);
        }

        if (SceneName.Length > 0)
        {
            SceneManager.LoadScene(_lastScene.ToString());
        }
    }
}
