using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.interfaces;

using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using api.Dtos.Comment;
using Microsoft.AspNetCore.Identity;
using api.Extensions;
using api.helpers;
using Microsoft.AspNetCore.Authorization;
namespace api.Controllers



{
    [Route("api/comment")]
    [ApiController]
    public class CommentController :ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;

        private readonly UserManager<AppUser>_userManager;

        private readonly IFMPService _fmpService;
        public CommentController(ICommentRepository commentRepo,IStockRepository stockRepo,UserManager<AppUser>userManager,IFMPService fmpService)
        {
            _commentRepo= commentRepo ?? throw new ArgumentNullException(nameof(commentRepo));
            _stockRepo= stockRepo ?? throw new ArgumentNullException(nameof(stockRepo));
            _userManager=userManager?? throw new ArgumentNullException(nameof(userManager));
            _fmpService=fmpService?? throw new ArgumentNullException(nameof(userManager));
        }
        [HttpGet]
        [Authorize]


        public async Task<IActionResult> GetAll([FromQuery]CommentQueryObject queryObject)

        {

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            
            } 

            var comments=await _commentRepo.GetAllAsync(queryObject);
           
            var commentDto =comments.Select(s=>s.ToCommentDto());

            return Ok(commentDto);

        }
        [HttpGet("{id:int}")]

        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            
            } 
            var comment = await _commentRepo.GetByIdAsync(id);

            if (comment==null){

                return NotFound();
            }

            return Ok(comment.ToCommentDto());



        }
        [HttpPost("{symbol}")]
        public async Task<IActionResult>Create([FromRoute] string symbol,CreateCommentDto commentDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            
            } 
       var stock=await _stockRepo.GetBySymbolAsync( symbol);
       if(stock==null)
       {
        stock=await _fmpService.FindStockBySymbolAsync(symbol);
        if(stock==null)
        {
            return BadRequest("Stock does not exist");
        }
        else
        {
                await _stockRepo.CreateAsync(stock);
        }
       }

        //comment userid one to one relation
        var username=User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);





        var  commentModel = commentDto.ToCommentFromCreate(stock.Id);
        commentModel.AppUserId=appUser.Id;
        await _commentRepo.CreateAsync(commentModel);
        return CreatedAtAction(nameof(GetById),new{id=commentModel.Id},commentModel.ToCommentDto());

        }

        //
        [HttpPut]
        [Route ("{id:int}")]

        public async Task<IActionResult>Update([FromRoute] int id,[FromBody]UpdateCommentRequestDto updateDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            
            } 
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
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            
            } 
            var commentModel =await _commentRepo.DeleteAsync(id);

            if (commentModel==null)
            {return NotFound("Comment does not exist");}

            return Ok(commentModel);

        }


    }
}