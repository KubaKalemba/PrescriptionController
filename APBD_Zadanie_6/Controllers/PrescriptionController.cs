using APBD_Zadanie_6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBD_Zadanie_6.Controllers;

public class PrescriptionController : ControllerBase
{
    private readonly Context _context;

        public PrescriptionController(Context context)
        {
            _context = context;
        }
        
        // Post new prescription
        [HttpPost]
        public async Task<IActionResult> AddPrescription([FromBody] PrescriptionRequest request)
        {
            
            //max 10 medicines
            if (request.Medicaments.Count > 10)
            {
                return BadRequest("A prescription can contain a maximum of 10 medicaments.");
            }

            //if patient doesnt exist create one
            var patient = await _context.Patients.FindAsync(request.PatientId);
            if (patient == null)
            {
                patient = new Patient { IdPatient = request.PatientId, FirstName = request.PatientFirstName, LastName = request.PatientLastName };
                _context.Patients.Add(patient);
            }

            //if doctor doesnt exist return
            var doctor = await _context.Doctors.FindAsync(request.DoctorId);
            if (doctor == null)
            {
                return BadRequest("Doctor not found.");
            }

            //if invalid medicine return
            var invalidMedicaments = request.Medicaments.Where(id => !_context.Medicaments.Any(m => m.IdMedicament == id)).ToList();
            if (invalidMedicaments.Any())
            {
                return BadRequest($"Medicament(s) with ID(s) {string.Join(", ", invalidMedicaments)} not found.");
            }

            //check if prescription is still working
            if (request.Date >= request.DueDate)
            {
                return BadRequest("Prescription outdated");
            }
            
            //add new prescription
            var prescription = new Prescription
            {
                Date = request.Date,
                DueDate = request.DueDate,
                IdPatient = patient.IdPatient,
                IdDoctor = doctor.IdDoctor,
                PrescriptionMedicaments = request.Medicaments.Select(medId => new PrescriptionMedicament
                {
                    IdMedicament = medId,
                    Dose = request.Dose,
                    Details = request.Details
                }).ToList()
            };

            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            return Ok(prescription);
        }
    }

public class PrescriptionRequest
{
    public int PatientId { get; set; }
    public string PatientFirstName { get; set; }
    public string PatientLastName { get; set; }
    public int DoctorId { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<int> Medicaments { get; set; }
    public int Dose { get; set; }
    public string Details { get; set; }
}
