using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Online_Course_Enrollment_System_Properties.Domain
{

    public class EnrollmentRepository
    {
        private readonly List<Course> _courses = new();

        public void Save(Course course)
        {
            _courses.Add(course);
        }

        public IReadOnlyList<Course> All() => _courses;
    }

}
