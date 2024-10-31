using Newtonsoft.Json;
using System;

namespace Balancy.Data
{
#pragma warning disable 649

	public class Chat : BaseData
	{

		[JsonProperty]
		private string iD;
		[JsonProperty, JsonConverter(typeof(SmartListConverter<string>))]
		private SmartList<string> messages;
		[JsonProperty]
		private int spend;


		[JsonIgnore]
		public string ID
		{
			get => iD;
			set {
				if (UpdateValue(ref iD, value))
					_cache?.UpdateStorageValue(_path + "ID", iD);
			}
		}

		[JsonIgnore]
		public SmartList<string> Messages => messages;

		[JsonIgnore]
		public int Spend
		{
			get => spend;
			set {
				if (UpdateValue(ref spend, value))
					_cache?.UpdateStorageValue(_path + "Spend", spend);
			}
		}

		protected override void InitParams() {
			base.InitParams();

			ValidateData(ref messages);
		}

		public static Chat Instantiate()
		{
			return Instantiate<Chat>();
		}

		protected override void AddAllParamsToCache(string path, IInternalStorageCache cache)
		{
			base.AddAllParamsToCache(path, cache);
			AddCachedItem(path + "ID", iD, newValue => ID = (string)newValue, cache);
			AddCachedItem(path + "Messages", Messages, null, cache);
			AddCachedItem(path + "Spend", spend, newValue => Spend = Utils.ToInt(newValue), cache);
		}
	}
#pragma warning restore 649
}