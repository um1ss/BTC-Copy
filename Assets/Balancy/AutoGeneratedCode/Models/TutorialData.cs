using Newtonsoft.Json;
using System;

namespace Balancy.Data
{
#pragma warning disable 649

	public class TutorialData : ParentBaseData
	{

		[JsonProperty]
		private Data.TutorialInfo tutorialInfo;


		[JsonIgnore]
		public Data.TutorialInfo TutorialInfo => tutorialInfo;

		protected override void InitParams() {
			base.InitParams();

			ValidateData(ref tutorialInfo);
		}

		public static TutorialData Instantiate()
		{
			return Instantiate<TutorialData>();
		}

		protected override void AddAllParamsToCache(string path, IInternalStorageCache cache)
		{
			base.AddAllParamsToCache(path, cache);
			AddCachedItem(path + "TutorialInfo", TutorialInfo, null, cache);
		}
	}
#pragma warning restore 649
}