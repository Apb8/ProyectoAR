﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UnityEngine.XR.ARFoundation.Samples
{
    public class ARSceneSelectUI : MonoBehaviour
    {
        const string k_DefaultTitleLabel = "AR Foundation Samples";
        static readonly Color s_DisabledButtonColor = new(228f / 255, 228f / 255, 228f / 255, .5f);

        static GameObject s_SelectedMenu;
        static MenuInfo s_SelectedMenuInfo;

        [SerializeField]
        TextMeshProUGUI m_TitleLabel;

        [SerializeField]
        GameObject m_BackButton;

        public Score_logic score;
        void Awake()
        {
            if (s_SelectedMenuInfo == null)
            {
                s_SelectedMenu = gameObject;
                s_SelectedMenuInfo = new MenuInfo(name, null);
            }
            else
            {
                s_SelectedMenu = transform.parent.Find(s_SelectedMenuInfo.gameObjectName).gameObject;
                if (s_SelectedMenuInfo.menuName != null)
                    SetTitleLabelMenuName(s_SelectedMenuInfo.menuName);

                gameObject.SetActive(false);
            }
            score = FindObjectOfType<Score_logic>();
            SelectMenu(s_SelectedMenu.gameObject);
        }

        void OnDestroy()
        {
            s_SelectedMenu = null;
        }

        public void SelectMenu(GameObject menu)
        {
            if (menu == null)
            {
                Debug.LogWarning($"{nameof(ARSceneSelectUI)}.{nameof(SelectMenu)}: {nameof(menu)} was null.");
                return;
            }

            s_SelectedMenu.SetActive(false);
            s_SelectedMenu = menu;
            s_SelectedMenu.SetActive(true);
            m_BackButton.SetActive(s_SelectedMenu.gameObject != gameObject);
            s_SelectedMenuInfo.gameObjectName = menu.name;
        }

        public void ResetTitleLabel()
        {
            m_TitleLabel.text = k_DefaultTitleLabel;
            s_SelectedMenuInfo.menuName = null;
        }

        public void SetTitleLabelMenuName(string menuName)
        {
            m_TitleLabel.text = $"Samples / {menuName}";
            s_SelectedMenuInfo.menuName = menuName;
        }

        public void LoadScene(string sceneName)
        {
            if(SceneManager.GetActiveScene().buildIndex == 2)
            {
                PlayerPrefs.SetInt("PlayerScore", score.score);
                PlayerPrefs.SetInt("FrostScore", score.JackoLantern);
                PlayerPrefs.SetInt("LanternScore", score.JackFrostID);
                PlayerPrefs.Save();
            } 
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        public static void DisableButton(Button button)
        {
            if (button == null)
                return;

            button.interactable = false;
            var text = button.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
                text.color = s_DisabledButtonColor;
        }

        class MenuInfo
        {
            public string gameObjectName { get; set; }
            public string menuName { get; set; }

            public MenuInfo(string gameObjectName, string menuName)
            {
                this.gameObjectName = gameObjectName;
                this.menuName = menuName;
            }
        }
    }
}