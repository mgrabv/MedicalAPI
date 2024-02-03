using System;
using System.Collections.Generic;

namespace MedicalAPI.Models
{
    public class Prescription
    {
        public int IdPrescription { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public int IdPatient { get; set; }
        public int IdDoctor { get; set; }


        public virtual ICollection<Prescription_Medicament> Prescription_Medicaments { get; set; }
        = new HashSet<Prescription_Medicament>();

        public virtual Patient PatientNavigation { get; set; }
        public virtual Doctor DoctorNavigation { get; set; }
    }
}
