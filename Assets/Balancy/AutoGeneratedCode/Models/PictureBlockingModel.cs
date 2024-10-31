using Newtonsoft.Json;
using System;

namespace Balancy.Models
{
#pragma warning disable 649

	public class PictureBlockingModel : BaseModel
	{



		[JsonProperty("dialogueID")]
		public readonly string DialogueID;

		[JsonProperty("pictureID")]
		public readonly string PictureID;

	}
#pragma warning restore 649
}