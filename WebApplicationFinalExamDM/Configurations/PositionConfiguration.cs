using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplicationFinalExamDM.Models;

namespace WebApplicationFinalExamDM.Configurations
{
    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(256);

            builder.HasMany(x => x.Members).WithOne(x => x.Position).HasForeignKey(x => x.PositionId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
