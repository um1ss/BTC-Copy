using Newtonsoft.Json;
using System;

namespace Balancy.Data
{
#pragma warning disable 649

	public class PlayerMissions : BaseData
	{

		[JsonProperty, JsonConverter(typeof(SmartListConverter<Data.PlayerMissionInfo>))]
		private SmartList<Data.PlayerMissionInfo> playerMissionsList;


		[JsonIgnore]
		public SmartList<Data.PlayerMissionInfo> PlayerMissionsList => playerMissionsList;

		protected override void InitParams() {
			base.InitParams();

			ValidateData(ref playerMissionsList);
		}

		public static PlayerMissions Instantiate()
		{
			return Instantiate<PlayerMissions>();
		}

		protected override void AddAllParamsToCache(string path, IInternalStorageCache cache)
		{
			base.AddAllParamsToCache(path, cache);
			AddCachedItem(path + "PlayerMissionsList", PlayerMissionsList, null, cache);
		}
	}
#pragma warning restore 649
}