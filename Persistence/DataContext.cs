using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityAttendee> ActivityAttendees {get; set;}
        public DbSet<Photo> Photos {get; set;}
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserFollowing> UserFollowings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ActivityAttendee>(x => x.HasKey(aa => new {aa.AppUserID, aa.ActivityID}));

            builder.Entity<ActivityAttendee>()
            .HasOne(u => u.AppUser)
            .WithMany(a => a.Activities)
            .HasForeignKey(aa => aa.AppUserID);

            builder.Entity<ActivityAttendee>()
            .HasOne(u => u.Activity)
            .WithMany(a => a.Attendees)
            .HasForeignKey(aa => aa.ActivityID);

            builder.Entity<Comment>()
            .HasOne(a => a.Activity)
            .WithMany(c => c.Comments)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserFollowing>(b => 
            {
                b.HasKey(k => new {k.ObserverID, k.TargetID});
                
                b.HasOne(o => o.Observer)
                    .WithMany(f => f.Followings)
                    .HasForeignKey(o => o.ObserverID)
                    .OnDelete(DeleteBehavior.Cascade);
                
                b.HasOne(o => o.Target)
                    .WithMany(f => f.Followers)
                    .HasForeignKey(o => o.TargetID)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}