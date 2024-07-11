using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlPanel : MonoBehaviour
{
    private bool isOpenSettings = false;
    [SerializeField] private GameObject SettingMenu;

    public void OpenCloseControlSetting()
    {
        if (!isOpenSettings)
        {
            gameObject.SetActive(true);
            SettingMenu.SetActive(false);
            isOpenSettings = true;
        }
        else
        {
            gameObject.SetActive(false);
            SettingMenu.SetActive(true);
            isOpenSettings = false;
        }
    }
}
