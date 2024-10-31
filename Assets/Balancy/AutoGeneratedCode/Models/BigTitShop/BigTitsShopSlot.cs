using Newtonsoft.Json;
using System;

namespace Balancy.Models.BigTitShop
{
#pragma warning disable 649

	public class BigTitsShopSlot : LiveOps.Store.Slot
	{

		[JsonProperty]
		private string unnyIdUIData;


		[JsonIgnore]
		public Models.BigTitShop.BigTitsShopSlotUI UIData => DataEditor.GetModelByUnnyId<Models.BigTitShop.BigTitsShopSlotUI>(unnyIdUIData);

	}
#pragma warning restore 649
}