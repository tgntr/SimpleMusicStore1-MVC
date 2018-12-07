﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleMusicStore.Data;
using SimpleMusicStore.Models;
using SimpleMusicStore.Web.Models.ViewModels;
using SimpleMusicStore.Web.Services;

namespace SimpleMusicStore.Web.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private CommentService _commentService;
        
        private string _referrerUrl;

        private string _userId;

        public CommentsController(SimpleDbContext context)
        {
            _commentService = new CommentService(context);

            _userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _referrerUrl = Request.Headers["Referer"].ToString();
        }
        
        [HttpPost]
        public IActionResult CommentRecord(RecordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Redirect(_referrerUrl);
            }

            _commentService.AddComment<Record>(model.Id, model.Comment, _userId);

            return Redirect(_referrerUrl);
        }

        [HttpPost]
        public IActionResult CommentArtist(ArtistViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Redirect(_referrerUrl);
            }

            _commentService.AddComment<Artist>(model.Id, model.Comment, _userId);

            return Redirect(_referrerUrl);
        }

        [HttpPost]
        public IActionResult CommentLabel(LabelViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Redirect(_referrerUrl);
            }

            _commentService.AddComment<Label>(model.Id, model.Comment, _userId);

            return Redirect(_referrerUrl);
        }

        public IActionResult RemoveComment(int commentId)
        {
            var isAdmin = User.IsInRole("Admin");
            _commentService.RemoveComment(commentId, _userId, isAdmin);

            return Redirect(_referrerUrl);
        }
        
    }
}