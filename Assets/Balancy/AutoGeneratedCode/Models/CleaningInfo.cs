using Newtonsoft.Json;
using System;

namespace Balancy.Data
{
#pragma warning disable 649

	public class CleaningInfo : BaseData
	{

		[JsonProperty]
		private long lastStartDateTime;


		[JsonIgnore]
		public long LastStartDateTime
		{
			get => lastStartDateTime;
			set {
				if (UpdateValue(ref lastStartDateTime, value))
					_cache?.UpdateStorageValue(_path + "LastStartDateTime", lastStartDateTime);
			}
		}

		protected override void InitParams() {
			base.InitParams();

		}

		public static CleaningInfo Instantiate()
		{
			return Instantiate<CleaningInfo>();
		}

		protected override void AddAllParamsToCache(string path, IInternalStorageCache cache)
		{
			base.AddAllParamsToCache(path, cache);
			AddCachedItem(path + "LastStartDateTime", lastStartDateTime, newValue => LastStartDateTime = Utils.ToLong(newValue), cache);
		}
	}
#pragma warning restore 649
}