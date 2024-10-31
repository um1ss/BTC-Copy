using Newtonsoft.Json;
using System;

namespace Balancy.Data
{
#pragma warning disable 649

	public class PlayerMissionInfo : BaseData
	{

		[JsonProperty]
		private int missionId;
		[JsonProperty]
		private int missionStage;
		[JsonProperty]
		private int missionProgress;
		[JsonProperty]
		private int missionState;


		[JsonIgnore]
		public int MissionId
		{
			get => missionId;
			set {
				if (UpdateValue(ref missionId, value))
					_cache?.UpdateStorageValue(_path + "MissionId", missionId);
			}
		}

		[JsonIgnore]
		public int MissionStage
		{
			get => missionStage;
			set {
				if (UpdateValue(ref missionStage, value))
					_cache?.UpdateStorageValue(_path + "MissionStage", missionStage);
			}
		}

		[JsonIgnore]
		public int MissionProgress
		{
			get => missionProgress;
			set {
				if (UpdateValue(ref missionProgress, value))
					_cache?.UpdateStorageValue(_path + "MissionProgress", missionProgress);
			}
		}

		[JsonIgnore]
		public int MissionState
		{
			get => missionState;
			set {
				if (UpdateValue(ref missionState, value))
					_cache?.UpdateStorageValue(_path + "MissionState", missionState);
			}
		}

		protected override void InitParams() {
			base.InitParams();

		}

		public static PlayerMissionInfo Instantiate()
		{
			return Instantiate<PlayerMissionInfo>();
		}

		protected override void AddAllParamsToCache(string path, IInternalStorageCache cache)
		{
			base.AddAllParamsToCache(path, cache);
			AddCachedItem(path + "MissionId", missionId, newValue => MissionId = Utils.ToInt(newValue), cache);
			AddCachedItem(path + "MissionStage", missionStage, newValue => MissionStage = Utils.ToInt(newValue), cache);
			AddCachedItem(path + "MissionProgress", missionProgress, newValue => MissionProgress = Utils.ToInt(newValue), cache);
			AddCachedItem(path + "MissionState", missionState, newValue => MissionState = Utils.ToInt(newValue), cache);
		}
	}
#pragma warning restore 649
}