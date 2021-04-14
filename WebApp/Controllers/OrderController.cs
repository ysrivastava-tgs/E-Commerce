using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer;
using Models;
using Microsoft.AspNetCore.Authorization;
using System.Dynamic;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _securityManager;
        private readonly SignInManager<ApplicationUser> _loginManager;
        public OrderController(AppDbContext context, UserManager<ApplicationUser>_sec, SignInManager<ApplicationUser>_log)
        {
            _context = context;
            _securityManager = _sec;
            _loginManager = _log;
        }
        [Authorize]
        // GET: Order
        public async Task<IActionResult> Index()
        {
            var user = await this._securityManager.GetUserAsync(User);
            /*await _context.DetailsModel.
            return View(await _context.DetailsModel.AllAsync(m => m.Name == user.UserName));*/

            var list = await _context.DetailsModel.ToListAsync();
             list = list.Where(ord => ord.Name == user.UserName).ToList();
            return View(list);
        }
        [Authorize]
        // GET: Order/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetails = await _context.DetailsModel
                .FirstOrDefaultAsync(m => m.Oid == id);
            var pid = orderDetails.Pid;
            var laptopModel = await _context.LaptopModel
                .FirstOrDefaultAsync(m => m.ID == pid);
            OrderDetails ord = new OrderDetails();
            ord = orderDetails;
            ord.Product = laptopModel;
            if (orderDetails == null)
            {
                return NotFound();
            }

            return View(ord);
        }
        [Authorize]
        
        // GET: Order/Create
        public async Task<IActionResult> CreateAsync(string id)
        {
            ViewBag.Pid = id;
            var laptopModel = await _context.LaptopModel
                .FirstOrDefaultAsync(m => m.ID == id);
            /* dynamic mymodel = new ExpandoObject();
             mymodel.Laptop = laptopModel;
             mymodel.Order = this._ord;*/
            var user = await _securityManager.GetUserAsync(User);
            OrderDetails ord = new OrderDetails();
            ord.Name = user.UserName;
            ord.Address = user.Address;
            ord.Phone = user.PhoneNumber;
          
            ord.Product = laptopModel;
            return View(ord);
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( OrderDetails orderDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orderDetails);
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetails = await _context.DetailsModel.FindAsync(id);
            if (orderDetails == null)
            {
                return NotFound();
            }
            return View(orderDetails);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Oid,Name,Address,Phone")] OrderDetails orderDetails)
        {
            if (id != orderDetails.Oid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailsExists(orderDetails.Oid))
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
            return View(orderDetails);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetails = await _context.DetailsModel
                .FirstOrDefaultAsync(m => m.Oid == id);
            if (orderDetails == null)
            {
                return NotFound();
            }

            return View(orderDetails);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var orderDetails = await _context.DetailsModel.FindAsync(id);
            _context.DetailsModel.Remove(orderDetails);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailsExists(string id)
        {
            return _context.DetailsModel.Any(e => e.Oid == id);
        }
    }
}
