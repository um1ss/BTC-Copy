using System;
using System.Collections.Generic;
using Balancy.Data;
using Balancy.Localization;
using Balancy.Models;
using Project.Missions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class MissionPopupView : AbstractWindowView
{
    [SerializeField] private TMP_Text header;
    [SerializeField] private Button closeWindowButton;
    [SerializeField] private MissionTile missionTile;
    [SerializeField] private Transform tilesContainer;

    [Inject] private IAssetProvider _assetProviderService;

    public List<MissionTile> tiles;

    public override void Initialize() {
        _buttons = new() {
            {AppConstants.CloseWindowButtonName, closeWindowButton},
        };
        header.text = Manager.Get("Missions");
    }

    public MissionTile CreateTile(int id, PlayerMissionInfo mission, Action<int> onCollectBtnClick, MissionTexts texts) {
        var tile = Instantiate(missionTile, tilesContainer);
        tile.Init(id, mission.MissionStage, (MissionState) mission.MissionState, onCollectBtnClick, texts);
        return tile;
    }

    public void UpdateTile(int id, PlayerMissionInfo mission) {
        var tile = tiles.Find(tile => tile.MissionId == id);
        if (tile != null) {
            tile.UpdateTile(mission.MissionStage, (MissionState) mission.MissionState);
        }
    }

    public void DestroyPrefabTile() {
        Destroy(missionTile.gameObject);
    }
}