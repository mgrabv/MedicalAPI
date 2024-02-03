using System;
using System.Collections.Generic;

namespace MedicalAPI.DTOs
{
    public class PrescriptionInfo
    {
        public int IdPrescription { get; set; }

        public DateTime Date { get; set; }

        public DateTime DueDate { get; set; }

        public PatientInfo Patient { get; set; }

        public DoctorInfo Doctor { get; set; }

        public List<MedicamentInfo> Medicaments { get; set; }

    }
}
