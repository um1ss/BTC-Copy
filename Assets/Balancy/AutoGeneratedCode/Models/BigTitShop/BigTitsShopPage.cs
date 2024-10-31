using Newtonsoft.Json;
using System;

namespace Balancy.Models.BigTitShop
{
#pragma warning disable 649

	public class BigTitsShopPage : LiveOps.Store.Page
	{



		[JsonProperty("type")]
		public readonly Models.BigTitShop.ShopSectionType Type;

	}
#pragma warning restore 649
}