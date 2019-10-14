using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Proiect.Studenti.Models;

namespace Proiect.Studenti.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentiController : ControllerBase
    {
        private readonly StudentContext _context;

        public StudentiController(StudentContext context)
        {
            _context = context;
        }

        //GET: api/studenti
        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetStudenti()
        {
            return _context.Studenti;
        }

        //GET: api/studenti/n
        [HttpGet("{id}")]
        public ActionResult<Student> GetStudent(int id)
        {
            var student = _context.Studenti.Find(id);

            if (student == null)
            {
                return NotFound();
            }
            
            if (student.Nume.Equals(" ") || student.Prenume.Equals(" ") || student.Localitate.Equals(" "))
            {
                return BadRequest();
            }
            
            return student;
        }

        //PUT: api/studenti
        [HttpPut]
        public ActionResult<Student> PutStudent(Student student)
        {
            _context.Studenti.Add(student);
            _context.SaveChanges();

            return CreatedAtAction("GetStudent", new { id = student.IdStudent }, student);
        }

        //DELETE: api/studenti/n
        [HttpDelete("{id}")]
        public ActionResult<Student> DeleteStudent(int id)
        {           
            StringValues values;
            bool validKey = false;

            if (this.Request.Headers.TryGetValue("X-API-KEY", out values))
            {
                if (values.FirstOrDefault().Equals("123456"))
                {
                    validKey = true;
                }              
            }

            if (!validKey)
            {
                return BadRequest();
            }
            
            var student = _context.Studenti.Find(id);
            if (student == null)
            {
                return BadRequest();
            }

            _context.Studenti.Remove(student);
            _context.SaveChanges();

            return student;
        }
    }
}