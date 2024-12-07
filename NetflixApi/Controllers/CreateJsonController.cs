using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using NetflixApi.Model;

namespace NetflixApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateJsonController : ControllerBase
    {
        private static readonly string jsonFilePath = "videos.json";

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var videos = new List<Video>
                {
                    new Video { Id = ObjectId.GenerateNewId(), Title = "Sample Video 1", VideoPath = "C:\\Users\\shanu.kumar3\\Desktop\\Videos\\WISH.mkv" },
                    new Video { Id = ObjectId.GenerateNewId(), Title = "Sample Video 2", VideoPath = "C:\\Users\\shanu.kumar3\\Desktop\\Videos\\video_2.mp4" }
                };
                //var videos = new List<Video>
                //{
                //    new Video { MovieId = 1, Title = "Sample Video 1", VideoPath = "C:\\Users\\shanu.kumar3\\Desktop\\Videos\\WISH.mkv" },
                //    new Video { MovieId = 2, Title = "Sample Video 2", VideoPath = "C:\\Users\\shanu.kumar3\\Desktop\\Videos\\video_2.mp4" }
                //};

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(videos);
                System.IO.File.WriteAllText(jsonFilePath, json);

                return Ok("JSON file created successfully.");
            }
            catch (Exception ex)
            {
                // Return an internal server error response
                return StatusCode(500, ex.Message);
            }
        }

        }
}
