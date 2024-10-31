using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spine.Unity;

public class GeneratorButton : MonoBehaviour
{
    [SerializeField] private SkeletonGraphic _generatorAnimation;
    [SerializeField] private SkeletonGraphic _girlAnimation;
    [SerializeField] private TextMeshProUGUI _earnedChipsText;
    [SerializeField] private Image _backgroundSliderImage;
    [SerializeField] private Image _fillSliderImage;
    [SerializeField] private Color _deactivateColor;
    [SerializeField] private Button _collectButton;

    private IGenerator _generator;
    public Button CollectButton => _collectButton;
    public SkeletonGraphic GirlAnimation => _girlAnimation;
    private void Awake()
    {
        if (_generatorAnimation != null)
        {
            _generatorAnimation.AnimationState.ClearTrack(0);
            _generatorAnimation.color = _deactivateColor;
        }

        _girlAnimation.AnimationState.ClearTrack(0);
        _girlAnimation.gameObject.SetActive(false);
        _backgroundSliderImage.gameObject.SetActive(false);

        _collectButton.onClick.AddListener(ResetIncomeText);
    }

    public void SubscribeGenerator(IGenerator generator)
    {
        if (generator != null)
        {
            _generator = generator;
            _generator.OnGenerateTick += SetFillAmount;
            _generator.OnEndGenerate += SetChipText;
            _generator.OnActivateGenerate += Actiate;
            _generator.OnDeactivateGenerate += Deactiate;
        }
    }
    public void Unsubscribe()
    {
        _generator.OnGenerateTick -= SetFillAmount;
        _generator.OnEndGenerate -= SetChipText;
        _generator.OnActivateGenerate -= Actiate;
        _generator.OnDeactivateGenerate -= Deactiate;
    }
    public void ResetIncomeText()
    {
        _earnedChipsText.text = "0";
    }

    private void SetFillAmount(float value)
    {
        _fillSliderImage.fillAmount = value;
    }
    private void SetChipText(int value)
    {
        _earnedChipsText.text = value.ToString();
    }
    private void Actiate(string name)
    {
        if (_generatorAnimation != null)
        {
            _generatorAnimation.AnimationState.SetAnimation(0, "idle", true);
            _generatorAnimation.color = Color.white;
        }

        _girlAnimation.AnimationState.SetAnimation(0, "idle", true);
        _girlAnimation.gameObject.SetActive(true);
        _backgroundSliderImage.gameObject.SetActive(true);
    }
    private void Deactiate()
    {
        if (_generatorAnimation != null)
        {
            _generatorAnimation.AnimationState.ClearTrack(0);
            _generatorAnimation.color = _deactivateColor;
        }

        _girlAnimation.AnimationState.ClearTrack(0);
        _girlAnimation.gameObject.SetActive(false);
        _backgroundSliderImage.gameObject.SetActive(false);
    }
}