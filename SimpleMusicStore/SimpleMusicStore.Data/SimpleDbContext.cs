﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleMusicStore.Models;

namespace SimpleMusicStore.Data
{
    public class SimpleDbContext : IdentityDbContext<SimpleUser>
    {
        public SimpleDbContext(DbContextOptions<SimpleDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Address> Addresses { get; set; }

        public DbSet<Record> Records { get; set; }

        public DbSet<Artist> Artists { get; set; }

        public DbSet<Label> Labels { get; set; }

        public DbSet<Track> Tracks { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<ArtistUser> ArtistUsers { get; set; }

        public DbSet<LabelUser> LabelUsers { get; set; }

        public DbSet<RecordUser> RecordUsers { get; set; }

        public DbSet<Video> Videos { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<RecordOrder> RecordOrders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }


            builder.Entity<ArtistUser>()
                .HasKey(au => new { au.ArtistId, au.UserId });

            builder.Entity<ArtistUser>()
                .HasOne(au => au.Artist)
                .WithMany(a => a.Followers)
                .HasForeignKey(au => au.ArtistId);

            builder.Entity<ArtistUser>()
                .HasOne(au => au.User)
                .WithMany(u => u.FollowedArtists)
                .HasForeignKey(au => au.UserId);

            builder.Entity<LabelUser>()
                .HasKey(lu => new { lu.LabelId, lu.UserId });

            builder.Entity<LabelUser>()
                .HasOne(lu => lu.Label)
                .WithMany(l => l.Followers)
                .HasForeignKey(lu => lu.LabelId);

            builder.Entity<LabelUser>()
                .HasOne(lu => lu.User)
                .WithMany(u => u.FollowedLabels)
                .HasForeignKey(lu => lu.UserId);

            builder.Entity<RecordUser>()
                .HasKey(ru => new { ru.RecordId, ru.UserId });

            builder.Entity<RecordUser>()
                .HasOne(ru => ru.Record)
                .WithMany(r => r.WantedBy)
                .HasForeignKey(ru => ru.RecordId);

            builder.Entity<RecordUser>()
                .HasOne(ru => ru.User)
                .WithMany(u => u.Wantlist)
                .HasForeignKey(ru => ru.UserId);

            builder.Entity<RecordOrder>()
                .HasKey(ro => new { ro.RecordId, ro.OrderId });

            builder.Entity<RecordOrder>()
                .HasOne(ro => ro.Record)
                .WithMany(r => r.Orders)
                .HasForeignKey(ro => ro.RecordId);

            builder.Entity<RecordOrder>()
                .HasOne(ro => ro.Order)
                .WithMany(u => u.Items)
                .HasForeignKey(ro => ro.OrderId);

            builder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            

        }

        
    }
}
