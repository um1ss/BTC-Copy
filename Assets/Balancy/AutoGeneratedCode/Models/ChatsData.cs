using Newtonsoft.Json;
using System;

namespace Balancy.Data
{
#pragma warning disable 649

	public class ChatsData : ParentBaseData
	{

		[JsonProperty]
		private Data.History history;


		[JsonIgnore]
		public Data.History History => history;

		protected override void InitParams() {
			base.InitParams();

			ValidateData(ref history);
		}

		public static ChatsData Instantiate()
		{
			return Instantiate<ChatsData>();
		}

		protected override void AddAllParamsToCache(string path, IInternalStorageCache cache)
		{
			base.AddAllParamsToCache(path, cache);
			AddCachedItem(path + "History", History, null, cache);
		}
	}
#pragma warning restore 649
}