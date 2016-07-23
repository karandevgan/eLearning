using Learning.Data.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Learning.Data.Mappers {
    public class CourseMapper : EntityTypeConfiguration<Course> {
        public CourseMapper() {
            this.ToTable("Courses");

            this.HasKey(c => c.Id);
            this.Property(c => c.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            this.Property(c => c.Id).IsRequired();

            this.Property(c => c.Name).IsRequired();
            this.Property(c => c.Name).HasMaxLength(255);

            this.Property(c => c.Duration).IsRequired();

            this.Property(c => c.Description).IsOptional();
            this.Property(c => c.Description).HasMaxLength(1000);

            this.HasRequired(c => c.CourseSubject).WithMany(c => c.Courses).Map(s => s.MapKey("SubjectID"));
            this.HasRequired(c => c.CourseTutor).WithMany(c => c.Courses).Map(t => t.MapKey("TutorID"));
        }
    }
}
