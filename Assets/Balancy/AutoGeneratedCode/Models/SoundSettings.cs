using Newtonsoft.Json;
using System;

namespace Balancy.Data
{
#pragma warning disable 649

	public class SoundSettings : BaseData
	{

		[JsonProperty]
		private float soundVolume;
		[JsonProperty]
		private float musicVolume;


		[JsonIgnore]
		public float SoundVolume
		{
			get => soundVolume;
			set {
				if (UpdateValue(ref soundVolume, value))
					_cache?.UpdateStorageValue(_path + "SoundVolume", soundVolume);
			}
		}

		[JsonIgnore]
		public float MusicVolume
		{
			get => musicVolume;
			set {
				if (UpdateValue(ref musicVolume, value))
					_cache?.UpdateStorageValue(_path + "MusicVolume", musicVolume);
			}
		}

		protected override void InitParams() {
			base.InitParams();

		}

		public static SoundSettings Instantiate()
		{
			return Instantiate<SoundSettings>();
		}

		protected override void AddAllParamsToCache(string path, IInternalStorageCache cache)
		{
			base.AddAllParamsToCache(path, cache);
			AddCachedItem(path + "SoundVolume", soundVolume, newValue => SoundVolume = Utils.ToFloat(newValue), cache);
			AddCachedItem(path + "MusicVolume", musicVolume, newValue => MusicVolume = Utils.ToFloat(newValue), cache);
		}
	}
#pragma warning restore 649
}