using UnityEngine;
using UnityEngine.UI;

namespace FLFlight.UI
{
    /// <summary>
    /// Shows throttle and speed of the player ship.
    /// </summary>
    public class InputUI : MonoBehaviour
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
                text.text = string.Format("{0:0.00}\n{1:0.00}\n{2:0.00}",
                    MainGame.Instance.PlayerShip.Pitch,
                    MainGame.Instance.PlayerShip.Yaw,
                    MainGame.Instance.PlayerShip.Roll);
            }
        }
    }
}
