﻿using information_system.Models.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace information_system.Data
{
    public class SystemContext : IdentityDbContext<User>
    {
        public SystemContext(DbContextOptions<SystemContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        internal object Include(Func<object, object> p)
        {
            throw new NotImplementedException();
        }
        new public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }
        public DbSet<Chapter> Chapters { get; set;}
        public DbSet<Status> Statuses { get; set; }
    }
}
