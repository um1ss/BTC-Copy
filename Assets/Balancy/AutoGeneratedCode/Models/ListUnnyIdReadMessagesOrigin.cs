using Newtonsoft.Json;
using System;

namespace Balancy.Data
{
#pragma warning disable 649

	public class ListUnnyIdReadMessagesOrigin : BaseData
	{



		protected override void InitParams() {
			base.InitParams();

		}

		public static ListUnnyIdReadMessagesOrigin Instantiate()
		{
			return Instantiate<ListUnnyIdReadMessagesOrigin>();
		}

		protected override void AddAllParamsToCache(string path, IInternalStorageCache cache)
		{
			base.AddAllParamsToCache(path, cache);
		}
	}
#pragma warning restore 649
}