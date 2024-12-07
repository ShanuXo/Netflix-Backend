using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using NetflixApi.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NetflixApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoStreamController : ControllerBase
    {
        private static readonly string jsonFilePath = "videos.json";

        [HttpGet("{videoId}")]
        public IActionResult StreamVideo(string videoId)
        {
            try
            {
                if (!ObjectId.TryParse(videoId, out ObjectId objectId))
                {
                    return BadRequest("Invalid ObjectId format");
                }

                string json = System.IO.File.ReadAllText(jsonFilePath);
                var videos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Video>>(json);
                var video = videos.FirstOrDefault(v => v.Id == objectId);

                if (video == null)
                {
                    return NotFound();
                }

                var videoPath = video.VideoPath; // Relative path to the video

                if (!System.IO.File.Exists(videoPath))
                {
                    return NotFound();
                }

                // Determine the MIME type based on the file extension (e.g., video/mp4 for .mp4 files)
                string contentType = GetContentType(videoPath);

                return PhysicalFile(videoPath, contentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        string GetContentType(string path)
            {
                string contentType = "application/octet-stream"; // Default content type

                string extension = Path.GetExtension(path);

                if (!string.IsNullOrEmpty(extension))
                {
                    extension = extension.ToLower();

                    if (extension == ".mp4")
                    {
                        contentType = "video/mp4";
                    }
                    else if (extension == ".mkv")
                    {
                        contentType = "video/x-matroska";
                    }
                    // Add more file extensions and content types as needed
                }

                return contentType;
            }
        }
    }
