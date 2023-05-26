using ECSBattleSimulation.Components;
using ECSBattleSimulation.Mono.UiMonoBehaviours.Data;
using ECSBattleSimulation.Systems;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace ECSBattleSimulation.Mono.UiMonoBehaviours
{
    public class GameResultUI : UiBase
    {
        [SerializeField] private TextMeshProUGUI _winnerText;
        [SerializeField] private Button _restartButton;

        private GameResultUiData GameResultData => Data as GameResultUiData;

        private void OnEnable()
        {
            _restartButton.onClick.AddListener(OnRestartButton);

        }
        
        private void OnDisable()
        {
            _restartButton.onClick.RemoveListener(OnRestartButton);
        }
        

        protected override void RefreshView()
        {
            string teamColor = GameResultData.team == Team.Friendly ? "Blue" : "Red";
            _winnerText.text = $"Team {teamColor}";
        }


        private void OnRestartButton()
        {
            GameResultData.OnRestartButtonClicked?.Invoke();
        }

        protected override void ResetView()
        {
            _winnerText.text = "";
        }
    }
}
