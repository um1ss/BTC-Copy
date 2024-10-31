using Newtonsoft.Json;
using System;

namespace Balancy.Data
{
#pragma warning disable 649

	public class GeneratorProgressInfo : BaseData
	{

		[JsonProperty]
		private string generatorName;
		[JsonProperty]
		private int generatorLvl;


		[JsonIgnore]
		public string GeneratorName
		{
			get => generatorName;
			set {
				if (UpdateValue(ref generatorName, value))
					_cache?.UpdateStorageValue(_path + "GeneratorName", generatorName);
			}
		}

		[JsonIgnore]
		public int GeneratorLvl
		{
			get => generatorLvl;
			set {
				if (UpdateValue(ref generatorLvl, value))
					_cache?.UpdateStorageValue(_path + "GeneratorLvl", generatorLvl);
			}
		}

		protected override void InitParams() {
			base.InitParams();

		}

		public static GeneratorProgressInfo Instantiate()
		{
			return Instantiate<GeneratorProgressInfo>();
		}

		protected override void AddAllParamsToCache(string path, IInternalStorageCache cache)
		{
			base.AddAllParamsToCache(path, cache);
			AddCachedItem(path + "GeneratorName", generatorName, newValue => GeneratorName = (string)newValue, cache);
			AddCachedItem(path + "GeneratorLvl", generatorLvl, newValue => GeneratorLvl = Utils.ToInt(newValue), cache);
		}
	}
#pragma warning restore 649
}