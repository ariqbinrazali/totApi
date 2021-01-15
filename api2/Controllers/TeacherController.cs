using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api2.Data;
using api2.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using api2.Models.Dtos;

namespace api2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TeacherController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        private readonly IMapper _mapper;

        public TeacherController(ApplicationDBContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        /// <summary>
        /// Get list of teacher
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetTeachers()
        {
            IEnumerable<Teacher> objFromDb = _db.Teachers.OrderBy(x => x.Name).ToList();
            return Ok(objFromDb);
        }
        /// <summary>
        /// Get single record of teacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        [HttpGet("{teacherId:int}", Name = "GetTeacher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTeacher(int teacherId)
        {
            var objFromDb = _db.Teachers.FirstOrDefault(x => x.Id == teacherId);
            if (objFromDb == null)
            {
                return NotFound();
            }
            return Ok(objFromDb);
        }
        /// <summary>
        /// Create Teacher
        /// </summary>
        /// <param name="TeacherCreateDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTeacher(Models.Dtos.TeacherCreateDto TeacherCreateDto)
        {



            if (TeacherCreateDto == null)
            {
                return BadRequest();
            }


            var teacher = _mapper.Map<Teacher>(TeacherCreateDto);
            _db.Teachers.Add(teacher);
            _db.SaveChanges();

            return CreatedAtRoute(nameof(GetTeacher), new { teacherId = teacher.Id }, teacher);
        }

        /// <summary>
        /// Update Teacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="teacher"></param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTeacher(int teacherId, Teacher teacher)
        {
            if (teacher == null && teacherId != teacher.Id)
            {
                return BadRequest();
            }

            _db.Teachers.Update(teacher);
            _db.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// Delete Teacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTeacher(int teacherId)
        {
            Teacher teacher = _db.Teachers.FirstOrDefault(x => x.Id == teacherId);
            if (teacherId != teacher.Id)
            {
                return NotFound();
            }
            _db.Teachers.Remove(teacher);
            _db.SaveChanges();
            return NoContent();
        }
    }
}
