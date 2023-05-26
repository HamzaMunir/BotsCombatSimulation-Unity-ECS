using System;
using System.Collections.Generic;
using ECSBattleSimulation.Components;
using ECSBattleSimulation.Mono.UiMonoBehaviours.Data;
using ECSBattleSimulation.Scriptable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ECSBattleSimulation.Mono.UiMonoBehaviours
{
    public class StartGameUI : UiBase
    {
        private StartGameUiData GameUiData => Data as StartGameUiData;

        [SerializeField] private GameObject _enemySideContent;
        [SerializeField] private GameObject _friendSideContent;
        [SerializeField] private TeamConfigCellView _configPrefab;
        [SerializeField] private List<TeamConfigCellView> _spawnedTeamConfigCellViews;
        [SerializeField] private List<TeamConfigCellView> _friendSpawnedTeamConfigCellViews;
        [SerializeField] private Button _startGameButton;

        private TeamSpawnData _currentEnemySelection;
        private TeamSpawnData _currentFriendlySelection;

        private void OnEnable()
        {
            _startGameButton.onClick.AddListener(OnStartGame);
        }

        private void OnDisable()
        {
            _startGameButton.onClick.AddListener(OnStartGame);
        }

        private void Start()
        {
            _spawnedTeamConfigCellViews = new List<TeamConfigCellView>();
        }
        protected override void RefreshView()
        {
            for (int i = 0; i < GameUiData.MatchConfig.EnemyTeamConfigs.Length; i++)
            {
                var cellView = Instantiate(_configPrefab, _enemySideContent.transform);
                cellView.SetData(i,OnTeamButtonClicked, Team.Enemy);
                _spawnedTeamConfigCellViews.Add(cellView);
            }
            for (int i = 0; i < GameUiData.MatchConfig.FriendlyTeamConfig.Length; i++)
            {
                var cellView = Instantiate(_configPrefab, _friendSideContent.transform);
                cellView.SetData(i,OnTeamButtonClicked, Team.Friendly);
                _friendSpawnedTeamConfigCellViews.Add(cellView);
            }
            _startGameButton.gameObject.SetActive(false);
        }

        private void OnTeamButtonClicked(int index, Team team)
        {
            
            if (team == Team.Enemy)
            {
                _currentEnemySelection = GameUiData.MatchConfig.EnemyTeamConfigs[index];
                GameUiData.OnTeamSelected?.Invoke(_currentEnemySelection, team);
            }
            else
            {
                _currentFriendlySelection = GameUiData.MatchConfig.FriendlyTeamConfig[index];
                GameUiData.OnTeamSelected?.Invoke(_currentFriendlySelection, team);
            }
            
        }

        protected override void ResetView()
        {
            _spawnedTeamConfigCellViews.Clear();
            _currentEnemySelection = null;
            _currentFriendlySelection = null;
            foreach(Transform cell in _enemySideContent.transform)
            {
                Destroy(cell.gameObject);
            }
            foreach(Transform cell in _friendSideContent.transform)
            {
                Destroy(cell.gameObject);
            }
        }

        public void EnableStartGame(bool value)
        {
            _startGameButton.gameObject.SetActive(value);
        }
        
        private void OnStartGame()
        {
            GameUiData.OnStartGame?.Invoke(_currentEnemySelection);
        }
    }
}