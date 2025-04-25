using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;
using DataBaseFirst.Models.ViewModel;

namespace DataBaseFirst.Controllers
{
    public class EmployeesController : Controller
    {
        // GET: Employees

        protected readonly EMDBEntities1 _context = new EMDBEntities1();
        [HttpGet]
        public ActionResult Index()
        {
            var employeeList = _context.Employees
                .Include(q=>q.QualificationEntries.Select(s=>s.Skill))
                .OrderByDescending(e=>e.EmployeeID).ToList();
            return View(employeeList);
        }
        [HttpPost]
        public ActionResult Delete(int id) 
        {
            var employee = _context.Employees.Where(e=>e.EmployeeID == id).FirstOrDefault();
            if (employee == null)
            {
                return HttpNotFound("No employee found");
            }
            if(!string.IsNullOrEmpty(employee.Picture)) 
            {
                string imagePath = Path.Combine("Image",employee.Picture);
                if (System.IO.File.Exists(imagePath)) 
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            _context.Employees.Remove(employee);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Create() 
        {
            return View();
        }

        public ActionResult AddSlills(int? id) 
        {
            ViewBag.skill = new SelectList(_context.Skills.ToList(),
                "SkillId", "SkillName", (id != null) ? id.ToString() : "");
            return PartialView("_AddSlills");
        }
        [HttpPost]
        public ActionResult Create(EmployeeViewModel vobj,int[] skillid)
        {
            if (ModelState.IsValid)
            {
                Employee employee = new Employee
                {
                    FirstName = vobj.FirstName,
                    lastName = vobj.lastName,
                    DateOfBirth = vobj.DateOfBirth,
                    Salary = vobj.Salary,
                    IsActive = vobj.IsActive,
                };
                HttpPostedFileBase file = vobj.PicturePath;
                if (file != null)
                {
                    string filePath = Path.Combine("/Image/", Guid.NewGuid().ToString())
                        + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath(filePath));
                    employee.Picture = filePath;
                }
                foreach (var item in skillid)
                {
                    QualificationEntry qe = new QualificationEntry
                    {
                        Employee = employee,
                        EmployeeId = employee.EmployeeID,
                        SkillId = item
                    };
                    _context.QualificationEntries.Add(qe);
                }
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else 
            {
                return View();
            }
           
        }
        [HttpGet]
        public ActionResult Edit(int id) 
        {
            Employee employee = _context.Employees.First(x => x.EmployeeID == id);
            var skill = _context.QualificationEntries.Where(q=>q.EmployeeId == id).ToList();
            EmployeeViewModel vobj = new EmployeeViewModel
            {
                EmployeeID = employee.EmployeeID,
                FirstName = employee.FirstName,
                lastName = employee.lastName,
                DateOfBirth = employee.DateOfBirth,
                Salary = employee.Salary,
                IsActive = employee.IsActive,
                Picture = employee.Picture,
            };
            if (skill.Count > 0) 
            {
                foreach (var item in skill) 
                {
                    vobj.QualificationList.Add(item.SkillId);
                }
            }
            return View(vobj);
        }

        [HttpPost]
        public ActionResult Edit(EmployeeViewModel vobj, int[] skillid) 
        {
            if (ModelState.IsValid)
            {
                Employee employee = new Employee
                {
                    EmployeeID= vobj.EmployeeID,
                    FirstName = vobj.FirstName,
                    lastName = vobj.lastName,
                    DateOfBirth = vobj.DateOfBirth,
                    Salary = vobj.Salary,
                    IsActive = vobj.IsActive,
                };
                HttpPostedFileBase file = vobj.PicturePath;
                if (file != null)
                {
                    string imgPath = Path.Combine("/Image/", Guid.NewGuid().ToString())
                        + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath(imgPath));
                    employee.Picture = imgPath;
                }
                else
                    employee.Picture = vobj.Picture;
                var skill = _context.QualificationEntries.
                    Where(e => e.EmployeeId == employee.EmployeeID).ToList();
                foreach (var item in skill)
                {
                    _context.QualificationEntries.Remove(item);
                }
                foreach (var item in skillid)
                {
                    QualificationEntry Q = new QualificationEntry
                    {
                        EmployeeId = employee.EmployeeID,
                        SkillId = item
                    };
                    _context.QualificationEntries.Add(Q);
                }
                _context.Entry(employee).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else { return View(); }
        }
    }
}