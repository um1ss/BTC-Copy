using Balancy;
using Balancy.Models.LiveOps.Store;
using Balancy.Models.BigTitShop;
using System.Collections.Generic;
using Balancy.Addressables;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ShopPopupViewTest : AbstractWindowView
{
    public UnityAction OnFinishInitialize;

    [SerializeField] private Button _closeWindowButton;

    [SerializeField] private ShopSectionButton _sectionButtonPrefab;
    [SerializeField] private RectTransform _sectionButtonParent;

    [SerializeField] private GameObject _pagePrefab;
    [SerializeField] private RectTransform _pageParent;

    [SerializeField] private GameObject _defaultSlopPrefab;

    [SerializeField] private Button _fullScreenGirlObject;
    [SerializeField] private Image _girlImage;

    [SerializeField] private ScrollRect _scrollRect;

    private Dictionary<ShopSectionType, GameObject> _pages = new();
    private ShopSectionType _currentOpenPage;

    private int _pageRefreshIndex = 0;

    private List<ShopSectionButton> _shopMiniButtons = new();

    private void Start()
    {
        _fullScreenGirlObject.onClick.AddListener(() => 
        {
            OnClickButton?.Invoke();
            _fullScreenGirlObject.gameObject.SetActive(false);
        });
        _fullScreenGirlObject.gameObject.SetActive(false);

        BalancyShopSmartObjectsEvents.onSmartObjectsInitializedEvent += () =>
        {
            foreach (var item in _pages)
            {
                Destroy(item.Value);
            }
            foreach (var item in _shopMiniButtons)
            {
                Destroy(item.gameObject);
            }
            Initialize();
        };
    }
    public override void Initialize()
    {
        _buttons = new()
        {
            { AppConstants.CloseWindowButtonName, _closeWindowButton }
        };

        CreateSectionButtons();
        CreateSections();
    }
    private void CreateSections()
    {
        var smartConfig = LiveOps.Store.DefaultStore;
        var pages = smartConfig.ActivePages;

        foreach ( var page in pages )
        {
            var pageIndex = ++_pageRefreshIndex;

            var pageObject = Instantiate(_pagePrefab, _pageParent);
            pageObject.gameObject.name = page.Name.Value + " Page";

            if (page is BigTitsShopPage titsPage)
            {
                _pages.Add(titsPage.Type, pageObject);
                PreloadPrefabs(titsPage, pageObject, pageIndex);
            }
        }
        OpenAnotherSection(ShopSectionType.Chip);
    }
    private void CreateSectionButtons()
    {
        _shopMiniButtons.Clear();
        var sectionButtons = DataEditor.BigTitShop.BigTitsShopSectionButtons;
        foreach (var buttonInfo in sectionButtons)
        {
            var button = Instantiate(_sectionButtonPrefab, _sectionButtonParent);
            var sectionButton = button.GetComponent<ShopSectionButton>();
            if (sectionButton != null)
            {
                sectionButton.Init(buttonInfo, OpenAnotherSection);
                _shopMiniButtons.Add(button);
            }
        }
    }
    private void PreloadPrefabs(BigTitsShopPage page, GameObject pageObject, int pageIndex)
    {
        void CreateNewSlot(Slot storeSlot, GameObject prefab)
        {
            var storeItem = Instantiate(prefab, pageObject.transform);
            if (page.Type == ShopSectionType.Girl)
            {
                var girlShopSlot = storeItem.GetComponent<GirlShopSlot>();
                girlShopSlot.Init(storeSlot);
                girlShopSlot.OnOpenGirl += OpenFullScreenGirlImage;
                girlShopSlot.OnClick += () => OnClickButton?.Invoke();
            }
            else
            {
                var bigTitsSlot = storeItem.GetComponent<ShopSlot>();
                bigTitsSlot.OnClick += () => OnClickButton?.Invoke();
                bigTitsSlot.Init(storeSlot);
            }
        }

        var loadingElements = page.ActiveSlots.Count;
        foreach (var storeSlot in page.ActiveSlots)
        {
            if (storeSlot is BigTitsShopSlot myCustomSlot)
            {
                var ui = myCustomSlot.UIData;

                AssetsLoader.GetObject(ui.Asset.Name, prefab =>
                {
                    if (pageObject == null || UnityObjectUtility.IsDestroyed(pageObject))
                        return;
                    CreateNewSlot(storeSlot, prefab as GameObject);
                });
            }
            else
            {
                CreateNewSlot(storeSlot, _defaultSlopPrefab);
            }
            loadingElements--;
            if (loadingElements == 0)
            OnFinishInitialize?.Invoke();
        }
        pageObject.gameObject.SetActive(false);
    }
    public void OpenAnotherSection(ShopSectionType type)
    {
        OnClickButton?.Invoke();
        if (_currentOpenPage == type) return;

        foreach (var item in _pages)
        {
            item.Value.gameObject.SetActive(false);
        }
        if (_pages.TryGetValue(type, out var page))
        {
            _currentOpenPage = type;
            page.gameObject.SetActive(true);
            _scrollRect.content = page.GetComponent<RectTransform>();
        }
    }
    public override void DestroyView()
    {
        base.DestroyView();
        _pages.Clear();
    }
    private void OpenFullScreenGirlImage(Sprite sprite)
    {
        _fullScreenGirlObject.gameObject.SetActive(true);
        _girlImage.sprite = sprite;
        _girlImage.preserveAspect = true;
    }
}
