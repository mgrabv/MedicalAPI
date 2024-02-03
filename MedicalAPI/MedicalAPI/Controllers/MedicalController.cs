using MedicalAPI.DTOs;
using MedicalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MedicalAPI.Controllers
{
    [Authorize]
    [Route("api/medical")]
    [ApiController]
    public class MedicalController : ControllerBase
    {
        private MedicalService Service;

        public MedicalController(MedicalService Service)
        {
            this.Service = Service;
        }

        [HttpGet("doctors/{IdDoctor}")]
        public async Task<IActionResult> DownloadDoctorInfo(int IdDoctor)
        {
            if (!await Service.DoctorExists(IdDoctor))
            {
                return StatusCode(404, "Doctor with the given ID does not exist in the database.");
            }

            return Ok(await Service.GetDoctor(IdDoctor));
        }

        [HttpPut("doctors")]
        public async Task<IActionResult> AddDoctor(DoctorInfo Info)
        {
            await Service.AddDoctor(Info);
            return Ok();
        }

        [HttpPost("doctors/{IdDoctor}")]
        public async Task<IActionResult> UpdateDoctor(int IdDoctor, DoctorInfo Info)
        {
            if (!await Service.DoctorExists(IdDoctor))
            {
                return StatusCode(404, "Doctor with the given ID does not exist in the database.");
            }

            await Service.UpdateDoctor(IdDoctor, Info);
            return Ok();
        }

        [HttpDelete("doctors/{IdDoctor}")]
        public async Task<IActionResult> DeleteDoctor(int IdDoctor)
        {
            if (!await Service.DoctorExists(IdDoctor))
            {
                return StatusCode(404, "Doctor with the given ID does not exist in the database.");
            }

            await Service.DeleteDoctor(IdDoctor);
            return Ok();
        }

        [HttpGet("prescriptions/{IdPrescription}")]
        public async Task<IActionResult> GetPrescriptionInfo(int IdPrescription)
        {
            if (!await Service.PrescriptionExists(IdPrescription))
            {
                return StatusCode(404, "Prescription with the given ID does not exist in the database.");
            }

            return Ok(await Service.GetPrescriptionInfo(IdPrescription));
        }
    }
}
