using System;
using CyberEvolution.Grid;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CyberEvolution.Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        // GridController _gridController = new GridController();
        
        private void Awake()
        {
            SceneManager.LoadScene("Main");
        }
    }
}