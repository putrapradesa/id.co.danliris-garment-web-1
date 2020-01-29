﻿using Manufactures.Domain.GarmentScrapTransactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
	public class GarmentScrapSourceDto : BaseDto
	{
		public Guid Id { get; internal set; }
		public string Code { get; internal set; }
		public string Name { get; internal set; }
		public string Description { get; internal set; }

		public GarmentScrapSourceDto(GarmentScrapSource garment)
		{
			Id = garment.Identity;
			Code = garment.Code;
			Name = garment.Name;
			Description = garment.Description;
		}
	}
}
