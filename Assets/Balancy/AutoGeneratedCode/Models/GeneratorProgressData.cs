using Newtonsoft.Json;
using System;

namespace Balancy.Data
{
#pragma warning disable 649

	public class GeneratorProgressData : ParentBaseData
	{

		[JsonProperty]
		private Data.GeneratorProgress generatorProgress;


		[JsonIgnore]
		public Data.GeneratorProgress GeneratorProgress => generatorProgress;

		protected override void InitParams() {
			base.InitParams();

			ValidateData(ref generatorProgress);
		}

		public static GeneratorProgressData Instantiate()
		{
			return Instantiate<GeneratorProgressData>();
		}

		protected override void AddAllParamsToCache(string path, IInternalStorageCache cache)
		{
			base.AddAllParamsToCache(path, cache);
			AddCachedItem(path + "GeneratorProgress", GeneratorProgress, null, cache);
		}
	}
#pragma warning restore 649
}