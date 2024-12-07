using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetflixApi.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NetflixApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoListController : ControllerBase
    {
        private static readonly string jsonFilePath = "videos.json";
        [HttpGet]
        public IActionResult GetVideos()
        {
            try
            {
                string json = System.IO.File.ReadAllText(jsonFilePath);
                var videos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Video>>(json);
                return Ok(videos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
