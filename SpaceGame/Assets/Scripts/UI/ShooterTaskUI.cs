using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShooterTaskUI : MonoBehaviour
{
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }
    
    void Update()
    {
        //Asteroids Destroyed:      000
        //Distance:                          000
        
        if (text != null && MainGame.Instance.PlayerShip != null)
        {
            string.Format("Asteroids Destroyed: {0}\nDistance: {1}",
                (MainGame.Instance.PlayerShip.AsteroidsDestroyed).ToString("000"),
                MainGame.Instance.PlayerShip.Velocity.magnitude.ToString("000"));
        }
    }
}
