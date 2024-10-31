using Newtonsoft.Json;
using System;

namespace Balancy.Models
{
#pragma warning disable 649

	public class IdBalancyConstants : SmartObjects.ConditionalTemplate
	{

		[JsonProperty]
		private string unnyIdLIKE;
		[JsonProperty]
		private string unnyIdCHIP;
		[JsonProperty]
		private string unnyIdAngela;
		[JsonProperty]
		private string unnyIdCONDOM;
		[JsonProperty]
		private string unnyIdENERGY;
		[JsonProperty]
		private string unnyIdBonny;
		[JsonProperty]
		private string unnyIdJhinny;
		[JsonProperty]
		private string unnyIdAnna;
		[JsonProperty]
		private string unnyIdMichelle;


		[JsonIgnore]
		public Models.BigTitShop.BigTitsItem LIKE => DataEditor.GetModelByUnnyId<Models.BigTitShop.BigTitsItem>(unnyIdLIKE);

		[JsonIgnore]
		public Models.BigTitShop.BigTitsItem CHIP => DataEditor.GetModelByUnnyId<Models.BigTitShop.BigTitsItem>(unnyIdCHIP);

		[JsonIgnore]
		public Models.BigTitShop.BigTitsItem Angela => DataEditor.GetModelByUnnyId<Models.BigTitShop.BigTitsItem>(unnyIdAngela);

		[JsonIgnore]
		public Models.BigTitShop.BigTitsItem CONDOM => DataEditor.GetModelByUnnyId<Models.BigTitShop.BigTitsItem>(unnyIdCONDOM);

		[JsonIgnore]
		public Models.BigTitShop.BigTitsItem ENERGY => DataEditor.GetModelByUnnyId<Models.BigTitShop.BigTitsItem>(unnyIdENERGY);

		[JsonIgnore]
		public Models.BigTitShop.BigTitsItem Bonny => DataEditor.GetModelByUnnyId<Models.BigTitShop.BigTitsItem>(unnyIdBonny);

		[JsonIgnore]
		public Models.BigTitShop.BigTitsItem Jhinny => DataEditor.GetModelByUnnyId<Models.BigTitShop.BigTitsItem>(unnyIdJhinny);

		[JsonIgnore]
		public Models.BigTitShop.BigTitsItem Anna => DataEditor.GetModelByUnnyId<Models.BigTitShop.BigTitsItem>(unnyIdAnna);

		[JsonIgnore]
		public Models.BigTitShop.BigTitsItem Michelle => DataEditor.GetModelByUnnyId<Models.BigTitShop.BigTitsItem>(unnyIdMichelle);

	}
#pragma warning restore 649
}