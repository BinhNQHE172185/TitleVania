using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanel : MonoBehaviour
{

    public void OpenControlSetting()
    {
        gameObject.SetActive(true);
    }
    public void CloseControlSetting()
    {
        gameObject.SetActive(false);
    }
}
