using MedicalAPI.DTOs;
using MedicalAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAPI.Services
{
    public class MedicalService
    {
        private MedicalContext Context;

        public MedicalService(MedicalContext Context)
        {
            this.Context = Context;
        }

        public async Task<Doctor> GetDoctor(int Id)
        {
            return await Context.Doctors.FindAsync(Id);
        }

        public async Task AddDoctor(DoctorInfo Info)
        {
            await Context.Doctors.AddAsync(new Doctor()
            {
                FirstName = Info.FirstName,
                LastName = Info.LastName,
                Email = Info.Email
            });

            await Context.SaveChangesAsync();
        }

        public async Task UpdateDoctor(int Id, DoctorInfo Info)
        {
            Doctor Doc = await Context.Doctors.FindAsync(Id);

            Doc.FirstName = Info.FirstName;
            Doc.LastName = Info.LastName;
            Doc.Email = Info.Email;

            await Context.SaveChangesAsync();
        }

        public async Task DeleteDoctor(int Id)
        {
            Doctor Doc = await Context.Doctors.FindAsync(Id);

            Context.Doctors.Remove(Doc);

            await Context.SaveChangesAsync();
        }

        public async Task<PrescriptionInfo> GetPrescriptionInfo(int Id)
        {
            return await Context.Prescriptions.Where(p => p.IdPrescription == Id).Include(p => p.DoctorNavigation)
                .ThenInclude(p => p.Prescriptions).ThenInclude(p => p.PatientNavigation)
                .ThenInclude(p => p.Prescriptions).ThenInclude(p => p.Prescription_Medicaments)
                .ThenInclude(p => p.MedicamentNavigation).Select(p => new PrescriptionInfo()
                {
                    IdPrescription = p.IdPrescription,
                    Date = p.Date,
                    DueDate = p.DueDate,

                    Patient = new PatientInfo()
                    {
                        FirstName = p.PatientNavigation.FirstName,
                        LastName = p.PatientNavigation.LastName,
                        Birthdate = p.PatientNavigation.BirthDate
                    },

                    Doctor = new DoctorInfo()
                    {
                        FirstName = p.DoctorNavigation.FirstName,
                        LastName = p.DoctorNavigation.LastName,
                        Email = p.DoctorNavigation.Email
                    },

                    Medicaments = p.Prescription_Medicaments.Select(pm => new MedicamentInfo()
                    {
                        Name = pm.MedicamentNavigation.Name,
                        Description = pm.MedicamentNavigation.Description,
                        Type = pm.MedicamentNavigation.Type,
                        Dose = pm.Dose,
                        Details = pm.Details
                    }).ToList()
                }).FirstAsync();
        }

        public async Task<bool> DoctorExists(int Id)
        {
            return await Context.Doctors.AnyAsync(d => d.IdDoctor == Id);
        }

        public async Task<bool> PrescriptionExists(int Id)
        {
            return await Context.Prescriptions.AnyAsync(p => p.IdPrescription == Id);
        }
    }
}
