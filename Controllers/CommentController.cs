using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.interfaces;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
// using api.Dtos.Comment;
namespace api.Controllers



{
    [Route("api/comment")]
    [ApiController]
    public class CommentController :ControllerBase
    {
        private readonly ICommentRepository _commentRepo;

        public CommentController(ICommentRepository commentRepo)
        {
            _commentRepo=commentRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments=await _commentRepo.GetAllAsync();
            return null;

        }


        
    }
}