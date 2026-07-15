using HotelReservationSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservationSystem.Configurations
{
    public class RoomTypeConfiguration : IEntityTypeConfiguration<RoomType>
    {
        public void Configure(EntityTypeBuilder<RoomType> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(t => t.Description)
                   .HasMaxLength(500);

            builder.HasMany(t => t.Rooms)
                   .WithOne(r => r.RoomType)
                   .HasForeignKey(r => r.RoomTypeId);
        }
    }
}
