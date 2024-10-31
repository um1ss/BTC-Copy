using Newtonsoft.Json;
using System;

namespace Balancy.Data
{
#pragma warning disable 649

	public class MissionsData : ParentBaseData
	{

		[JsonProperty]
		private Data.PlayerMissions playerMissions;


		[JsonIgnore]
		public Data.PlayerMissions PlayerMissions => playerMissions;

		protected override void InitParams() {
			base.InitParams();

			ValidateData(ref playerMissions);
		}

		public static MissionsData Instantiate()
		{
			return Instantiate<MissionsData>();
		}

		protected override void AddAllParamsToCache(string path, IInternalStorageCache cache)
		{
			base.AddAllParamsToCache(path, cache);
			AddCachedItem(path + "PlayerMissions", PlayerMissions, null, cache);
		}
	}
#pragma warning restore 649
}