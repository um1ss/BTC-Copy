using Newtonsoft.Json;
using System;

namespace Balancy.Models
{
#pragma warning disable 649

	public class DialogueOrder : BaseModel
	{



		[JsonProperty("girlName")]
		public readonly string GirlName;

		[JsonProperty("order")]
		public readonly int Order;

		[JsonProperty("dialogueID")]
		public readonly string DialogueID;

	}
#pragma warning restore 649
}