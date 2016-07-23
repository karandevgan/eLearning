using Learning.Data.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Learning.Data.Mappers {
    public class TutorMapper: EntityTypeConfiguration<Tutor> {
        public TutorMapper() {
            this.ToTable("Tutors");

            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired();
            this.Property(t => t.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            this.Property(t => t.Email).IsRequired();
            this.Property(t => t.Email).HasMaxLength(255);
            this.Property(t => t.Email).IsUnicode(false);

            this.Property(t => t.UserName).IsRequired();
            this.Property(t => t.UserName).HasMaxLength(50);
            this.Property(t => t.UserName).IsUnicode(false);

            this.Property(t => t.Password).IsRequired();
            this.Property(t => t.Password).HasMaxLength(255);

            this.Property(t => t.FirstName).IsRequired();
            this.Property(t => t.FirstName).HasMaxLength(255);

            this.Property(t => t.LastName).IsRequired();
            this.Property(t => t.LastName).HasMaxLength(255);

            this.Property(t => t.Gender).IsOptional();
        }
    }
}
