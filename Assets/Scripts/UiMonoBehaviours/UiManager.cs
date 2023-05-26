using ECSBattleSimulation.Components;
using ECSBattleSimulation.Mono.UiMonoBehaviours.Data;
using ECSBattleSimulation.Scriptable;
using ECSBattleSimulation.Systems;
using Unity.Entities;
using UnityEngine;

namespace ECSBattleSimulation.Mono.UiMonoBehaviours
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private GameResultUI _gameResultUI;
        [SerializeField] private StartGameUI _startGameUI;
        private void OnEnable()
        {
            var gameResultSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<GameResultSystem>();
            gameResultSystem.OnGameResult += ShowGameResultUI;

        }
        
        private void OnDisable()
        {
            if (World.DefaultGameObjectInjectionWorld == null) return;
            var gameResultSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<GameResultSystem>();
            gameResultSystem.OnGameResult -= ShowGameResultUI;
        }

        void Start()
        {
            _startGameUI.SetData(GetStartGameData());
            _gameResultUI.Hide();
        }

        private void OnTeamSelected(TeamSpawnData selectedConfig, Team team)
        {
            if (team == Team.Enemy)
            {
                DataManager.Instance.CurrentSelectedEnemyConfig = selectedConfig;
            }
            else
            {
                DataManager.Instance.CurrentSelectedFriendlyConfig = selectedConfig;
            }

            if (DataManager.Instance.CurrentSelectedEnemyConfig != null &&
                DataManager.Instance.CurrentSelectedFriendlyConfig)
            {
                var readDataSystem =
                    World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<ReadInitialDataSystem>();
                readDataSystem.Enabled = true;
                _startGameUI.EnableStartGame(true);
            }
        }


        #region Callbacks
        
        private void OnGameStart(TeamSpawnData config)
        {
            _startGameUI.Hide();
            var targetLockSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<TargetLockSystem>();
            targetLockSystem.Enabled = true;
        }
        
        private void ShowGameResultUI(Team winner)
        {
            _gameResultUI.SetData(new GameResultUiData()
            {
                team = winner,
                OnRestartButtonClicked = OnGameRestartClicked
            });
        }

        private void OnGameRestartClicked()
        {
           _gameResultUI.Hide();
           _startGameUI.SetData(GetStartGameData());
           
        }

        private StartGameUiData GetStartGameData()
        {
            return new StartGameUiData()
            {
                MatchConfig = DataManager.Instance.Config,
                OnStartGame = OnGameStart,
                OnTeamSelected = OnTeamSelected
            };
        }
        #endregion
    }
}