#if !DISABLE_NUTAKU
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
#if UNITY_WEBGL
using System.Runtime.InteropServices;
#endif
using System.Text;
using Balancy.API;
using Balancy.API.Auth;
using Balancy.API.Payments;
using Balancy.Data.SmartObjects;
using Balancy.Models.SmartObjects;
using Newtonsoft.Json;
using Nutaku.Unity;
using UnityEngine.Events;
using Purchase = Balancy.API.Payments.Purchase;

namespace Balancy.Platforms.Nutaku
{
#region interfaces and helping structures

    internal interface INutakuProvider
    { 
        string GetUserId();
    }
    
    internal interface INutakuAPI
    {
        void Login(Constants.Platform platform, UnityAction<AuthResponseData> onResult);
        
        void MakePayment(string userId, Product product, string callbackURL, UnityAction<NutakuPaymentData> onResponse);
    }
    
    public class NutakuProviderAndroid : INutakuProvider
    {
        public string GetUserId()
        {
            return SdkPlugin.loginInfo.userId;
        }
    }
#if UNITY_WEBGL
    public class NutakuProviderBrowser : INutakuProvider
    {
	    public string GetUserId()
	    {
		    return NutakuNetwork.NutakuInstance.GetUserId();
	    }
    }
#endif
    public class NutakuPaymentData : ResponseData
    {
        private Payment paymentResult;

        public Payment PaymentResult
        {
            get { return paymentResult; }
            set { paymentResult = value; }
        }
    }

#endregion 
    
    public class Nutaku : INutakuAPI
    {
	    internal Nutaku()
        {
        }

        private LoginInfo GetLoginInfo()
        {
	        try
	        {
#if UNITY_EDITOR
		        return NativePluginStub.instance.loginInfo;
#else
				return SdkPlugin.loginInfo;
#endif
	        }
	        catch (Exception)
	        {
		        return null;
	        }
        }

        public void Login(Constants.Platform platform, UnityAction<AuthResponseData> onResult)
        {
	        var loginInfo = GetLoginInfo();
            if (loginInfo != null && loginInfo.userId != null)
                SdkPlugin.logoutAndExit();

            SdkPlugin.Initialize();

            Coroutines.WaitUntil(() =>
            {
	            loginInfo = GetLoginInfo();

	            return loginInfo != null && loginInfo.userId != null;
            }, () =>
            {
                loginInfo = GetLoginInfo();

#if !UNITY_WEBGL
                NutakuNetwork.NutakuInstance.RetrieveProfile();
#endif
                AuthNutaku(loginInfo.userId, platform, loginInfo.accessToken.token, loginInfo.accessToken.tokenSecret, (authResp) =>
                {
                    if (authResp.Success)
                    {
                        // Settings.SetNutakuOauthToken(environment, loginInfo.accessToken.token);
                        // Settings.SetNutakuOauthTokenSecret(environment, loginInfo.accessToken.tokenSecret);
                        onResult(authResp);
                    }
                    else
                    {
                        onResult(new AuthResponseData { Error = authResp.Error });
                    }
                });
            });
        }

        private void AuthNutaku(string userId, Constants.Platform platform, string oauthToken, string oauthTokenSecret, Action<AuthResponseData> doneCallback)
        {
            NutakuNetwork.NutakuInstance.CreateAuthRequest(userId, platform, oauthToken, oauthTokenSecret, doneCallback);
        }

        public void MakePayment(string userId, Product product, string callbackURL, UnityAction<NutakuPaymentData> onResponse)
        {
            var payment = new Payment
            {
                callbackUrl = callbackURL,
                finishPageUrl = "https://example.com/finish",
                message = product.Name
            };
            var item = new PaymentItem
            {
	            itemId = product.PlatformProductId,
	            itemName = product.LocalizedName?.Value ?? Localization.Manager.Get(product.Name),
	            unitPrice = UnityEngine.Mathf.RoundToInt(product.Price),
	            quantity = 1,
	            imageUrl = product.Icon,
	            description = product.LocalizedDescription?.Value ?? Localization.Manager.Get(product.Description)
            };
            payment.paymentItems.Add(item);

            RestApiHelper.PostPayment(payment, Coroutines.Instance, resp => { OnMakePayment(resp, userId, onResponse); });
        }

        private void OnMakePayment(RawResult rawResult, string userId, UnityAction<NutakuPaymentData> onResponse)
        {
	        try
	        {
		        if (rawResult.statusCode > 199 && rawResult.statusCode < 300)
		        {
			        var result = RestApi.HandleRequestCallback<Payment>(rawResult);

			        var payment = result.GetFirstEntry();

			        ConfirmPayment(userId, payment, onResponse);
		        }
		        else
		        {
			        UnnyLogger.Critical("RequestPostPayment Failure: " + rawResult.statusCode + " => " + Encoding.UTF8.GetString(rawResult.body));
			        onResponse(new NutakuPaymentData { Success = false, Error = new Error { Message = Encoding.UTF8.GetString(rawResult.body) } });
		        }
	        }
	        catch (Exception ex)
	        {
		        UnnyLogger.Critical("RequestPostPayment Failure: " + ex.Message + " => " + ex.StackTrace);
		        onResponse(new NutakuPaymentData { Success = false, Error = new Error { Message = ex.Message } });
	        }
        }

        private void OnGetPayment(RawResult rawResult, UnityAction<NutakuPaymentData> onResponse)
        {
	        try
	        {
		        if (rawResult.statusCode > 199 && rawResult.statusCode < 300)
		        {
			        var result = RestApi.HandleRequestCallback<Payment>(rawResult);

			        Payment payment = result.GetFirstEntry();

			        onResponse(new NutakuPaymentData { Success = true, Error = null, PaymentResult = payment });
		        }
		        else
		        {
			        UnnyLogger.Critical("RequestPostPayment Failure: " + rawResult.statusCode + " => " + Encoding.UTF8.GetString(rawResult.body));
			        onResponse(new NutakuPaymentData { Success = false, Error = new Error { Message = Encoding.UTF8.GetString(rawResult.body) } });
		        }
	        }
	        catch (Exception ex)
	        {
		        UnnyLogger.Critical("RequestPostPayment Failure: " + ex.Message + " => " + ex.StackTrace);
		        onResponse(new NutakuPaymentData { Success = false, Error = new Error { Message = ex.Message } });
	        }
        }

        private void OnPaymentConfirmed(string userId, Payment payment, WebViewEvent result, UnityAction<NutakuPaymentData> onResponse)
        {
	        try
	        {
		        switch (result.kind)
		        {
			        case WebViewEventKind.Succeeded:
				        RestApiHelper.GetPayment(userId, payment.paymentId, Coroutines.Instance, rawResult => OnGetPayment(rawResult, onResponse));
				        break;

			        case WebViewEventKind.Failed:
				        UnnyLogger.Critical("Error during purchase");
				        onResponse(new NutakuPaymentData { Success = false, Error = new Error { Message = "Error during purchase: " + result.message } });
				        break;

			        case WebViewEventKind.Cancelled:
				        UnnyLogger.Verbose("User cancelled the purchase");
				        onResponse(new NutakuPaymentData { Success = false, Error = new Error { Message = "User cancelled the purchase" } });
				        break;
		        }
	        }
	        catch (Exception ex)
	        {
		        onResponse(new NutakuPaymentData { Success = false, Error = new Error { Message = ex.Message } });
	        }
        }

        private void ConfirmPayment(string userId, Payment payment, UnityAction<NutakuPaymentData> onResponse)
        {
            try
            {
                SdkPlugin.OpenPaymentView(payment, (result) => OnPaymentConfirmed(userId, payment, result, onResponse));
            }
            catch (Exception ex)
            {
                UnnyLogger.Critical(ex.StackTrace);
                onResponse(new NutakuPaymentData { Success = false, Error = new Error { Message = ex.Message } });
            }
        }
    }
    
    public class NutakuNetwork : DefaultSocial
	{
#if UNITY_WEBGL
		public class UserInfo
		{
			[JsonProperty("id")]
			public string UserId;
		}
		public class TokensInfo
		{
			[JsonProperty("oauthTokenSecret")]
			public string OAuthTokenSecret;

			[JsonProperty("oauthToken")]
			public string OAuthToken;

			[JsonProperty("id")]
			public string UserId;
		}

		[DllImport("__Internal")]
		private static extern void purchase(int env, string id, string name, int price, string img, string desc, string req_id);
		[DllImport("__Internal")]
		private static extern void purchaseSP(int env, string id, string name, int price, string img, string desc, string req_id);
		[DllImport("__Internal")]
		private static extern void auth(int env, string game_id, string device_id, string token, string req_id);
		[DllImport("__Internal")]
		public static extern void getUserId(string req_id);
		[DllImport("__Internal")]
		private static extern void getTokens(string req_id);

		private string _userId;

		public string GetUserId()
		{
			if (string.IsNullOrEmpty(_userId))
			{
				var guid = Guid.NewGuid().ToString();
				_requests.Add(guid, res =>
				{
					var data = (BalancyWebGLResponseData)res;
					if (data.Success)
					{
						var obj = JsonConvert.DeserializeObject<UserInfo>(data.Data);
						_userId = obj.UserId;
					}
					else
					{
						UnnyLogger.Critical("GetUserId: cant get user id " + data.Data);
					}
				});

				getUserId(guid);
			}

			return _userId;
		}

		private Dictionary<string, UnityAction<object>> _requests = new Dictionary<string, UnityAction<object>>();
#else
		private string _userId;

		internal void RetrieveProfile()
		{
			try
			{
				RestApiHelper.GetMyProfile(Coroutines.Instance, OnGetMyProfile);
			}
			catch (Exception ex)
			{
				UnnyLogger.Critical("RetrieveProfile: error " + " => " + ex.StackTrace);
			}
		}

		private void OnGetMyProfile(RawResult rawResult)
		{
			if (rawResult.statusCode > 199 && rawResult.statusCode < 300)
			{
				var result = RestApi.HandleRequestCallback<Person>(rawResult);
				var profile = result.GetFirstEntry();
				_userId = profile.id;
			}
			else
			{
				UnnyLogger.Critical("OnGetMyProfile: error");
			}
		}

		public string GetUserId()
		{
			if (!string.IsNullOrEmpty(_userId))
				return _userId;

			return null;
		}
#endif
		
		private static NutakuNetwork _nutakuInstance;

		public static NutakuNetwork NutakuInstance
		{
			get
			{
				if (_nutakuInstance == null)
					_nutakuInstance = new NutakuNetwork();
				return _nutakuInstance;
			}
		}

		private NutakuNetwork()
		{
			_nutakuInstance = this;

			var config = Config;
			if (Config.NutakuConfig == null)
				throw new NullReferenceException("You should set nutaku config");
#if !UNITY_WEBGL
			_nutaku = new Nutaku();
#endif

			Payments.InvalidateCache();
			
			switch (config.Platform)
			{
				case Constants.Platform.NutakuAndroid:
					_provider = new NutakuProviderAndroid();
					break;
#if UNITY_WEBGL
				case Constants.Platform.NutakuPCBrowser:
				case Constants.Platform.NutakuSPBrowser:
					_provider = new NutakuProviderBrowser();
					break;
#endif
			}
#if UNITY_WEBGL
			ActionPrePurchaseProduct += NutakuPrePurchaseProduct;
			ActionPrePurchaseOffer += NutakuPrePurchaseOffer;
			ActionPrePurchaseOfferGroup += NutakuPrePurchaseOfferGroup;
			ActionPrePurchaseStoreItem += NutakuPrePurchaseStoreItem;
			ActionCompleteUnknownPurchase += NutakuCompleteUnknownPurchase;
#endif
			ActionPurchaseProduct += PurchaseNutakuProduct;
			ActionCompletePurchase += NutakuCompletePurchase;
			ActionCreateNetwork += OnCreateNetwork;

			UpdateProductsInfo += OnUpdateProductsInfo;
#if UNITY_WEBGL
			var gameObject = new UnityEngine.GameObject("BalancyWebGLObject");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			gameObject.AddComponent<BalancyWebGLObject>();
#endif
		}
#if UNITY_WEBGL
		internal class PendingPurchases
		{
			[JsonProperty("purchases")]
			internal List<string> products;

			[JsonProperty("offers")]
			internal List<PendingOfferPurchase> offers;

			[JsonProperty("group_offers")]
			internal List<PendingOfferGroupPurchase> offerGroups;

			[JsonProperty("store_items")]
			internal List<PendingStoreItemPurchase> storeItems;

			internal class PendingPurchase
			{
				[JsonProperty("id")]
				internal string productId;
			}

			internal class PendingOfferPurchase : PendingPurchase
			{
				[JsonProperty("offer")]
				internal string offer;
			}
			
			internal class PendingOfferGroupPurchase : PendingPurchase
			{
				[JsonProperty("offer")]
				internal string offer;
				
				[JsonProperty("item")]
				internal string storeItem;
			}
			
			internal class PendingStoreItemPurchase : PendingPurchase
			{
				[JsonProperty("id")]
				internal string storeItem;
			}
		}

		private PendingPurchases GetOrCreatePendingPurchases()
		{
			var str = UnityEngine.PlayerPrefs.GetString("BalancyPendingPurchases", null);
			PendingPurchases pendingItems = null;
			if (string.IsNullOrEmpty(str))
			{
				pendingItems = new PendingPurchases();
			}
			else
			{
				pendingItems = JsonConvert.DeserializeObject<PendingPurchases>(str);
			}

			if (pendingItems.products == null)
				pendingItems.products = new List<string>();
			if (pendingItems.offers == null)
				pendingItems.offers = new List<PendingPurchases.PendingOfferPurchase>();
			if (pendingItems.offerGroups == null)
				pendingItems.offerGroups = new List<PendingPurchases.PendingOfferGroupPurchase>();
			if (pendingItems.storeItems == null)
				pendingItems.storeItems = new List<PendingPurchases.PendingStoreItemPurchase>();

			SetPendingPurchases(pendingItems);

			return pendingItems;
		}

		private void SetPendingPurchases(PendingPurchases pendings)
		{
			var str = JsonConvert.SerializeObject(pendings);
			UnityEngine.PlayerPrefs.SetString("BalancyPendingPurchases", str);
			UnityEngine.PlayerPrefs.Save();
		}

		private void NutakuCompleteUnknownPurchase(Product product, Purchase purchase, Action<PurchaseProductResponseData, Payments.CompletedPurchaseType, OfferInfo, OfferGroupInfo, StoreItem> doneCallback)
		{

			var pendingItems = GetOrCreatePendingPurchases();

			if (pendingItems.offers != null)
				for (int i = 0; i < pendingItems.offers.Count; i++)
				{
					var index = i;
					if (!string.Equals(pendingItems.offers[i].productId, product.ProductId)) continue;
					var info = LiveOps.GameOffers.GetActiveOffers()?.ToList().FirstOrDefault(o => string.Equals(o.GameOfferUnnyId, pendingItems.offers[i].offer));
					if (info != null)
					{
						NutakuCompleteInternalPurchase(purchase.OrderId, data =>
						{
							pendingItems.offers.RemoveAt(index);

							SetPendingPurchases(pendingItems);

							doneCallback(GetCompleteResponse(product, data), Payments.CompletedPurchaseType.Offer, info, null, null);
						});
						
						return;
					}
				}

			if (pendingItems.offerGroups != null)
				for (int i = 0; i < pendingItems.offerGroups.Count; i++)
				{
					var index = i;
					if (!string.Equals(pendingItems.offerGroups[i].productId, product.ProductId)) continue;
					var gInfo = LiveOps.GameOffers.GetActiveOfferGroups()?.ToList().FirstOrDefault(o => string.Equals(o.GameOfferGroupUnnyId, pendingItems.offerGroups[i].offer));
					if (gInfo != null)
					{
						NutakuCompleteInternalPurchase(purchase.OrderId, data =>
						{
							var si = pendingItems.offerGroups[i].storeItem;
							pendingItems.offerGroups.RemoveAt(index);

							SetPendingPurchases(pendingItems);

							doneCallback(GetCompleteResponse(product, data), Payments.CompletedPurchaseType.OfferGroup, null, gInfo, DataEditor.GetModelByUnnyId<StoreItem>(si));
						});
						
						return;
					}
				}

			if (pendingItems.storeItems != null)
				for (int i = 0; i < pendingItems.storeItems.Count; i++)
				{
					var index = i;
					if (!string.Equals(pendingItems.storeItems[i].productId, product.ProductId)) continue;
					NutakuCompleteInternalPurchase(purchase.OrderId, data =>
					{
						var si = pendingItems.storeItems[i].storeItem;
						pendingItems.storeItems.RemoveAt(index);

						SetPendingPurchases(pendingItems);

						doneCallback(GetCompleteResponse(product, data), Payments.CompletedPurchaseType.StoreItem, null, null, DataEditor.GetModelByUnnyId<StoreItem>(si));
					});

					return;
				}

			if (pendingItems.products != null)
				for (int i = 0; i < pendingItems.products.Count; i++)
				{
					var index = i;
					if (!string.Equals(pendingItems.products[i], product.ProductId)) continue;

					NutakuCompleteInternalPurchase(purchase.OrderId, data =>
					{
						pendingItems.products.RemoveAt(index);

						SetPendingPurchases(pendingItems);

						doneCallback(GetCompleteResponse(product, data), Payments.CompletedPurchaseType.Product, null, null, null);
					});

					return;
				}

			var newErrorResponse = new PurchaseProductResponseData
			{
				Success = false,
				Error = new Error { Message = "No pending purchase", Code = Errors.NoPurchaseItem },
			};

			doneCallback?.Invoke(newErrorResponse, Payments.CompletedPurchaseType.None, null, null, null);
		}

		private void NutakuPrePurchaseProduct(Product product)
		{
			var pendingItems = GetOrCreatePendingPurchases();
			pendingItems.products.Add(product.ProductId);
			SetPendingPurchases(pendingItems);
		}

		private void NutakuPrePurchaseOffer(Product product, OfferInfo offerInfo)
		{
			var pendingItems = GetOrCreatePendingPurchases();
			pendingItems.offers.Add(new PendingPurchases.PendingOfferPurchase { offer = offerInfo.GameOfferUnnyId, productId = product.ProductId });
			SetPendingPurchases(pendingItems);
		}

		private void NutakuPrePurchaseOfferGroup(Product product, OfferGroupInfo offerInfo, StoreItem storeItem)
		{
			var pendingItems = GetOrCreatePendingPurchases();
			pendingItems.offerGroups.Add(new PendingPurchases.PendingOfferGroupPurchase { offer = offerInfo.GameOfferGroupUnnyId, productId = product.ProductId, storeItem = storeItem.UnnyId});
			SetPendingPurchases(pendingItems);
		}

		private void NutakuPrePurchaseStoreItem(Product product, StoreItem storeItem)
		{
			var pendingItems = GetOrCreatePendingPurchases();
			pendingItems.storeItems.Add(new PendingPurchases.PendingStoreItemPurchase { productId = product.ProductId, storeItem = storeItem.UnnyId });
			SetPendingPurchases(pendingItems);
		}
#endif
		private PurchaseProductResponseData GetCompleteResponse(Product product, CompletePurchaseResponseData data)
		{
			PurchaseProductResponseData successResponse;
			if (data.Success)
			{
				successResponse = ResponseData.GetSuccessResponse<PurchaseProductResponseData>();
			}
			else
			{
				successResponse = ResponseData.GetErrorResponse<PurchaseProductResponseData>(data.Error.Code, data.Error.Message);
			}

			successResponse.Data = new PurchaseData
			{
				Id = product.PlatformProductId,
				BaseProductId = product.ProductId,
				Items = data.Data?.Items ?? new List<string> { product.ProductId },
				PriceBalancy = product.Price,
				PriceUSD = product.Price,
				Time = data.Data?.Time ?? 0
			};

			return successResponse;
		}

		private void OnUpdateProductsInfo(Product[] products, Action<bool> callback)
		{
			foreach (var product in products)
			{
				product.Meta = new ProductMetaData
				{
					LocalizedPriceString = product.Price.ToString(CultureInfo.InvariantCulture),
					LocalizedTitle = product.LocalizedName?.Value ?? Localization.Manager.Get(product.Name),
					LocalizedDescription = product.LocalizedDescription?.Value ?? Localization.Manager.Get(product.Description),
					IsoCurrencyCode = "",
					LocalizedPrice = (decimal)product.Price
				};
			}

			callback?.Invoke(true);
		}

		private DefaultSocial OnCreateNetwork(Constants.Platform arg)
		{
			return _nutakuInstance;
		}

#if !UNITY_WEBGL
		private string CallbackURL
		{
			get
			{
				switch (Config.Environment)
				{
					case Constants.Environment.Development:
						return Constants.NutakuConstants.PaymentCallbackDev;
					case Constants.Environment.Stage:
						return Constants.NutakuConstants.PaymentCallbackStage;
					case Constants.Environment.Production:
						return Constants.NutakuConstants.PaymentCallbackProd;
				}
				
				return Constants.NutakuConstants.PaymentCallbackDev;
			}
		}

		private INutakuAPI _nutaku;
#endif		
		private INutakuProvider _provider;

		private void CompleteInternalPurchase(Product product, string paymentId, Action<PurchaseProductResponseData> callback)
		{
			NutakuCompleteInternalPurchase(paymentId, completeResult =>
			{
				if (completeResult.Success)
				{
					var successResponse = ResponseData.GetSuccessResponse<PurchaseProductResponseData>();
					successResponse.Data = new PurchaseData
					{
						Id = product.PlatformProductId,
						BaseProductId = product.ProductId,
						Items = completeResult.Data.Items,
						PriceBalancy = product.Price,
						PriceUSD = product.Price,
						Time = completeResult.Data.Time
					};
					callback(successResponse);
				}
				else
				{
					callback(ResponseData.GetErrorResponse<PurchaseProductResponseData>(completeResult));
				}
			});
		}

		private void PurchaseNutakuProduct(Product product, Action<PurchaseProductResponseData> callback)
		{
#if UNITY_WEBGL
			var guid = Guid.NewGuid().ToString();
			_requests.Add(guid, res =>
			{
				var data = (BalancyWebGLResponseData)res;

				if (data.Success)
				{
					var pData = JsonConvert.DeserializeObject<BalancyWebGLPayment>(data.Data);
					CompleteInternalPurchase(product, pData.PaymentId, callback);
				}
				else
				{
					UnnyLogger.Critical("PurchaseNutakuProduct: error for " + product.ProductId + " => " + data.Data);
					callback(ResponseData.GetErrorResponse<PurchaseProductResponseData>(0, data.Data));
				}

				// NutakuCompleteInternalPurchase(((BalancyWebGLPayment)res).PaymentId, completeResult =>
				// {
				// 	if (completeResult.Success)
				// 	{
				// 		var successResponse = ResponseData.GetSuccessResponse<PurchaseProductResponseData>();
				// 		successResponse.Data = new PurchaseData
				// 		{
				// 			Id = product.PlatformProductId,
				// 			Items = new List<string> { product.PlatformProductId },
				// 			PriceBalancy = product.Price,
				// 			PriceUSD = product.Price
				// 		};
				// 		callback(successResponse);
				// 	}
				// 	else
				// 	{
				// 		callback(ResponseData.GetErrorResponse<PurchaseProductResponseData>(completeResult));
				// 	}
				// });
			});
			if (_nutakuInstance.Config.Platform == Constants.Platform.NutakuSPBrowser)
				purchaseSP((int)Config.Environment,
					product.PlatformProductId,
					product.LocalizedName?.Value ?? Localization.Manager.Get(product.Name),
					UnityEngine.Mathf.RoundToInt(product.Price),
					product.Icon,
					product.LocalizedDescription?.Value ?? Localization.Manager.Get(product.Description),
					guid);
			else
				purchase((int)Config.Environment,
					product.PlatformProductId,
					product.LocalizedName?.Value ?? Localization.Manager.Get(product.Name),
					UnityEngine.Mathf.RoundToInt(product.Price),
					product.Icon,
					product.LocalizedDescription?.Value ?? Localization.Manager.Get(product.Description),
					guid);
#else
			_nutaku.MakePayment(_provider.GetUserId(), product, CallbackURL, response =>
			{
				if (response.Success)
				{
					CompleteInternalPurchase(product, response.PaymentResult.paymentId, callback);
					// NutakuCompleteInternalPurchase(response.PaymentResult.paymentId, completeResult =>
					// {
					// 	if (completeResult.Success)
					// 	{
					// 		var successResponse = ResponseData.GetSuccessResponse<PurchaseProductResponseData>();
					// 		successResponse.Data = new PurchaseData
					// 		{
					// 			Id = product.PlatformProductId,
					// 			Items = new List<string> { product.PlatformProductId },
					// 			PriceBalancy = product.Price,
					// 			PriceUSD = product.Price
					// 		};
					// 		callback(successResponse);
					// 	}
					// 	else
					// 	{
					// 		callback(ResponseData.GetErrorResponse<PurchaseProductResponseData>(completeResult));
					// 	}
					// });
				}
				else
				{
					callback(ResponseData.GetErrorResponse<PurchaseProductResponseData>(response));
				}
			});
#endif

		}

		private void NutakuCompleteInternalPurchase(string orderId, Action<CompletePurchaseResponseData> callback)
		{
			CreateDefaultRequest("/v2/payments/nutaku/complete",
				new Dictionary<string, object>
				{
					{ "order_id", orderId },
					{ "user_id", _provider.GetUserId() }
				}, callback);
		}
		
		private void NutakuCompletePurchase(string orderId, Action<CompletePurchaseResponseData> callback)
		{
			CreateDefaultRequest("/v2/payments/nutaku/complete",
				new Dictionary<string, object>
				{
					{ "un_order_id", orderId },
					{ "user_id", _provider.GetUserId() }
				}, callback);
		}

		internal void CreateAuthRequest(string userId, Constants.Platform platform, string oauthToken, string oauthTokenSecret, Action<AuthResponseData> doneCallback)
		{
			CreateAuthRequest("/v2/auth/nutaku", new Dictionary<string, object>
			{
				{ "user_id", userId },
				{ "platform", (int)platform },
				{ "oauth_token", oauthToken },
				{ "oauth_token_secret", oauthTokenSecret },
				{ "device_id", UnityUtils.GetUniqId() }
			}, doneCallback);
		}

		public void Login(Action<AuthResponseData> onResult)
		{
#if UNITY_WEBGL
			var guid = Guid.NewGuid().ToString();
			if (_nutakuInstance.Config.Platform == Constants.Platform.NutakuSPBrowser)
			{
				_requests.Add(guid, res =>
				{
					var data = (BalancyWebGLResponseData)res;
					if (data.Success)
					{
						var obj = JsonConvert.DeserializeObject<TokensInfo>(data.Data);

						_nutakuInstance.CreateAuthRequest(obj.UserId, _nutakuInstance.Config.Platform, obj.OAuthToken, obj.OAuthTokenSecret, (authResp) =>
						{
							if (authResp.Success)
							{
								onResult(authResp);
							}
							else
							{
								onResult(new AuthResponseData { Error = authResp.Error });
							}
						});
					}
					else
					{
						UnnyLogger.Critical("OnAuth error: " + data.Data);
						onResult(ResponseData.GetErrorResponse<AuthResponseData>(0, data.Data));
					}
				});

				getTokens(guid);
			}
			else
			{
				_requests.Add(guid, res =>
				{
					GetUserId();
				
					Coroutines.WaitUntil(() => !string.IsNullOrEmpty(_userId), () =>
					{
						var data = (BalancyWebGLResponseData)res;
						if (data.Success)
						{
							var resData = data.Data;
							OnAuth(resData, onResult, false);
						}
						else
						{
							UnnyLogger.Critical("OnAuth error: " + data.Data);
							onResult(ResponseData.GetErrorResponse<AuthResponseData>(0, data.Data));
						}
					});
				});
				
				var token = GetToken();
				if (token == null)
					token = "";
				auth((int)Config.Environment, Config.ApiGameId, UnityUtils.GetUniqId(), token, guid);
			}
#else
			_nutaku.Login(Config.Platform, response => { onResult?.Invoke(response); });
#endif
		}
#if UNITY_WEBGL
		internal void OnWebGLMessage(string reqId, object obj)
		{
			if (_requests.TryGetValue(reqId, out UnityAction<object> callBack))
			{
				callBack?.Invoke(obj);
				_requests.Remove(reqId);
			}
		}
#endif
	}
#if UNITY_WEBGL
	internal class BalancyWebGLObjectResponse
	{
		[JsonProperty("req_id")]
		public string RequestId;
		
		[JsonProperty("response")]
		public BalancyWebGLResponseData Response;
	}
	
	internal class BalancyWebGLResponseData
	{
		[JsonProperty("success")]
		public bool Success;
		
		[JsonProperty("data")]
		public string Data;
	}
	
	public class BalancyWebGLPayment
	{
		[JsonProperty("payment_id")]
		public string PaymentId;
	}

	public class BalancyWebGLObject: UnityEngine.MonoBehaviour
    {
	    public void OnPurchaseConfirmed(string response)
	    {
		    var obj = JsonConvert.DeserializeObject<BalancyWebGLObjectResponse>(response);
		    NutakuNetwork.NutakuInstance.OnWebGLMessage(obj.RequestId, obj.Response);
	    }

	    public void OnAuth(string response)
	    {
		    var obj = JsonConvert.DeserializeObject<BalancyWebGLObjectResponse>(response);
		    NutakuNetwork.NutakuInstance.OnWebGLMessage(obj.RequestId, obj.Response);
	    }

	    public void OnUserId(string response)
	    {
		    var obj = JsonConvert.DeserializeObject<BalancyWebGLObjectResponse>(response);
		    NutakuNetwork.NutakuInstance.OnWebGLMessage(obj.RequestId, obj.Response);
	    }

	    public void OnTokens(string response)
	    {
		    var obj = JsonConvert.DeserializeObject<BalancyWebGLObjectResponse>(response);
		    NutakuNetwork.NutakuInstance.OnWebGLMessage(obj.RequestId, obj.Response);
	    }
    }
#endif
}

#endif