using Newtonsoft.Json;
using System;

namespace Balancy.Models
{
#pragma warning disable 649

	public class MissionStage : BaseModel
	{



		[JsonProperty("id")]
		public readonly int Id;

		[JsonProperty("targetCount")]
		public readonly int TargetCount;

		[JsonProperty("reward")]
		public readonly Models.SmartObjects.Reward Reward;

	}
#pragma warning restore 649
}