using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    [SerializeField]
    Image timer;

    [SerializeField]
    float maxTime;

    float _currentTime;

    private void Start()
    {
        _currentTime = maxTime;
    }

    private void Update()
    {
        _currentTime -= Time.deltaTime;
        if (_currentTime < 0)
        {
            PlayerController2D player = FindObjectOfType<PlayerController2D>();
            StartCoroutine(player.Die());
            return;
        }

        timer.fillAmount = _currentTime / maxTime;
    }
}
