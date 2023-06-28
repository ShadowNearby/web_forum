﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebForumApi.Domain.Entities;
using WebForumApi.Domain.Entities.Common;
using BC=BCrypt.Net.BCrypt;

namespace WebForumApi.Infrastructure.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion<UserId.EfCoreValueConverter>();
        builder.Property(x => x.Email).IsRequired().HasMaxLength(128);
        builder.HasIndex(x => x.Email).IsUnique();
        builder.Property(x => x.Username).IsRequired().HasMaxLength(64);
        builder.Property(x => x.Avatar).HasMaxLength(256);
        builder.HasIndex(x => x.Username).IsUnique();
        builder.Property(x => x.About).HasMaxLength(128);
        builder.Property(x => x.Location).HasMaxLength(128);
        builder.Property(x => x.Role).HasMaxLength(16);
        builder.HasOne(x => x.Token).WithOne(x => x.User).HasForeignKey<Token>(u => u.UserId);
        builder.ToTable("user");
    }
}

public class UserFollowConfiguration : IEntityTypeConfiguration<UserFollow>
{
    public void Configure(EntityTypeBuilder<UserFollow> builder)
    {
        builder.HasKey(x => new
        {
            x.UserId, UserFollowingId = x.UserIdFollowing
        });
        builder.HasOne(c => c.User).WithMany(u => u.UsersFollowing).HasForeignKey(c => c.UserId).IsRequired();
        builder.HasOne(c => c.UserFollowing).WithMany(u => u.UsersFollowed).HasForeignKey(c => c.UserIdFollowing).IsRequired();
        builder.ToTable("user_following");
    }
}