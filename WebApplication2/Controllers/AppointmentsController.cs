using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AppointmentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManage)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManage;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User); // <- Bierzemy odwołanie do zalogowanego usera.


            var appts = _context.Appointments
                .Include(a => a.Doctor)
                    .ThenInclude(o => ((Doctor)o).ApplicationUser)
                .Include(a => a.Patient)
                    .ThenInclude(o => ((Patient)o).ApplicationUser)
                .Include(a => a.Room)
                .Include(a => a.Type);
                //.Include(a => a.UserViewModel);

            // Teraz sprawdzamy role
            if (User.IsInRole("Admin") || User.IsInRole("Recepcja"))
                return View(await appts.ToListAsync()); // <- Jeśli warunek jest spełniony to wyświetlają się wszystkie appointmenty
            else
            {
                var matched = await appts.Where(o => o.Doctor.ApplicationUserId == currentUser.Id ||
                o.Patient.ApplicationUserId == currentUser.Id).ToListAsync(); // <- tak można przefiltrować wizyty na podstawie tego czy user jest albo pacjentem albo doktorem w tej konretnej
                return View(matched);
            }
         
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.ApplicationUser)
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.Room)
                .Include(a => a.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }
        [Authorize(Roles = "Recepcja,Admin")]
        // GET: Appointments/Create
        public IActionResult Create()
        {

            ViewData["Lekarz"] = new SelectList(_context.Doctors.Include(o => o.ApplicationUser), "Id", "ApplicationUser.UserN");
            ViewData["Pacjent"] = new SelectList(_context.Patients.Include(o => o.ApplicationUser), "Id", "ApplicationUser.UserN");
            ViewData["Pokój"] = new SelectList(_context.Room, "Id", "Name");
            ViewData["Rodzajwizyty"] = new SelectList(_context.AppointmentTypes, "Id", "Type");
            return View();
        }
        

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DoctorId,RoomId,PatientId,Reservation,ReservationEnd,AppointmentTypeId,ApplicationUserId")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", appointment.PatientId);
            ViewData["RoomId"] = new SelectList(_context.Room, "Id", "Name", appointment.RoomId);
            ViewData["AppointmentTypeId"] = new SelectList(_context.AppointmentTypes, "Id", "Type", appointment.AppointmentTypeId);
            return View(appointment);
        }
        [Authorize(Roles = "Recepcja,Admin")]
        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", appointment.ApplicationUserId);
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", appointment.PatientId);
            ViewData["RoomId"] = new SelectList(_context.Room, "Id", "Id", appointment.RoomId);
            ViewData["AppointmentTypeId"] = new SelectList(_context.AppointmentTypes, "Id", "Id", appointment.AppointmentTypeId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,DoctorId,RoomId,PatientId,Reservation,AppointmentTypeId,ApplicationUserId")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", appointment.ApplicationUserId);
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", appointment.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", appointment.PatientId);
            ViewData["RoomId"] = new SelectList(_context.Room, "Id", "Id", appointment.RoomId);
            ViewData["AppointmentTypeId"] = new SelectList(_context.AppointmentTypes, "Id", "Id", appointment.AppointmentTypeId);
            return View(appointment);
        }
        [Authorize(Roles = "Recepcja,Admin")]
        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.ApplicationUser)
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.Room)
                .Include(a => a.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(long id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}
