using Newtonsoft.Json;
using System;

namespace Balancy.Models
{
#pragma warning disable 649

	public class DialogueModel : BaseModel
	{



		[JsonProperty("queueNumber")]
		public readonly int QueueNumber;

		[JsonProperty("dialogueID")]
		public readonly string DialogueID;

		[JsonProperty("girlPhoto")]
		public readonly UnnyObject GirlPhoto;

		[JsonProperty("message")]
		public readonly string Message;

		[JsonProperty("author")]
		public readonly string Author;

		[JsonProperty("branchNumber")]
		public readonly int BranchNumber;

	}
#pragma warning restore 649
}