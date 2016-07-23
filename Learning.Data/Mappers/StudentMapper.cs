﻿using Learning.Data.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Learning.Data.Mappers {
    public class StudentMapper: EntityTypeConfiguration<Student> {
        public StudentMapper() {
            this.ToTable("Students");

            this.HasKey(s => s.Id);
            this.Property(s => s.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            this.Property(s => s.Id).IsRequired();

            this.Property(s => s.Email).IsRequired();
            this.Property(s => s.Email).HasMaxLength(255);
            this.Property(s => s.Email).IsUnicode(false);

            this.Property(s => s.UserName).IsRequired();
            this.Property(s => s.UserName).HasMaxLength(50);
            this.Property(s => s.UserName).IsUnicode(false);

            this.Property(s => s.Password).IsRequired();
            this.Property(s => s.Password).HasMaxLength(255);

            this.Property(s => s.FirstName).IsRequired();
            this.Property(s => s.FirstName).HasMaxLength(255);

            this.Property(s => s.LastName).IsRequired();
            this.Property(s => s.LastName).HasMaxLength(255);

            this.Property(s => s.Gender).IsOptional();

            this.Property(s => s.DateOfBirth).IsRequired();
            this.Property(s => s.DateOfBirth).HasColumnType("smalldatetime");

            this.Property(s => s.RegistrationDate).IsOptional();
            this.Property(s => s.DateOfBirth).HasColumnType("smalldatetime");

            this.Property(s => s.LastLoginDate).IsOptional();
            this.Property(s => s.LastLoginDate).HasColumnType("smalldatetime");
        }
    }
}
