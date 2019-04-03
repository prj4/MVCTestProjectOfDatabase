using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design.Internal;
using PhotoBook.Repository.HostRepository;
using Microsoft.Extensions.Configuration;
using PhotoBookDatabase.Model;

namespace MVCTestProjectOfDatabase.Controllers
{
    public class HostController : Controller
    {
        private readonly string _connectionString =
            "Server=tcp:katrinesphotobook.database.windows.net,1433;Initial Catalog=PhotoBook4;" +
            "Persist Security Info=False;User ID=Ingeniørhøjskolen@katrinesphotobook.database.windows.net;" +
            "Password=Katrinebjergvej22;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private IConfiguration _configuration;
        

        private HostRepository _hostRepository;

        public HostController(IConfiguration iconfig)
        {
            _configuration = iconfig;

           _connectionString = _configuration.GetConnectionString("DefaultConnection");

            _hostRepository = new HostRepository(_connectionString);
        }
        // GET: Host
        public async Task<IActionResult> Index()
        {
            
            return View(await _hostRepository.GetHosts());
        }

        // GET: Host/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var host = await _hostRepository.GetHost(id);
            if (host == null)
            {
                return NotFound();
            }

            return View(host);
        }

        public IActionResult Create()
        {
            //return Content(_connectionString);
            return View();
        }

        // GET: Applicant/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,Name")] Host host)
        {
            if (ModelState.IsValid)
            {
                await Task.Run(() => 
                    _hostRepository.InsertHost(host)
                );
                
                return RedirectToAction(nameof(Index));
            }

            return View(host);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var host = await _hostRepository.GetHost(id);

            if (host == null)
            {
                return NotFound();
            }

            return View(host);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Email,PictureTakerId,Name")] Host host)
        {
            if (id != host.PictureTakerId)
            {
                return NotFound();
            }

            await Task.Run(() =>
                _hostRepository.UpdateHost(host)
                );
 
            if (await _hostRepository.GetHost( host.PictureTakerId) == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Applicant/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var host = await _hostRepository.GetHost(id);
            if (host == null)
            {
                return NotFound();
            }
            return View(host);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await Task.Run(() =>
                _hostRepository.DeleteHost(id)
                );

            return RedirectToAction(nameof(Index));
        }
    }
}