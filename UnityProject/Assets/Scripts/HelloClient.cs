using UnityEngine;

public class HelloClient : MonoBehaviour
{
    private HelloRequester _helloRequester;
    public InputManager inputManager;

    private void Start()
    {
        _helloRequester = new HelloRequester();
        _helloRequester.inputManager = inputManager;
        _helloRequester.Start();
    }

    private void OnDestroy()
    {
        _helloRequester.Stop();
    }
}