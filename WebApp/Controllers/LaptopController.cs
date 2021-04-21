﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer;
using Models;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    
    public class LaptopController : Controller
    {
        private readonly AppDbContext _context;

        public LaptopController(AppDbContext context)
        {
            _context = context;
        }
        
        // GET: Laptop
        public IActionResult Index()
        {
            
            return View("_Index");
        }
        public async Task<IActionResult> AdminIndex()
        {

            return View(await _context.LaptopModel.ToListAsync());
        }
        public async Task<IActionResult> UserIndex()
        {

            return View("Index2",await _context.LaptopModel.ToListAsync());
        }
        // GET: Laptop/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laptopModel = await _context.LaptopModel
                .FirstOrDefaultAsync(m => m.ID == id);
            if (laptopModel == null)
            {
                return NotFound();
            }

            return View(laptopModel);
        }

        // GET: Laptop/Create
       [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Laptop/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Product_Name,Img_Url,Price,Description")] LaptopModel laptopModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(laptopModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(laptopModel);
        }
        [Authorize(Roles = "Admin")]
        // GET: Laptop/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laptopModel = await _context.LaptopModel.FindAsync(id);
            if (laptopModel == null)
            {
                return NotFound();
            }
            return View(laptopModel);
        }

        // POST: Laptop/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,Product_Name,Img_Url,Price,Description")] LaptopModel laptopModel)
        {
            if (id != laptopModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(laptopModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LaptopModelExists(laptopModel.ID))
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
            return View(laptopModel);
        }
        [Authorize(Roles = "Admin")]
        // GET: Laptop/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laptopModel = await _context.LaptopModel
                .FirstOrDefaultAsync(m => m.ID == id);
            if (laptopModel == null)
            {
                return NotFound();
            }

            return View(laptopModel);
        }

        // POST: Laptop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var laptopModel = await _context.LaptopModel.FindAsync(id);
            _context.LaptopModel.Remove(laptopModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LaptopModelExists(string id)
        {
            return _context.LaptopModel.Any(e => e.ID == id);
        }
    }
}
