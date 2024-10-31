using Newtonsoft.Json;
using System;

namespace Balancy.Models
{
#pragma warning disable 649

	public class PictureModel : BaseModel
	{



		[JsonProperty("pictureID")]
		public readonly string PictureID;

		[JsonProperty("picture")]
		public readonly UnnyObject Picture;

	}
#pragma warning restore 649
}