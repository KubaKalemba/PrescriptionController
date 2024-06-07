using APBD_Zadanie_6.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBD_Zadanie_6.Configuration;

public class PrescriptionConfig : IEntityTypeConfiguration<Prescription> 
{ 
    public void Configure(EntityTypeBuilder<Prescription> builder) 
    { 
        builder.HasKey(e => e.IdPrescription).HasName("Prescription_PK"); 
        builder.Property(e => e.IdPrescription).UseIdentityColumn();
        
        builder.Property(e => e.Date).IsRequired(); 
        builder.Property(e => e.DueDate).IsRequired();

            builder.HasOne(e => e.Patient)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(e => e.IdPatient)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Prescription_Patient");

            builder.HasOne(e => e.Doctor)
                .WithMany(d => d.Prescriptions)
                .HasForeignKey(e => e.IdDoctor)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Prescription_Doctor");
            var prescriptions = new List<Prescription>
            {
                new Prescription
                {
                    IdPrescription = 1,
                    Date = new DateTime(2022, 1, 1),
                    DueDate = new DateTime(2022, 1, 10),
                    IdPatient = 1,
                    IdDoctor = 1
                },
                new Prescription
                {
                    IdPrescription = 2,
                    Date = new DateTime(2022, 2, 1),
                    DueDate = new DateTime(2022, 2, 10),
                    IdPatient = 2,
                    IdDoctor = 2
                },
                new Prescription
                {
                    IdPrescription = 3,
                    Date = new DateTime(2022, 3, 1),
                    DueDate = new DateTime(2022, 3, 10),
                    IdPatient = 3,
                    IdDoctor = 3
                }
            };

            builder.HasData(prescriptions);
        }
    }