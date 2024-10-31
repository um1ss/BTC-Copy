using Newtonsoft.Json;
using System;

namespace Balancy.Models.BigTitShop
{
#pragma warning disable 649

	public class BigTitsStoreItem : SmartObjects.StoreItem
	{



		[JsonProperty("rewardIcon")]
		public readonly UnnyObject RewardIcon;

	}
#pragma warning restore 649
}