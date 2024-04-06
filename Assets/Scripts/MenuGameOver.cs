using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGameOver : MonoBehaviour
{
    public void Salir()
    {
        Debug.Log("Perdiste, sal del jeugo");
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
