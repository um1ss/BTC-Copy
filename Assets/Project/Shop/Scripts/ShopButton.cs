using System;
using Balancy;
using Balancy.API.Payments;
using Balancy.Data.SmartObjects;
using Balancy.Models.BigTitShop;
using Balancy.Models.LiveOps.Store;
using Balancy.Models.SmartObjects;
using Google.Apis.Sheets.v4.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

    public class ShopButton : MonoBehaviour
    {
        private const int REFRESH_RATE = 1;

        public UnityAction OnSuccessBuy;
        public UnityAction OnUnsuccessBuy;

        private abstract class IBuyButton
        {
            public abstract void TryToBuy(Action<bool> complete);
            public abstract StoreItem GetStoreItem();
            public virtual bool IsAvailable() => true;
            public virtual int GetSecondsLeftUntilAvailable() => 0;
            public abstract BigTitsShopSlotUI GetUIData();
            public virtual string GetHintText() => null;

            protected PaymentInfo GetPaymentInfo()
            {
                // This is the code you should use:
                // //unityProduct is what you receive from Unity payments, it's a type of UnityEngine.Purchasing.Product
                // var paymentInfo = new PaymentInfo
                // {
                //     Receipt = unityProduct.receipt,
                //     Price = (float)unityProduct.metadata.localizedPrice,
                //     Currency = unityProduct.metadata.isoCurrencyCode,
                //     ProductId = unityProduct.definition.id,
                //     OrderId = unityProduct.transactionID
                // };

                // This is just for testing purposes, because we didn't connect UnityPurchasing yet:
                var storeItem = GetStoreItem();
                return new PaymentInfo
                {
                    Currency = "USD",
                    Price = storeItem.Price.Product.Price,
                    ProductId = storeItem.Price.Product.ProductId,
                    OrderId = "<transactionId>",
                    Receipt = "<receipt>"
                };
            }
        }

        private class BuyButtonStoreSlot : IBuyButton
        {
            private readonly Slot _slot;

            public BuyButtonStoreSlot(Slot slot)
            {
                _slot = slot;
            }

            public override void TryToBuy(Action<bool> complete)
            {
                var storeItem = _slot.GetStoreItem();
                if (storeItem.IsFree() || storeItem.Price.Type == PriceType.Soft || (storeItem.IsAdsWatching() && storeItem.IsEnoughResources()))
                {
                    Balancy.LiveOps.Store.PurchaseStoreItem(storeItem,
                        response =>
                        {
                            Debug.Log("Purchase " + response.Success + " ? " + response.Error?.Message);
                            complete?.Invoke(response.Success);
                        });
                }
                else
                {
                    switch (storeItem.Price.Type)
                    {
                        case PriceType.Hard:

                            //TODO You can manage the in-app purchases by yourself, only informing Balancy about the results
                        Balancy.LiveOps.Store.PurchaseStoreItem(storeItem, response =>
                        {
                            Debug.Log("Purchase " + response.Success + " ? " + response.Error?.Message);
                            complete?.Invoke(response.Success);
                        });

                        //Balancy.LiveOps.Store.ItemWasPurchased(storeItem, GetPaymentInfo(), response =>
                        //{
                        //    Debug.Log("Purchase " + response.Success + " ? " + response.Error?.Message);
                        //    complete?.Invoke(response.Success);
                        //});

                        //Balancy.Payments.PurchaseProduct();
                        break;
                        case PriceType.Ads:
                            if (!storeItem.IsEnoughResources())
                            {
                                //TODO Show Ads here
                                LiveOps.Store.AdWasWatchedForStoreItem(storeItem);
                                complete?.Invoke(true);
                            }
                            break;
                    }
                }
            }

            public override StoreItem GetStoreItem()
            {
                return _slot.GetStoreItem();
            }

            public override bool IsAvailable()
            {
                return _slot.IsAvailable();
            }

            public override int GetSecondsLeftUntilAvailable()
            {
                return _slot.GetSecondsLeftUntilAvailable();
            }

            public override BigTitsShopSlotUI GetUIData()
            {
                return (_slot as BigTitsShopSlot)?.UIData;
            }

            public override string GetHintText()
            {
                if (!_slot.HasLimits())
                    return null;

                return $"{_slot.GetPurchasesDoneDuringTheLastCycle()}/{_slot.GetPurchasesLimitForCycle()}";
            }
        }

        private class BuyButtonOffer : IBuyButton
        {
            private readonly OfferInfo _offerInfo;
            private readonly BigTitsShopSlotUI _uiStoreItem;

            public BuyButtonOffer(OfferInfo offerInfo, BigTitsShopSlotUI uiStoreItem)
            {
                _offerInfo = offerInfo;
                _uiStoreItem = uiStoreItem;
            }

            public override void TryToBuy(Action<bool> complete)
            {
                var storeItem = _offerInfo.GameOffer.StoreItem;
                if (storeItem.IsFree() || storeItem.Price.Type == PriceType.Soft || (storeItem.IsAdsWatching() && storeItem.IsEnoughResources()))
                {
                    Balancy.LiveOps.GameOffers.PurchaseOffer(_offerInfo,
                        response =>
                        {
                            Debug.Log("Purchase " + response.Success + " ? " + response.Error?.Message);
                            complete?.Invoke(response.Success);
                        });
                }
                else
                {
                    switch (storeItem.Price.Type)
                    {
                        case PriceType.Hard:
                            //TODO You can manage the in-app purchases by yourself, only informing Balancy about the results  
                            Balancy.LiveOps.GameOffers.OfferWasPurchased(_offerInfo, GetPaymentInfo(), response =>
                            {
                                Debug.Log("Purchase " + response.Success + " ? " + response.Error?.Message);
                                complete?.Invoke(response.Success);
                            });
                            break;
                        case PriceType.Ads:
                            if (!storeItem.IsEnoughResources())
                            {
                                //TODO Show Ads here
                                LiveOps.Store.AdWasWatchedForStoreItem(storeItem);
                                complete?.Invoke(true);
                            }
                            break;
                    }
                }
            }

            public override StoreItem GetStoreItem()
            {
                return _offerInfo.GameOffer.StoreItem;
            }

            public override BigTitsShopSlotUI GetUIData()
            {
                return _uiStoreItem;
            }
        }

        [SerializeField] private Button buyButton;
        [SerializeField] private Image buyIcon;
        [SerializeField] private TMP_Text buyText;
        [SerializeField] private TMP_Text buyHintText;
        [SerializeField] private Image buyButtonBack;

        [SerializeField] private Sprite watchAdIcon;

        private IBuyButton _buyButtonLogic;

        public void Init(Slot slot)
        {
            _buyButtonLogic = new BuyButtonStoreSlot(slot);
            Refresh();
        }
        public void Init(OfferInfo offerInfo, BigTitsShopSlotUI uiStoreItem)
        {
            _buyButtonLogic = new BuyButtonOffer(offerInfo, uiStoreItem);
            Refresh();
        }
        public void SetInteractble(bool interactble)
        {
            buyButton.interactable = interactble;
        }
        private void Start()
        {
            buyButton?.onClick.AddListener(TryToBuy);
        }

        private void TryToBuy()
        {
            buyButton.interactable = false;
            _buyButtonLogic.TryToBuy((isSuccessBuy) =>
            {
                if (isSuccessBuy)
                {
                    OnSuccessBuy?.Invoke();
                } else
                {
                    OnUnsuccessBuy?.Invoke();
                }
                buyButton.interactable = true;
                Refresh();
            });
        }

        public void Refresh()
        {
            ApplyBuyButton();
            SetTimer(!_buyButtonLogic.IsAvailable());
        }

        private void OnDisable()
        {
            SetTimer(false);
        }

        private bool _subscribedForTimer;

        private void SetTimer(bool required)
        {
            if (required == _subscribedForTimer)
                return;

            if (required)
            {
                BalancyTimer.SubscribeForTimer(REFRESH_RATE, ApplyBuyButton);
            }
            else
            {
                BalancyTimer.UnsubscribeFromTimer(REFRESH_RATE, ApplyBuyButton);
            }

            _subscribedForTimer = required;
        }

        private void ApplyBuyButton()
        {
            SetButtonTextAndIcon();
            ApplyButtonState();
            SetButtonBackground();
        }

        private void SetButtonTextAndIcon()
        {
            StoreItem storeItem = _buyButtonLogic.GetStoreItem();
            buyIcon?.gameObject.SetActive(false);
            string buyTextString = "WRONG_PRICE";
            if (storeItem.IsFree())
            {
                buyTextString = "Collect";
            }
            else
            {
                switch (storeItem.Price.Type)
                {
                    case PriceType.Hard:
                        buyTextString = storeItem.Price.Product?.Price.ToString();
                        break;
                    case PriceType.Soft:
                        var firstItem = storeItem.Price.Items.Length > 0 ? storeItem.Price.Items[0] : null;
                        if (firstItem == null)
                        {
                            buyTextString = "No price";
                        }
                        else
                        {
                            buyTextString = firstItem.Count.ToString();
                            if (buyIcon != null)
                            {
                                (firstItem.Item as BigTitsItem)?.Icon.LoadSprite(sprite =>
                                {
                                    buyIcon.gameObject.SetActive(true);
                                    buyIcon.sprite = sprite;
                                });
                            }
                        }
                        break;
                    case PriceType.Ads:
                        if (buyIcon != null)
                        {
                            buyIcon.gameObject.SetActive(true);
                            buyIcon.sprite = watchAdIcon;
                        }

                        if (storeItem.IsEnoughResources())
                            buyTextString = "Collect";
                        else
                            buyTextString = $"{storeItem.GetWatchedAds()}/{storeItem.GetRequiredAdsToWatch()}";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            buyText?.SetText(buyTextString);
        }

        private void SetAdditionalHintText()
        {
            var hintText = _buyButtonLogic.GetHintText();
            if (string.IsNullOrEmpty(hintText))
                buyHintText?.gameObject.SetActive(false);
            else
            {
                buyHintText?.gameObject.SetActive(true);
                buyHintText?.SetText(hintText);
            }
        }

        private void ApplyButtonState()
        {
            if (buyButton != null)
            {
                if (_buyButtonLogic.IsAvailable())
                {
                    buyButton.interactable = true;
                    SetAdditionalHintText();
                }
                else
                {
                    buyButton.interactable = false;
                    if (buyHintText != null)
                    {
                        var seconds = _buyButtonLogic.GetSecondsLeftUntilAvailable();
                        if (seconds > 0)
                        {
                            var text = GameUtils.FormatTime(seconds);
                            buyHintText.SetText(text);
                            buyHintText.gameObject.SetActive(true);
                        }
                        else
                            SetAdditionalHintText();
                    }
                }
            }
        }

        private void SetButtonBackground()
        {
            if (buyButtonBack != null)
            {
                BigTitsShopSlotUI uiStoreItem = _buyButtonLogic.GetUIData();
                if (uiStoreItem?.Button != null)
                    uiStoreItem.Button.LoadSprite(btnSprite =>
                    {
                        if (buyButtonBack != null && !UnityObjectUtility.IsDestroyed(buyButtonBack))
                            buyButtonBack.sprite = btnSprite;
                    });
            }
        }
    }

