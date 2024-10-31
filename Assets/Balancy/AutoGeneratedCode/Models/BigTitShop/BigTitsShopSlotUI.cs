using Newtonsoft.Json;
using System;

namespace Balancy.Models.BigTitShop
{
#pragma warning disable 649

	public class BigTitsShopSlotUI : BalancyShop.UIStoreItem
	{



		[JsonProperty("coinIcon")]
		public readonly UnnyObject CoinIcon;

	}
#pragma warning restore 649
}