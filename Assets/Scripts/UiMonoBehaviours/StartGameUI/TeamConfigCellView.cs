using System;
using System.Collections;
using System.Collections.Generic;
using ECSBattleSimulation.Components;
using ECSBattleSimulation.Scriptable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ECSBattleSimulation.Mono.UiMonoBehaviours
{
    public class TeamConfigCellView : MonoBehaviour
    {
        private int _index;
        [SerializeField] private TextMeshProUGUI _buttonText;
        [SerializeField] private Button _button;
        private Team _team;
        private Action<int, Team> _onClickCallback;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }


        public void SetData(int index, Action<int, Team> onClicked, Team team)
        {
            _index = index;
            _onClickCallback = onClicked;
            _team = team;
            _buttonText.text = $"Team {index + 1}";
        }
        private void OnButtonClicked()
        {
            _onClickCallback?.Invoke(_index, _team);
        }
    }
}