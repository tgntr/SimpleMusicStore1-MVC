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
    public class RecordsController : Controller
    {
        private readonly RecordService _recordService;
        private readonly string _referrerUrl;
        private readonly IMapper _mapper;




        public RecordsController(SimpleDbContext context, IMapper mapper)
        {
            _recordService = new RecordService(context);
            _mapper = mapper;
            _referrerUrl = Request.Headers["Referer"].ToString();
        }




        public async Task<IActionResult> All()
        {
            var userId = "";
            if (User != null)
            {
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            var records = (await _recordService.All(userId)).Select(_mapper.Map<RecordViewModel>).ToList();
            var allRecordsViewModel = new AllRecordsViewModel { Records = records };

            return View(allRecordsViewModel);
        }




        [HttpPost]
        public async Task<IActionResult> All(AllRecordsViewModel model)
        {
            var userId = "";
            if (User != null)
            {
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            var records = (await _recordService.All(model.Sort, userId, model.SelectedGenres)).Select(_mapper.Map<RecordViewModel>).ToList();
            model.Records = records;

            return View(model);
        }




        public async Task<IActionResult> Details(int id)
        {
            var record = await _recordService.GetRecordAsync(id);

            if (record is null)
            {
                return RedirectToAction("All");
            }

            var viewModel = _mapper.Map<RecordViewModel>(record);

            return View(viewModel);
        }



        [Authorize]
        public async Task<IActionResult> AddToWantlist(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _recordService.AddToWantlist(id, userId);

            return Redirect(_referrerUrl);
        }



        [Authorize]
        public async Task<IActionResult> RemoveFromWantList(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _recordService.RemoveFromWantlist(id, userId);

            return Redirect(_referrerUrl);
        }
    }
}