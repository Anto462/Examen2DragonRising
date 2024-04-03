using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightCicleController : MonoBehaviour
{
    [SerializeField]
    Light2D globalLight;

    [SerializeField]
    LightCicle[] lightCicles;

    float _cicleTime;
    float _currentTime;

    int _currentCicle = 0;
    int _nextCicle = 1;

    private void Start()
    {
        LightCicle lightCicle = lightCicles[0];
        _cicleTime = lightCicle.getTime();
        _currentTime = 0.0F;

        globalLight.color = lightCicle.getColor();
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= _cicleTime) 
        {
            _currentCicle = _nextCicle;
            _nextCicle = _nextCicle + 1 < lightCicles.Length
                ? _nextCicle + 1
                : 0;

            _cicleTime = lightCicles[_currentCicle].getTime();
            _currentTime = 0.0F;
        }

        float percentageTime = _currentTime / _cicleTime;
        Color currentColor = lightCicles[_currentCicle].getColor();
        Color nextColor = lightCicles[_nextCicle].getColor();

        globalLight.color = Color.Lerp(currentColor, nextColor, percentageTime);
    }
}
