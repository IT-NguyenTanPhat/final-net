﻿using Microsoft.EntityFrameworkCore;
using timviec.Models;

namespace timviec
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options) { }

        public DbSet<Category> categories { get; set; }
    }
}
