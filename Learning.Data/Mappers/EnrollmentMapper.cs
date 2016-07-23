using Learning.Data.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Learning.Data.Mappers {
    public class EnrollmentMapper: EntityTypeConfiguration<Enrollment> {
        public EnrollmentMapper() {
            this.ToTable("Enrollments");

            this.HasKey(e => e.Id);
            this.Property(e => e.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            this.Property(e => e.Id).IsRequired();

            this.Property(e => e.EnrollmentDate).IsRequired();
            this.Property(e => e.EnrollmentDate).HasColumnType("smalldatetime");

            this.HasOptional(e => e.Student).WithMany(e => e.Enrollments).Map(s => s.MapKey("StudentID")).WillCascadeOnDelete(false);
            this.HasOptional(e => e.Course).WithMany(e => e.Enrollments).Map(c => c.MapKey("CourseID")).WillCascadeOnDelete(false);
        }
    }
}
