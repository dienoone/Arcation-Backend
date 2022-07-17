using Arcation.API.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcation.API.Handler
{
    public interface IImageHandler
    {
        Task<string> UploadImage(IFormFile file);
    }
    public class ImageHandler : IImageHandler
    {
        private readonly IImageWriter _imageWriter;
        public ImageHandler(IImageWriter imageWriter)
        {
            _imageWriter = imageWriter;
        }

        public async Task<string> UploadImage(IFormFile file)
        {
            return await _imageWriter.UploadImage(file);
        }
    }
}
