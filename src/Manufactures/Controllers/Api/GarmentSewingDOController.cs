﻿using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSewingDOs.Commands;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [ApiController]
    [Authorize]
    [Route("sewing-dos")]
    public class GarmentSewingDOController : ControllerApiBase
    {
        private readonly IGarmentSewingDORepository _garmentSewingDORepository;
        private readonly IGarmentSewingDOItemRepository _garmentSewingDOItemRepository;

        public GarmentSewingDOController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentSewingDORepository = Storage.GetRepository<IGarmentSewingDORepository>();
            _garmentSewingDOItemRepository = Storage.GetRepository<IGarmentSewingDOItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSewingDORepository.Read(page, size, order, keyword, filter);
            var count = query.Count();
            List<GarmentSewingDOListDto> garmentSewingDOListDtos = _garmentSewingDORepository.Find(query).Select(sewingDO =>
            {
                var items = _garmentSewingDOItemRepository.Query.Where(o => o.SewingDOId == sewingDO.Identity).Select(sewingDOItem => new
                {
                    sewingDOItem.ProductCode,
                    sewingDOItem.Quantity,
                }).ToList();

                return new GarmentSewingDOListDto(sewingDO)
                {
                    Products = items.Select(i => i.ProductCode).ToList(),
                    TotalQuantity = items.Sum(i => i.Quantity),
                };
            }).ToList();

            await Task.Yield();
            return Ok(garmentSewingDOListDtos, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            GarmentSewingDODto garmentSewingDO = _garmentSewingDORepository.Find(o => o.Identity == guid).Select(sewingDO => new GarmentSewingDODto(sewingDO)
            {
                Items = _garmentSewingDOItemRepository.Find(o => o.SewingDOId == sewingDO.Identity).Select(sewingDOItem => new GarmentSewingDOItemDto(sewingDOItem)).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentSewingDO);
        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSewingDORepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentSewingDODto = _garmentSewingDORepository.Find(query).Select(o => new GarmentSewingDODto(o)).ToArray();
            var garmentSewingDOItemDto = _garmentSewingDOItemRepository.Find(_garmentSewingDOItemRepository.Query).Select(o => new GarmentSewingDOItemDto(o)).ToList();
            
            Parallel.ForEach(garmentSewingDODto, itemDto =>
            {
                var garmentSewingDOItems = garmentSewingDOItemDto.Where(x => x.SewingDOId == itemDto.Id).OrderBy(x => x.Id).ToList();

                itemDto.Items = garmentSewingDOItems;

            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentSewingDODto = QueryHelper<GarmentSewingDODto>.Order(garmentSewingDODto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentSewingDODto, info: new
            {
                page,
                size,
                count
            });
        }
    }
}