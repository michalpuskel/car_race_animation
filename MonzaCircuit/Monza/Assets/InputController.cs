using UnityEngine;

namespace CustomScripts
{
    public class CustomInputController : MonoBehaviour
    {
        public enum Controllable { FPS, RedBull, Ferrari };
        public Controllable InputIndex;

        // Use this for initialization
        void Start()
        {
            InputIndex = Controllable.FPS;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetAxis("Input_0") > 0)
            {
                InputIndex = Controllable.FPS;
            }
            else if (Input.GetAxis("Input_1") > 0)
            {
                InputIndex = Controllable.RedBull;
            }
            else if (Input.GetAxis("Input_2") > 0)
            {
                InputIndex = Controllable.Ferrari;
            }
        }
    }
}

