using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class InputManager : MonoBehaviour
{
    [System.Serializable]
    public class HeadData
    {
        public float x;
        public float y;
        public bool mouth_open;
    }

    [System.Serializable]
    public class HandData
    {
        public float x;
        public float y;
        public bool fist;
    }

    [System.Serializable]
    public class InputData
    {
        public HeadData head;
        public List<HandData> hands;
    }

    public Head head;
    public List<Hand> hands = new List<Hand>();
    public int screenWidth = 640;
    public int screenHeight = 480;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInputFromJSON(string jsonString)
    {
        // Parse the JSON string and set the head and hand properties accordingly
        // Example: {"head": {"x": 308, "y": 319, "mouth_open": false}, "hands": [{"x": 62, "y": 335, "fist": true}, {"x": 547, "y": 337, "fist": true}]}

        InputData inputData = JsonUtility.FromJson<InputData>(jsonString);

        if (head != null)
        {
            if (inputData.head.x != 0 || inputData.head.y != 0)
            {
                head.x = inputData.head.x - screenWidth / 2;
                head.y = -(inputData.head.y - screenHeight / 2);
                head.mouthOpen = inputData.head.mouth_open;
                head.active = true;
                                Debug.Log($"Head position: {head.x}, {head.y}, Mouth open: {head.mouthOpen}");
            }
            else
            {
                head.active = false;
            }
        }

        for (int i = 0; i < hands.Count; i++)
        {
            if (i < inputData.hands.Count)
            {
                HandData handData = inputData.hands[i];
                Hand hand = hands[i];
                hand.active = true;
                hand.x = handData.x - screenWidth / 2;
                hand.y = -(handData.y - screenHeight / 2);
                hand.fist = handData.fist;
            }
            else
            {
                hands[i].active = false;
            }
        }
    }
}
