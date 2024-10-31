using Newtonsoft.Json;
using System;

namespace Balancy.Data
{
#pragma warning disable 649

	public class SoundSettingsData : ParentBaseData
	{

		[JsonProperty]
		private Data.SoundSettings soundSettings;


		[JsonIgnore]
		public Data.SoundSettings SoundSettings => soundSettings;

		protected override void InitParams() {
			base.InitParams();

			ValidateData(ref soundSettings);
		}

		public static SoundSettingsData Instantiate()
		{
			return Instantiate<SoundSettingsData>();
		}

		protected override void AddAllParamsToCache(string path, IInternalStorageCache cache)
		{
			base.AddAllParamsToCache(path, cache);
			AddCachedItem(path + "SoundSettings", SoundSettings, null, cache);
		}
	}
#pragma warning restore 649
}