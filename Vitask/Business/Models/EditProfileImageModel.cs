using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Business.Models
{
	public class EditProfileImageModel
	{

		public IFormFile Image {  get; set; }

	}
}
