using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

public class MissionPopupViewModel : AbstractWindowViewModel<MissionPopupView>, IWindow
{
    public event UnityAction<WindowTypes> OnAnotherWindowOpen;
    public WindowTypes Type => WindowTypes.Missions;

    [Inject] private MissionsModel model;

    public async UniTask Initialize() {
        await LoadView(AssetsConstants.MissionsWindow);
    }

    protected override void OpenAnotherWindowView(WindowTypes type) {
        OnAnotherWindowOpen?.Invoke(type);
        DestroyView();
    }

    public void Show() {
        if (!model.IsInitialized) {
            return;
        }

        if (_view == null) {
            CreateView();
        }

        model.UpdateMissionsStates(false);
        CreateTiles();
    }

    private void CreateTiles() {
        var metas = model.Metas;

        _view.tiles = new List<MissionTile>();

        for (var i = 0; i < metas.Count; i++) {
            var meta = metas[i];
            var missionData = model.GetPlayerMissionById(meta.Id);

            var tile = _view.CreateTile(i, missionData, OnCollectBtnClick, model.Texts);
            _view.tiles.Add(tile);
        }

        _view.DestroyPrefabTile();
    }

    private void UpdateTiles() {
        var metas = model.Metas;

        for (var i = 0; i < metas.Count; i++) {
            var meta = metas[i];
            var missionData = model.GetPlayerMissionById(meta.Id);
            _view.UpdateTile(i, missionData);
        }
    }

    private void OnCollectBtnClick(int id) {
        model.OnCollectBtnClick(id);
        model.UpdateMissionsStates();
        UpdateTiles();
    }

    protected override void SubscribeView() {
        _view.SubscribeButton(AppConstants.CloseWindowButtonName, () => OpenAnotherWindowView(WindowTypes.Main), true);
    }
}