using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api2.Data;
using api2.Models;
using AutoMapper;
using api2.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace api2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        private readonly IMapper _mapper;

        public StudentController(ApplicationDBContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        /// <summary>
        /// Get list of student
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetStudent()
        {
            IEnumerable<Student> objFromDb = _db.Student.Include(x => x.Department).OrderBy(x => x.Name).ToList();
            return Ok(objFromDb);
        }
        /// <summary>
        /// Get single record of student
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpGet("{studentId:int}", Name = "GetStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetStudent(int studentId)
        {
            var objFromDb = _db.Student.Include(x => x.Department).FirstOrDefault(x => x.Id == studentId);
            if (objFromDb == null)
            {
                return NotFound();
            }
            return Ok(objFromDb);
        }
        /// <summary>
        /// Create Student
        /// </summary>
        /// <param name="StudentCreateDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateStudent(Models.Dtos.CreateStudentDto StudentCreateDto)
        {
            bool isExist = _db.Student.Any(x => x.Email.ToLower().Trim() == StudentCreateDto.Email.ToLower().Trim());

            if (StudentCreateDto == null)
            {
                return BadRequest();
            }
            if(isExist)
            {
                ModelState.AddModelError("", "Email already exist");
                return StatusCode(400, ModelState);
            }

            var student = _mapper.Map<Student>(StudentCreateDto);
            _db.Student.Add(student);
            _db.SaveChanges();
            return CreatedAtRoute(nameof(GetStudent), new { studentId = student.Id }, student);
        }

        /// <summary>
        /// Update student
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="student"></param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateStudent(int studentId, StudentUpdateDto student)
        {
            bool isExist = _db.Student.Any(x => x.Email.ToLower().Trim() == student.Email.ToLower().Trim());

            if (student == null || student.Id != studentId)
            {
                return BadRequest();
            }
            if (isExist)
            {
                ModelState.AddModelError("", "Email already exist");
                return StatusCode(400, ModelState);
            }

            var obj = _mapper.Map<Student>(student);
            _db.Student.Update(obj);
            _db.SaveChanges();
            return CreatedAtRoute(nameof(GetStudent), new { studentId = obj.Id }, obj);
        }

        /// <summary>
        /// Delete student
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteStudent(int studentId)
        {
            Student student = _db.Student.FirstOrDefault(x => x.Id == studentId);
            if (studentId != student.Id)
            {
                return NotFound();
            }
            _db.Student.Remove(student);
            _db.SaveChanges();
            return NoContent();
        }
    }
}
