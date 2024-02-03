using Microsoft.EntityFrameworkCore;
using System;

namespace MedicalAPI.Models
{
    public class MedicalContext : DbContext
    {
        public MedicalContext() { }

        public MedicalContext(DbContextOptions<MedicalContext> options) : base(options)
        {
        }

        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Prescription_Medicament> PrescriptionMedicaments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseSqlServer("Insert database connection");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medicament>(entity =>
            {
                entity.HasKey(m => m.IdMedicament);
                entity.Property(m => m.IdMedicament).ValueGeneratedOnAdd();
                entity.Property(m => m.Name).IsRequired()
                                            .HasMaxLength(100);

                entity.Property(m => m.Description).IsRequired()
                                                   .HasMaxLength(100);

                entity.Property(m => m.Type).IsRequired()
                                            .HasMaxLength(100);
            });

            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.HasKey(p => p.IdPrescription);
                entity.Property(p => p.IdPrescription).ValueGeneratedOnAdd();
                entity.Property(p => p.Date).IsRequired();
                entity.Property(p => p.DueDate).IsRequired();

                entity.HasOne(p => p.PatientNavigation)
                      .WithMany(p => p.Prescriptions)
                      .HasForeignKey(p => p.IdPatient);

                entity.HasOne(p => p.DoctorNavigation)
                      .WithMany(p => p.Prescriptions)
                      .HasForeignKey(p => p.IdDoctor);
            });

            modelBuilder.Entity<Prescription_Medicament>(entity =>
            {
                entity.HasKey(p => new { p.IdMedicament, p.IdPrescription });
                entity.Property(p => p.Dose).IsRequired(false);
                entity.Property(p => p.Details).IsRequired()
                                               .HasMaxLength(100);

                entity.HasOne(p => p.MedicamentNavigation)
                      .WithMany(p => p.Prescription_Medicaments)
                      .HasForeignKey(p => p.IdMedicament);

                entity.HasOne(p => p.PrescriptionNavigation)
                      .WithMany(p => p.Prescription_Medicaments)
                      .HasForeignKey(p => p.IdPrescription);
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(d => d.IdDoctor);
                entity.Property(d => d.IdDoctor).ValueGeneratedOnAdd();
                entity.Property(d => d.FirstName).IsRequired()
                                                 .HasMaxLength(100);

                entity.Property(d => d.LastName).IsRequired()
                                                .HasMaxLength(100);

                entity.Property(d => d.Email).IsRequired()
                                             .HasMaxLength(100);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(p => p.IdPatient);
                entity.Property(p => p.IdPatient).ValueGeneratedOnAdd();
                entity.Property(p => p.FirstName).IsRequired()
                                                 .HasMaxLength(100);

                entity.Property(p => p.LastName).IsRequired()
                                                .HasMaxLength(100);

                entity.Property(p => p.BirthDate).IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).ValueGeneratedOnAdd();
                entity.Property(u => u.Username).IsRequired();
                entity.Property(u => u.HashedPassword).IsRequired();
                entity.Property(u => u.Salt).IsRequired();
                entity.Property(u => u.RefreshToken).IsRequired();
                entity.Property(u => u.RefreshTokenExp).IsRequired();
            });

            Seed(modelBuilder);
        }

        protected void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medicament>(entity =>
            {
                entity.HasData(new Medicament()
                {
                    IdMedicament = 1,
                    Name = "Paracetamol",
                    Description = "Anti-Inflamation drug.",
                    Type = "Pills 250mg each."
                },
                new Medicament()
                {
                    IdMedicament = 2,
                    Name = "Ketonal",
                    Description = "Painkiller, Anti-Inflamation drug.",
                    Type = "Pills 100mg each."
                },
                new Medicament()
                {
                    IdMedicament = 3,
                    Name = "Melatonin",
                    Description = "A hormone, helps control the body's sleep cycle, and is an antioxidant.",
                    Type = "Pills 5mg each."
                });


            });

            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.HasData(new Prescription()
                {
                    IdPrescription = 1,
                    Date = DateTime.Now,
                    DueDate = DateTime.Now,
                    IdPatient = 1,
                    IdDoctor = 1
                });
            });

            modelBuilder.Entity<Prescription_Medicament>(entity =>
            {
                entity.HasData(new Prescription_Medicament()
                {
                    IdMedicament = 1,
                    IdPrescription = 1,
                    Dose = 2,
                    Details = "2 pills each day."
                },
                new Prescription_Medicament()
                {
                    IdMedicament = 2,
                    IdPrescription = 1,
                    Dose = 1,
                    Details = "1 pill each day."
                },
                new Prescription_Medicament()
                {
                    IdMedicament = 3,
                    IdPrescription = 1,
                    Dose = 1,
                    Details = "1 pill each day 1h before sleep."
                });
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasData(new Doctor()
                {
                    IdDoctor = 1,
                    FirstName = "Steven",
                    LastName = "Goodman",
                    Email = "Steven.Goodman@gmail.com",
                });
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasData(new Patient()
                {
                    IdPatient = 1,
                    FirstName = "James",
                    LastName = "King",
                    BirthDate = DateTime.Parse("02-05-1977")
                });
            });
        }
    }
}
