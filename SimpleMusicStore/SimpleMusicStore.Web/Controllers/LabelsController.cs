﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleMusicStore.Data;
using SimpleMusicStore.Web.Models.ViewModels;
using SimpleMusicStore.Web.Services;

namespace SimpleMusicStore.Web.Controllers
{
    public class LabelsController : Controller
    {
        private readonly LabelService _labelService;
        private readonly IMapper _mapper;



        public LabelsController(SimpleDbContext context, IMapper mapper)
        {
            _labelService = new LabelService(context);
            _mapper = mapper;
        }



        public async Task<IActionResult> All(string orderBy = "")
        {
            var userId = "";
            if (User != null)
            {
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            var model = (await _labelService.All(orderBy, userId)).Select(_mapper.Map<LabelViewModel>).ToList();

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var label = await _labelService.GetLabel(id);

            if (label is null)
            {
                return RedirectToAction("All");
            }

            var model = _mapper.Map<LabelViewModel>(label);

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (label.Followers.Any(lu => lu.UserId == userId))
                {
                    model.IsFollowed = true;
                }
            }

            return View(model);
        }


        [Authorize]
        public async Task<IActionResult> FollowLabel(int labelId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _labelService.FollowLabel(labelId, userId);

            return Redirect("/labels/details?labeId=" + labelId);
        }



        [Authorize]
        public async Task<IActionResult> UnfollowLabel(int labelId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _labelService.UnfollowLabel(labelId, userId);

            return Redirect("/labels/details?labeId=" + labelId);
        }
    }
}