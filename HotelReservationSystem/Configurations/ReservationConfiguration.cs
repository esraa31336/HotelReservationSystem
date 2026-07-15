using HotelReservationSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelReservationSystem.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.TotalPrice)
                   .IsRequired()
                   .HasColumnType("decimal(18, 2)");

            builder.Property(r => r.Status)
                   .IsRequired();

            builder.HasOne(r => r.room)
                   .WithMany(rm => rm.Resrvations)
                   .HasForeignKey(r => r.RoomId);
                
        }
    }
}
