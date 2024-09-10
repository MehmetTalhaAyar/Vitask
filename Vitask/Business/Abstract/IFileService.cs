using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Business.Abstract
{
    public interface IFileService
    {
		Task<string> UploadImageAsync(IFormFile file);
		Task<bool> DeleteImageAsync(string filePath);
		string GetImageUrl(string fileName);

	}
}
