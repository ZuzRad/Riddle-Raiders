using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button backToMenuButton;

    private void Start()
    {
        backToMenuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Menu");
        });
    }
}
