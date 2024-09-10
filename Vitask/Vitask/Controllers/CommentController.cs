using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Business.Abstract;
using Business.Models;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vitask.Models;

namespace Vitask.Controllers
{
	public class CommentController : Controller
	{
		private readonly ICommentService _commentService;

		private readonly UserManager<AppUser> _userService;

		private readonly ILikeService _likeService;

		public CommentController(ICommentService commentService, UserManager<AppUser> userService, ILikeService likeService)
		{
			_commentService = commentService;
			_userService = userService;
			_likeService = likeService;
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> AddComment(AddCommentModel addCommentModel)
		{

			var user = await _userService.GetUserAsync(User);

			if (user == null)
				return RedirectToAction("Index", "Login");

			Comment replyParent;

			


			Comment comment = new Comment()
			{
				Content = addCommentModel.Content,
				TaskId = addCommentModel.TaskId,
				UserId = user.Id,
				

			};
		
			if (Regex.IsMatch(addCommentModel.Content, @"@.*?#\d+#.*?"))
			{
				
				var id = int.Parse(addCommentModel.Content.Split("#")[1]);

				var text = addCommentModel.Content.Split("#")[2].Trim();

				replyParent = _commentService.GetById(id);

				if (replyParent.ParentCommentId == null)
					comment.ParentCommentId = replyParent.Id;
				else
					comment.ParentCommentId = replyParent.ParentCommentId;

				comment.Content = text;

				
			}

			

			_commentService.Insert(comment);

			return RedirectToAction("TaskDetails", "Task", new { id = addCommentModel.TaskId });
		}


		public async Task<IActionResult> LikeComment(int id,int TaskId)
		{
			int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

			if (userId == null)
				return RedirectToAction("Index", "Login");


			bool isLiked = _likeService.DeleteByCommentIdAndUserId(id, userId);

			if (!isLiked)
			{
				Like like = new Like();
				like.CommentId = id;
				like.UserId = userId;

				_likeService.Insert(like);
			}
			

			

			return RedirectToAction("TaskDetails","Task",new {id = TaskId});
		}



	}
}
