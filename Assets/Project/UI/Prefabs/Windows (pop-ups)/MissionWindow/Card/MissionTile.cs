using System;
using System.Linq;
using Balancy;
using Balancy.Localization;
using Balancy.Models;
using Balancy.Models.SmartObjects;
using Project.Missions;
using Project.Tutorial;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionTile : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text descriptionText;

    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private Image rewardIcon;

    [SerializeField] private TMP_Text rewardCountText;

    [SerializeField] private Button collectBtn;

    [SerializeField] private TMP_Text collectBtnText;
    [SerializeField] private GameObject btnTarget;
    [SerializeField] private Image targetBtnIcon;
    [SerializeField] private TMP_Text targetBtnText;

    [SerializeField] private GameObject greenBg;
    [SerializeField] private GameObject grayBg;
    [SerializeField] private GameObject yellowBg;

    private MissionsMeta missionMeta;
    private MissionState missionState;
    private MissionTexts missionTexts;
    private ItemWithAmount reward;
    private Action<int> onCollectBtnClick;

    private Sprite curRewardSprite;
    private Sprite missionIconSprite;

    public int MissionId { get; private set; }

    public void Init(int id, int stage, MissionState state, Action<int> onBtnClick, MissionTexts texts) {
        MissionId = id;
        missionMeta = DataEditor.MissionsMetas[MissionId];
        missionTexts = texts;
        onCollectBtnClick = onBtnClick;

        UpdateTile(stage, state);
        UpdateSprites();
    }

    public void UpdateTile(int stage, MissionState state) {
        stage = Mathf.Min(stage, missionMeta.Stages.Length - 1);

        missionState = state;
        reward = missionMeta.Stages[stage].Reward.Items[0];

        var textData = missionTexts.Texts[missionMeta.Id];
        UpdateTexts(textData);
        UpdateBtn(textData);
    }

    private void UpdateSprites() {
        var curRewardIcon = DataEditor.BigTitShop.BigTitsItems.First(item => item.IntUnnyId == reward.Item.IntUnnyId)
            .Icon;
        curRewardIcon.LoadSprite(sprite => rewardIcon.sprite = sprite);
        missionMeta.Icon.LoadSprite(sprite => icon.sprite = sprite);
    }

    private void UpdateBtn(MissionText textData) {
        var isInProgress = missionState == MissionState.InProgress;

        collectBtn.onClick.RemoveAllListeners();
        collectBtn.onClick.AddListener(() => onCollectBtnClick(missionMeta.Id));

        if (missionMeta.Id == 1 && !gameObject.TryGetComponent<HighlightElementId>(out var _)) {
            var elementId = gameObject.AddComponent<HighlightElementId>();
            elementId.onActivate += () => onCollectBtnClick(missionMeta.Id);
            elementId.elementId = HighlightElemId.missionCollectBtn;
        }

        greenBg.SetActive(missionState == MissionState.ClaimReward);
        yellowBg.SetActive(isInProgress);
        grayBg.SetActive(missionState == MissionState.Finished);

        collectBtnText.gameObject.SetActive(false);
        btnTarget.SetActive(false);

        if (missionState == MissionState.InProgress) {
            var btnText = textData.btnText;

            if (btnText.Length > 0) {
                btnTarget.SetActive(true);
                var targetIcon = missionMeta.TargetIcon;

                if (targetIcon != null) {
                    targetIcon.LoadSprite(sprite => targetBtnIcon.sprite = sprite);
                }
                else {
                    targetBtnIcon.gameObject.SetActive(false);
                }

                targetBtnText.text = btnText;
            }
            else {
                collectBtnText.gameObject.SetActive(true);
                collectBtnText.text = Manager.Get("Collect");
            }
        }
        else {
            collectBtnText.gameObject.SetActive(true);
            collectBtnText.text = Manager.Get("Collect");
        }
    }

    private void UpdateTexts(MissionText textData) {
        descriptionText.text = textData.descriptionText;
        rewardText.text = Manager.Get("Reward");
        rewardCountText.text = reward.Count.ToString();
    }
}