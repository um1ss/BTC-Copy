using Newtonsoft.Json;
using System;
using Balancy.Localization;

namespace Balancy.Models
{
#pragma warning disable 649

	public class MissionsMeta : BaseModel
	{

		[JsonProperty]
		private string[] unnyIdStages;


		[JsonProperty("id")]
		public readonly int Id;

		[JsonProperty("icon")]
		public readonly UnnyObject Icon;

		[JsonProperty("targetIcon")]
		public readonly UnnyObject TargetIcon;

		[JsonIgnore]
		public Models.MissionStage[] Stages
		{
			get
			{
				if (unnyIdStages == null)
					return new Models.MissionStage[0];
				var stages = new Models.MissionStage[unnyIdStages.Length];
				for (int i = 0;i < unnyIdStages.Length;i++)
					stages[i] = DataEditor.GetModelByUnnyId<Models.MissionStage>(unnyIdStages[i]);
				return stages;
			}
		}

		[JsonProperty("missionText"), JsonConverter(typeof(LocalizedStringConverter))]
		public readonly LocalizedString MissionText;

	}
#pragma warning restore 649
}