﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWayAPI.Models;

namespace MyWayAPI.TypesConfigurations
{
    public class CompanyConfiguration: IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(u => u.Id);
        }
    }
}
