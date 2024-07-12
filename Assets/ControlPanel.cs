using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlPanel : MonoBehaviour
{
    [SerializeField] private GameObject SettingMenu;

    public void OpenControlSetting()
    {
        gameObject.SetActive(true);
        SettingMenu.SetActive(false);
    }
    public void CloseControlSetting()
    {
        gameObject.SetActive(false);
        SettingMenu.SetActive(true);
    }
}
