using Newtonsoft.Json;
using System;

namespace Balancy.Models
{
#pragma warning disable 649

	public class ChatData : BaseModel
	{



		[JsonProperty("name")]
		public readonly string Name;

		[JsonProperty("order")]
		public readonly int Order;

		[JsonProperty("available")]
		public readonly bool Available;

		[JsonProperty("userPic")]
		public readonly UnnyObject UserPic;

		[JsonProperty("photos")]
		public readonly UnnyObject[] Photos;

	}
#pragma warning restore 649
}