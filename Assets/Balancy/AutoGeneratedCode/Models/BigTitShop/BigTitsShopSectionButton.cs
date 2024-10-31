using Newtonsoft.Json;
using System;

namespace Balancy.Models.BigTitShop
{
#pragma warning disable 649

	public class BigTitsShopSectionButton : BaseModel
	{



		[JsonProperty("type")]
		public readonly Models.BigTitShop.ShopSectionType Type;

		[JsonProperty("background")]
		public readonly UnnyObject Background;

		[JsonProperty("icon")]
		public readonly UnnyObject Icon;

	}
#pragma warning restore 649
}