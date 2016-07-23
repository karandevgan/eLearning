using Learning.Data.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Learning.Data.Mappers {
    public class SubjectMapper: EntityTypeConfiguration<Subject> {
        public SubjectMapper() {
            this.ToTable("Subjects");

            this.HasKey(s => s.Id);
            this.Property(s => s.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            this.Property(s => s.Id).IsRequired();

            this.Property(s => s.Name).IsRequired();
            this.Property(s => s.Name).HasMaxLength(255);
        }
    }
}
