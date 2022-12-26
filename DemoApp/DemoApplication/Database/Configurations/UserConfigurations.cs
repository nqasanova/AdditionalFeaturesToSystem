using DemoApplication.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace DemoApplication.Database.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
               .ToTable("Users")
               .HasIndex(u => u.Email).IsUnique();

            builder
               .HasOne(u => u.Basket)
               .WithOne(b => b.User)
               .HasForeignKey<Basket>(u => u.UserId);

            builder
               .HasOne(u => u.Address)
               .WithOne(a => a.User)
               .HasForeignKey<Address>(a => a.UserId);
        }
    }
}
