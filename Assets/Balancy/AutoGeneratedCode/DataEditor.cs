using System;
using Balancy.Data;
using System.Collections.Generic;

namespace Balancy
{
#pragma warning disable 649

	public partial class DataEditor
	{

		private static void LoadSmartObject(string userId, string name, string key, Action<ParentBaseData> callback)
		{
			switch (name)
			{
				case "SoundSettingsData":
				{
					SmartStorage.LoadSmartObject<Data.SoundSettingsData>(userId, key, responseData =>
					{
						callback?.Invoke(responseData.Data);
					});
					break;
				}
				case "BalancyShopData":
				{
					SmartStorage.LoadSmartObject<Data.BalancyShopData>(userId, key, responseData =>
					{
						callback?.Invoke(responseData.Data);
					});
					break;
				}
				case "MissionsData":
				{
					SmartStorage.LoadSmartObject<Data.MissionsData>(userId, key, responseData =>
					{
						callback?.Invoke(responseData.Data);
					});
					break;
				}
				case "TutorialData":
				{
					SmartStorage.LoadSmartObject<Data.TutorialData>(userId, key, responseData =>
					{
						callback?.Invoke(responseData.Data);
					});
					break;
				}
				case "SmartObjects.UnnyProfile":
				{
					SmartStorage.LoadSmartObject<Data.SmartObjects.UnnyProfile>(userId, key, responseData =>
					{
						callback?.Invoke(responseData.Data);
					});
					break;
				}
				case "CleaningData":
				{
					SmartStorage.LoadSmartObject<Data.CleaningData>(userId, key, responseData =>
					{
						callback?.Invoke(responseData.Data);
					});
					break;
				}
				case "GeneratorProgressData":
				{
					SmartStorage.LoadSmartObject<Data.GeneratorProgressData>(userId, key, responseData =>
					{
						callback?.Invoke(responseData.Data);
					});
					break;
				}
				case "ChatsData":
				{
					SmartStorage.LoadSmartObject<Data.ChatsData>(userId, key, responseData =>
					{
						callback?.Invoke(responseData.Data);
					});
					break;
				}
				default:
					UnnyLogger.Critical("No SmartObject found by name " + name);
					break;
			}
		}

		static partial void MoveAllData(string userId)
		{
			MigrateSmartObject(userId, "SoundSettingsData");
			MigrateSmartObject(userId, "BalancyShopData");
			MigrateSmartObject(userId, "MissionsData");
			MigrateSmartObject(userId, "TutorialData");
			MigrateSmartObject(userId, "UnnyProfile");
			MigrateSmartObject(userId, "CleaningData");
			MigrateSmartObject(userId, "GeneratorProgressData");
			MigrateSmartObject(userId, "ChatsData");
		}

		static partial void TransferAllSmartObjectsFromLocalToCloud(string userId)
		{
			TransferSmartObjectFromLocalToCloud<Data.SoundSettingsData>(userId);
			TransferSmartObjectFromLocalToCloud<Data.BalancyShopData>(userId);
			TransferSmartObjectFromLocalToCloud<Data.MissionsData>(userId);
			TransferSmartObjectFromLocalToCloud<Data.TutorialData>(userId);
			TransferSmartObjectFromLocalToCloud<Data.SmartObjects.UnnyProfile>(userId);
			TransferSmartObjectFromLocalToCloud<Data.CleaningData>(userId);
			TransferSmartObjectFromLocalToCloud<Data.GeneratorProgressData>(userId);
			TransferSmartObjectFromLocalToCloud<Data.ChatsData>(userId);
		}

		static partial void ResetAllSmartObjects(string userId)
		{
			ResetSmartObject<Data.SoundSettingsData>(userId);
			ResetSmartObject<Data.BalancyShopData>(userId);
			ResetSmartObject<Data.MissionsData>(userId);
			ResetSmartObject<Data.TutorialData>(userId);
			ResetSmartObject<Data.SmartObjects.UnnyProfile>(userId);
			ResetSmartObject<Data.CleaningData>(userId);
			ResetSmartObject<Data.GeneratorProgressData>(userId);
			ResetSmartObject<Data.ChatsData>(userId);
		}

		static partial void PreloadAllSmartObjects(string userId, bool skipServerLoading)
		{
			SmartStorage.LoadSmartObject<Data.SoundSettingsData>(userId, null, skipServerLoading);
			SmartStorage.LoadSmartObject<Data.BalancyShopData>(userId, null, skipServerLoading);
			SmartStorage.LoadSmartObject<Data.MissionsData>(userId, null, skipServerLoading);
			SmartStorage.LoadSmartObject<Data.TutorialData>(userId, null, skipServerLoading);
			SmartStorage.LoadSmartObject<Data.CleaningData>(userId, null, skipServerLoading);
			SmartStorage.LoadSmartObject<Data.GeneratorProgressData>(userId, null, skipServerLoading);
			SmartStorage.LoadSmartObject<Data.ChatsData>(userId, null, skipServerLoading);
		}

		public static List<Models.MyGameEvent> MyGameEvents { get; private set; }
		public static List<Models.Generator> Generators { get; private set; }
		public static List<Models.MissionsMeta> MissionsMetas { get; private set; }
		public static List<Models.DialogueModel> DialogueModels { get; private set; }
		public static List<Models.MissionStage> MissionStages { get; private set; }
		public static List<Models.ChatData> ChatData { get; private set; }
		public static List<Models.DialogueOrder> DialogueOrders { get; private set; }
		public static List<Models.PictureBlockingModel> PictureBlockingModels { get; private set; }
		public static Balancy.SmartObjects.BalancySingleton<Models.IdBalancyConstants> IdBalancyConstants { get; private set; }
		public static List<Models.PictureModel> PictureModels { get; private set; }
		public static class BalancyShop
		{
			public static List<Models.BalancyShop.BadgeInfo> BadgeInfos { get; private set; }
			public static List<Models.BalancyShop.GameSection> GameSections { get; private set; }
			public static List<Models.BalancyShop.MyStoreItem> MyStoreItems { get; private set; }
			public static List<Models.BalancyShop.UIStoreItem> UIStoreItems { get; private set; }
			public static List<Models.BalancyShop.MyItem> MyItems { get; private set; }
			public static List<Models.BalancyShop.MyOffer> MyOffers { get; private set; }

			public static void Init()
			{
				BadgeInfos = DataManager.ParseList<Models.BalancyShop.BadgeInfo>();
				GameSections = DataManager.ParseList<Models.BalancyShop.GameSection>();
				MyStoreItems = DataManager.ParseList<Models.BalancyShop.MyStoreItem>();
				UIStoreItems = DataManager.ParseList<Models.BalancyShop.UIStoreItem>();
				MyItems = DataManager.ParseList<Models.BalancyShop.MyItem>();
				MyOffers = DataManager.ParseList<Models.BalancyShop.MyOffer>();
			}
		}
		public static class BigTitShop
		{
			public static List<Models.BigTitShop.HardGirlStoreItem> HardGirlStoreItems { get; private set; }
			public static List<Models.BigTitShop.BigTitsItem> BigTitsItems { get; private set; }
			public static List<Models.BigTitShop.BigTitsShopSectionButton> BigTitsShopSectionButtons { get; private set; }
			public static List<Models.BigTitShop.BigTitsShopSlotUI> BigTitsShopSlotUIS { get; private set; }
			public static List<Models.BigTitShop.BigTitsStoreItem> BigTitsStoreItems { get; private set; }

			public static void Init()
			{
				HardGirlStoreItems = DataManager.ParseList<Models.BigTitShop.HardGirlStoreItem>();
				BigTitsItems = DataManager.ParseList<Models.BigTitShop.BigTitsItem>();
				BigTitsShopSectionButtons = DataManager.ParseList<Models.BigTitShop.BigTitsShopSectionButton>();
				BigTitsShopSlotUIS = DataManager.ParseList<Models.BigTitShop.BigTitsShopSlotUI>();
				BigTitsStoreItems = DataManager.ParseList<Models.BigTitShop.BigTitsStoreItem>();
			}
		}

		static partial void PrepareGeneratedData() {
			ParseDictionary<Models.BalancyShop.Badge>();
			ParseDictionary<Models.BalancyShop.MyCustomSlot>();
			MyGameEvents = DataManager.ParseList<Models.MyGameEvent>();
			ParseDictionary<Models.ContentHolder>();
			Generators = DataManager.ParseList<Models.Generator>();
			MissionsMetas = DataManager.ParseList<Models.MissionsMeta>();
			ParseDictionary<Models.BigTitShop.BigTitsShopPage>();
			DialogueModels = DataManager.ParseList<Models.DialogueModel>();
			MissionStages = DataManager.ParseList<Models.MissionStage>();
			ChatData = DataManager.ParseList<Models.ChatData>();
			DialogueOrders = DataManager.ParseList<Models.DialogueOrder>();
			PictureBlockingModels = DataManager.ParseList<Models.PictureBlockingModel>();
			ParseDictionary<Models.BigTitShop.BigTitsShopSlot>();
			IdBalancyConstants = DataManager.ParseSingletonType<Models.IdBalancyConstants>();
			PictureModels = DataManager.ParseList<Models.PictureModel>();
			BalancyShop.Init();
			BigTitShop.Init();
			SmartStorage.SetLoadSmartObjectMethod(LoadSmartObject);
		}
	}
#pragma warning restore 649
}