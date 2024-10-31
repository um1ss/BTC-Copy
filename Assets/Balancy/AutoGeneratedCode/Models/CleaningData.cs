using Newtonsoft.Json;
using System;

namespace Balancy.Data
{
#pragma warning disable 649

	public class CleaningData : ParentBaseData
	{

		[JsonProperty]
		private Data.CleaningInfo cleaningInfo;


		[JsonIgnore]
		public Data.CleaningInfo CleaningInfo => cleaningInfo;

		protected override void InitParams() {
			base.InitParams();

			ValidateData(ref cleaningInfo);
		}

		public static CleaningData Instantiate()
		{
			return Instantiate<CleaningData>();
		}

		protected override void AddAllParamsToCache(string path, IInternalStorageCache cache)
		{
			base.AddAllParamsToCache(path, cache);
			AddCachedItem(path + "CleaningInfo", CleaningInfo, null, cache);
		}
	}
#pragma warning restore 649
}