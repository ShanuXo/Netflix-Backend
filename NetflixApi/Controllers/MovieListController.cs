using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetflixApi.Model;
using NetflixApi.Services;
using System.Security.Claims;

namespace NetflixApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieListController : ControllerBase
    {
        private readonly MovieListServices _services;
        public MovieListController(MovieListServices movieListServices)
        {
            _services = movieListServices;

        }
        // Add-Movie :
        [HttpPost("{userId}/add-movie")]
        [Authorize]
        public async Task<IActionResult> AddMovie(string userId, [FromBody] string newMovieId)
        {
            try
            {
                // Call the MovieListServices method to add the movie
                var added = await _services.AddMovieAsync(userId, newMovieId);

                if (added)
                {
                    return Ok($"Movie added successfully for user with ID: {userId}");
                }
                else
                {
                    return NotFound($"User not found or movie not added");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed, log, and return an error response.
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("{userId}")]
        [Authorize]
        public async Task<IActionResult> UpdateMovieNames(string userId, [FromBody] List<string> newMovieNames)
        {
            var updated = await _services.UpdateMovieNamesAsync(userId, newMovieNames);

            if (updated)
            {
                return Ok("Movie names updated successfully");
            }
            else
            {
                return NotFound("User not found");
            }
        }
        [HttpPost]
        // [Authorize]
        public async Task<ActionResult<MovieList>> GetUserMovieList(MovieListReq request)
        {
            var userId = request.UserId;
            try
            {
                var movieList = await _services.GetUserMovieListAsync(userId);

                if (movieList != null)
                {
                    return Ok(movieList);
                }
                else
                {
                    return NotFound($"Movie list not found for user with ID: {userId}");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed, log, and return an error response.
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}





//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using NetflixApi.Model;
//using NetflixApi.Services;
//using System.Security.Claims;

//namespace NetflixApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class MovieListController : ControllerBase
//    {
//        private readonly MovieListServices _services;
//        public MovieListController(MovieListServices movieListServices)
//        {
//            _services = movieListServices;

//        }

//        [HttpPost("{userId}")]
//        [Authorize]
//        public async Task<IActionResult> UpdateMovieNames(string userId, [FromBody] List<string> newMovieNames)
//        {
//            var updated = await _services.UpdateMovieNamesAsync(userId, newMovieNames);

//            if (updated)
//            {
//                return Ok("Movie names updated successfully");
//            }
//            else
//            {
//                return NotFound("User not found");
//            }
//        }
//        [HttpPost]
//       // [Authorize]
//        public async Task<ActionResult<MovieList>> GetUserMovieList(MovieListReq request)
//        {
//            var userId = request.UserId;
//            try
//            {
//                var movieList = await _services.GetUserMovieListAsync(userId);

//                if (movieList != null)
//                {
//                    return Ok(movieList);
//                }
//                else
//                {
//                    return NotFound($"Movie list not found for user with ID: {userId}");
//                }
//            }
//            catch (Exception ex)
//            {
//                // Handle exceptions as needed, log, and return an error response.
//                return StatusCode(500, $"Internal Server Error: {ex.Message}");
//            }
//        }
//    }
//}
