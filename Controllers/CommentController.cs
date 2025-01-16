using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.interfaces;

using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using api.Dtos.Comment;
namespace api.Controllers



{
    [Route("api/comment")]
    [ApiController]
    public class CommentController :ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        public CommentController(ICommentRepository commentRepo,IStockRepository stockRepo)
        {
            _commentRepo= commentRepo ?? throw new ArgumentNullException(nameof(commentRepo));
            _stockRepo= stockRepo ?? throw new ArgumentNullException(nameof(stockRepo));
        }
        [HttpGet]


        public async Task<IActionResult> GetAll()

        {

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            
            } 

            var comments=await _commentRepo.GetAllAsync();
           
            var commentDto =comments.Select(s=>s.ToCommentDto());

            return Ok(commentDto);

        }
        [HttpGet("{id:int}")]

        public async Task<IActionResult> GetById([FromRoute]int id)
        {

            var comment = await _commentRepo.GetByIdAsync(id);

            if (comment==null){

                return NotFound();
            }

            return Ok(comment.ToCommentDto());



        }
        [HttpPost("{StockId:int}")]
        public async Task<IActionResult>Create([FromRoute] int StockId,CreateCommentDto commentDto)
        {
            
        if(!await _stockRepo.StockExist(StockId))
        {

            return BadRequest("Stock does not  exists");
        }

        var  commentModel = commentDto.ToCommentFromCreate(StockId);

        await _commentRepo.CreateAsync(commentModel);
        return CreatedAtAction(nameof(GetById),new{id=commentModel.Id},commentModel.ToCommentDto());

        }

        //
        [HttpPut]
        [Route ("{id:int}")]

        public async Task<IActionResult>Update([FromRoute] int id,[FromBody]UpdateCommentRequestDto updateDto)
        {

                var comment=await _commentRepo.UpdateAsync(id,updateDto.ToCommentFromUpdate());

                if(comment==null)
                {
                    return NotFound("Comment not Found");

                }

                return Ok (comment.ToCommentDto());

        }

        [HttpDelete]
        [Route("{id:int}")]

        public async Task<IActionResult>Delete([FromRoute]int id)
        {

            var commentModel =await _commentRepo.DeleteAsync(id);

            if (commentModel==null)
            {return NotFound("Comment does not exist");}

            return Ok(commentModel);

        }


    }
}