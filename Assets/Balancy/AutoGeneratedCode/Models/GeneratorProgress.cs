using Newtonsoft.Json;
using System;

namespace Balancy.Data
{
#pragma warning disable 649

	public class GeneratorProgress : BaseData
	{

		[JsonProperty, JsonConverter(typeof(SmartListConverter<Data.GeneratorProgressInfo>))]
		private SmartList<Data.GeneratorProgressInfo> generatorProgressList;


		[JsonIgnore]
		public SmartList<Data.GeneratorProgressInfo> GeneratorProgressList => generatorProgressList;

		protected override void InitParams() {
			base.InitParams();

			ValidateData(ref generatorProgressList);
		}

		public static GeneratorProgress Instantiate()
		{
			return Instantiate<GeneratorProgress>();
		}

		protected override void AddAllParamsToCache(string path, IInternalStorageCache cache)
		{
			base.AddAllParamsToCache(path, cache);
			AddCachedItem(path + "GeneratorProgressList", GeneratorProgressList, null, cache);
		}
	}
#pragma warning restore 649
}