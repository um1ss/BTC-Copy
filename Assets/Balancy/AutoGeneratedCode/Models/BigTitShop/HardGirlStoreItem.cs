using Newtonsoft.Json;
using System;

namespace Balancy.Models.BigTitShop
{
#pragma warning disable 649

	public class HardGirlStoreItem : SmartObjects.StoreItem
	{

		[JsonProperty]
		private string unnyIdGirlItem;


		[JsonProperty("blureSprite")]
		public readonly UnnyObject BlureSprite;

		[JsonIgnore]
		public Models.BigTitShop.BigTitsItem GirlItem => DataEditor.GetModelByUnnyId<Models.BigTitShop.BigTitsItem>(unnyIdGirlItem);

	}
#pragma warning restore 649
}