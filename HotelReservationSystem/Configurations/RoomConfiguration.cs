using HotelReservationSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservationSystem.Configurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Capacity)
                   .IsRequired();

            builder.Property(x => x.Description)
                   .HasMaxLength(500)
                   .IsRequired();

            builder.Property(x => x.RoomNumber)
                   .HasMaxLength(5)
                   .IsRequired();

            builder.Property(x => x.PricePerNight)
                   .HasColumnType("decimal(18, 2)");

            builder.HasOne(x => x.RoomType)
                   .WithMany(t => t.Rooms)
                   .HasForeignKey(x => x.RoomTypeId);

            builder.HasMany(r => r.Resrvations)
                   .WithOne(r => r.room)
                   .HasForeignKey(r => r.RoomId);
        }
    }
}
