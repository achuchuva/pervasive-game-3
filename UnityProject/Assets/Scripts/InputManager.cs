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
        public bool active;
    }

    [System.Serializable]
    public class HandData
    {
        public float x;
        public float y;
        public bool fist;
        public bool active;
        public string hand_type; // "Left" or "Right"
    }

    [System.Serializable]
    public class InputData
    {
        public HeadData head;
        public List<HandData> hands;
    }

    public Head head;
    public Hand leftHand;
    public Hand rightHand;
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

        if (head != null && inputData.head.active)
        {
            head.x = inputData.head.x - screenWidth / 2;
            head.y = -(inputData.head.y - screenHeight / 2);
            head.mouthOpen = inputData.head.mouth_open;
        }

        for (int i = 0; i < inputData.hands.Count; i++)
        {
            HandData handData = inputData.hands[i];
            Hand hand = (handData.hand_type == "Left") ? leftHand : rightHand;
            hand.x = handData.x - screenWidth / 2;
            hand.y = -(handData.y - screenHeight / 2);
            hand.fist = handData.fist;
        }
    }
}
