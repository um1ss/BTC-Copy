using Newtonsoft.Json;
using System;

namespace Balancy.Data
{
#pragma warning disable 649

	public class TutorialInfo : BaseData
	{

		[JsonProperty]
		private bool isStartTutorialShown;


		[JsonIgnore]
		public bool IsStartTutorialShown
		{
			get => isStartTutorialShown;
			set {
				if (UpdateValue(ref isStartTutorialShown, value))
					_cache?.UpdateStorageValue(_path + "IsStartTutorialShown", isStartTutorialShown);
			}
		}

		protected override void InitParams() {
			base.InitParams();

		}

		public static TutorialInfo Instantiate()
		{
			return Instantiate<TutorialInfo>();
		}

		protected override void AddAllParamsToCache(string path, IInternalStorageCache cache)
		{
			base.AddAllParamsToCache(path, cache);
			AddCachedItem(path + "IsStartTutorialShown", isStartTutorialShown, newValue => IsStartTutorialShown = (bool)newValue, cache);
		}
	}
#pragma warning restore 649
}