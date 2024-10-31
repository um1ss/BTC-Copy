using Newtonsoft.Json;
using System;

namespace Balancy.Models
{
#pragma warning disable 649

	public class Generator : BaseModel
	{



		[JsonProperty("iconName")]
		public readonly string IconName;

		[JsonProperty("baseIncome")]
		public readonly int BaseIncome;

		[JsonProperty("unlockCost")]
		public readonly long UnlockCost;

		[JsonProperty("id")]
		public readonly int Id;

		[JsonProperty("income")]
		public readonly float Income;

		[JsonProperty("generateTimeS")]
		public readonly int GenerateTimeS;

		[JsonProperty("background")]
		public readonly Models.BackgroundType Background;

		[JsonProperty("name")]
		public readonly string Name;

		[JsonProperty("lvls")]
		public readonly int Lvls;

		[JsonProperty("updateCosts")]
		public readonly float UpdateCosts;

	}
#pragma warning restore 649
}