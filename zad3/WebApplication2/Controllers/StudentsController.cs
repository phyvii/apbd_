
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private const string FilePath = "data/students.csv";

        
        [HttpGet]
        public IActionResult GetStudents()
        {
            List<Student> students = ReadStudentsFromCsv();
            return Ok(students);
        }

        
        [HttpGet("{IndexNumber}")]
        public IActionResult GetStudent(string IndexNumber)
        {
            List<Student> students = ReadStudentsFromCsv();
            Student student = students.Find(s => s.IndexNumber == IndexNumber);
            if (student == null)
                return NotFound();

            return Ok(student);
        }

        
        [HttpPost]
        public IActionResult AddStudent(Student student)
        {
            List<Student> students = ReadStudentsFromCsv();
            student.Id = GetNextId(students);
            students.Add(student);
            WriteStudentsToCsv(students);
            return Ok(student);
        }

        
        [HttpPut("{IndexNumber}")]
        public IActionResult UpdateStudent(string IndexNumber, Student updatedStudent)
        {
            List<Student> students = ReadStudentsFromCsv();
            Student student = students.Find(s => s.IndexNumber == IndexNumber);
            if (student == null)
                return NotFound();

            
            student.FirstName = updatedStudent.FirstName;
            student.LastName = updatedStudent.LastName;
            student.IndexNumber = updatedStudent.IndexNumber;
            student.BirthDate = updatedStudent.BirthDate;
            student.StudyProgram = updatedStudent.StudyProgram;
            student.ModeOfStudy = updatedStudent.ModeOfStudy;
            student.Email = updatedStudent.Email;
            student.FatherName = updatedStudent.FatherName;
            student.MotherName = updatedStudent.MotherName;

            WriteStudentsToCsv(students);
            return Ok(student);
        }

        
        [HttpDelete("{IndexNumber}")]
        public IActionResult DeleteStudent(string IndexNumber)
        {
            List<Student> students = ReadStudentsFromCsv();
            Student student = students.Find(s => s.IndexNumber == IndexNumber);
            if (student == null)
                return NotFound();

            students.Remove(student);
            WriteStudentsToCsv(students);
            return Ok(student);
        }

        private List<Student> ReadStudentsFromCsv()
        {
            List<Student> students = new List<Student>();
            var lines = System.IO.File.ReadAllLines(FilePath);
            foreach (string line in lines)
            {
                var values = line.Split(',');
                students.Add(new Student
                {
                    Id = int.Parse(values[0]),
                    FirstName = values[1],
                    LastName = values[2],
                    IndexNumber = values[3],
                    BirthDate = values[4],
                    StudyProgram = values[5],
                    ModeOfStudy = values[6],
                    Email = values[7],
                    FatherName = values[8],
                    MotherName = values[9]
                });
            }
            return students;
        }

        private void WriteStudentsToCsv(List<Student> students)
        {
            using var writer = new StreamWriter(FilePath);
            foreach (var student in students)
            {
                var line = $"{student.Id},{student.FirstName},{student.LastName},{student.IndexNumber},{student.BirthDate},{student.StudyProgram},{student.ModeOfStudy},{student.Email},{student.FatherName},{student.MotherName}";
                writer.WriteLine(line);
            }
        }

        private int GetNextId(List<Student> students)
        {
            if (students.Count == 0)
                return 1;

            return students[students.Count-1].Id+1;
        }
    }
}
