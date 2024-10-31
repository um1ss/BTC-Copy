using Newtonsoft.Json;
using System;

namespace Balancy.Data
{
#pragma warning disable 649

	public class History : BaseData
	{

		[JsonProperty, JsonConverter(typeof(SmartListConverter<string>))]
		private SmartList<string> generatorProgress;
		[JsonProperty, JsonConverter(typeof(SmartListConverter<Data.Chat>))]
		private SmartList<Data.Chat> chats;


		[JsonIgnore]
		public SmartList<string> GeneratorProgress => generatorProgress;

		[JsonIgnore]
		public SmartList<Data.Chat> Chats => chats;

		protected override void InitParams() {
			base.InitParams();

			ValidateData(ref generatorProgress);
			ValidateData(ref chats);
		}

		public static History Instantiate()
		{
			return Instantiate<History>();
		}

		protected override void AddAllParamsToCache(string path, IInternalStorageCache cache)
		{
			base.AddAllParamsToCache(path, cache);
			AddCachedItem(path + "GeneratorProgress", GeneratorProgress, null, cache);
			AddCachedItem(path + "Chats", Chats, null, cache);
		}
	}
#pragma warning restore 649
}