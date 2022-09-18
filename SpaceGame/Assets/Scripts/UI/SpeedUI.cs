using UnityEngine;
using UnityEngine.UI;

namespace FLFlight.UI
{
    /// <summary>
    /// Shows throttle and speed of the player ship.
    /// </summary>
    public class SpeedUI : MonoBehaviour
    {
        private Text text;

        private void Awake()
        {
            text = GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            if (text != null && MainGame.Instance.PlayerShip != null)
            {
                text.text = string.Format("THR: {0}\nSPD: {1}",
                                          (MainGame.Instance.PlayerShip.Throttle * 100.0f).ToString("000"),
                                          MainGame.Instance.PlayerShip.Velocity.magnitude.ToString("000"));
            }
        }
    }
}

