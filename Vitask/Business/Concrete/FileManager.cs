using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Microsoft.AspNetCore.Http;

namespace Business.Concrete
{
	public class FileManager : IFileService
	{
		private readonly string _imageFolder = "uploads/images";

		public Task<bool> DeleteImageAsync(string filePath)
		{
			var fullPath = Path.Combine("wwwroot", _imageFolder, filePath);

			if (File.Exists(fullPath))
			{
				File.Delete(fullPath);
				return Task.FromResult(true);
			}

			return Task.FromResult(false);
		}

		public string GetImageUrl(string fileName)
		{
			return Path.Combine("wwwroot",_imageFolder, fileName).Replace("\\", "/");
		}

		public async Task<string> UploadImageAsync(IFormFile file)
		{
			if (file == null || file.Length == 0)
				throw new ArgumentException("Dosya geçersiz.");

			var uploadPath = Path.Combine("wwwroot", _imageFolder);

			if (!Directory.Exists(uploadPath))
			{
				Directory.CreateDirectory(uploadPath);
			}

			var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
			var filePath = Path.Combine( uploadPath, fileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			return fileName;
		}
	}
}
