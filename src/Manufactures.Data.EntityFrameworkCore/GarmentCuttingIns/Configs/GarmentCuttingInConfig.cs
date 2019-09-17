﻿using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manufactures.Data.EntityFrameworkCore.GarmentCuttingIns.Configs
{
    public class GarmentCuttingInConfig : IEntityTypeConfiguration<GarmentCuttingInReadModel>
    {
        public void Configure(EntityTypeBuilder<GarmentCuttingInReadModel> builder)
        {
            builder.ToTable("GarmentCuttingIns");
            builder.HasKey(e => e.Identity);

            builder.Property(p => p.CutInNo).HasMaxLength(25);
            builder.Property(p => p.CuttingType).HasMaxLength(25);
            builder.Property(p => p.RONo).HasMaxLength(25);
            builder.Property(p => p.Article).HasMaxLength(50);
            builder.Property(p => p.UnitCode).HasMaxLength(25);
            builder.Property(p => p.UnitName).HasMaxLength(100);

            builder.ApplyAuditTrail();
            builder.ApplySoftDelete();
        }
    }
}