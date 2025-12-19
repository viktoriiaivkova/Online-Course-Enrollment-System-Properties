using FsCheck;
using FsCheck.Xunit;
using Online_Course_Enrollment_System_Properties.Domain;
using Online_Course_Enrollment_System_Properties.Tests.Arbitraries;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Online_Course_Enrollment_System_Properties.Tests.Properties
{
    public class EnrollmentProperties
    {
        private readonly EnrollmentRepository _repository = new();

        [Property(Arbitrary = new[] { typeof(CourseArbitraries) })]
        public bool NewCourse_IsAlways_Empty(Course course)
        {
            return course.Students.Count == 0;
        }

        [Property(Arbitrary = new[] { typeof(CourseArbitraries) })]
        public bool Student_CannotBeEnrolled_Twice(Course course, Student student)
        {
            course.Enroll(student);
            var secondEnrollment = course.Enroll(student);
            return secondEnrollment == false && course.Students.Count(s => s.Id == student.Id) == 1;
        }

        [Property(Arbitrary = new[] { typeof(CourseArbitraries) })]
        public bool Course_CannotExceed_Capacity(Course course, List<Student> students)
        {
            foreach (var s in students) course.Enroll(s);
            return course.Students.Count <= course.Capacity;
        }

        [Property(Arbitrary = new[] { typeof(CourseArbitraries) })]
        public bool Enrollment_ReturnsFalse_WhenCourseIsFull(Student s1, Student s2)
        {
            var course = new Course("ValidCourse", 1);
            course.Enroll(s1);
            if (s1.Id == s2.Id) return true;
            return course.Enroll(s2) == false;
        }

        [Property(Arbitrary = new[] { typeof(CourseArbitraries) })]
        public bool Operations_OverValidData_NeverThrowExceptions(Course course, Student student)
        {
            var exception = Record.Exception(() => course.Enroll(student));
            return exception == null;
        }

        [Property(Arbitrary = new[] { typeof(CourseArbitraries) })]
        public bool Repository_SavesAndRetrieves_WithoutLoss(Course course)
        {
            _repository.Save(course);
            return _repository.All().Any(c => c.Id == course.Id);
        }

        [Property(Arbitrary = new[] { typeof(CourseArbitraries) })]
        public Property Enrolling_UniqueStudents_UpToCapacity_AlwaysSucceeds()
        {
            return Prop.ForAll(CourseArbitraries.NonEmptyString(), CourseArbitraries.PositiveInt(), (id, capacity) =>
            {
                var course = new Course(id, capacity);
                var results = new List<bool>();
                for (int i = 0; i < capacity; i++)
                {
                    results.Add(course.Enroll(new Student($"Unique-{i}")));
                }
                return (results.All(r => r == true) && course.Students.Count == capacity).ToProperty();
            });
        }

        [Property(Arbitrary = new[] { typeof(CourseArbitraries) })]
        public bool StudentsCollection_IsConsistentWithEnrollment(Course course, Student student)
        {
            var countBefore = course.Students.Count;
            var added = course.Enroll(student);
            var expectedCount = added ? countBefore + 1 : countBefore;

            return course.Students.Count == expectedCount;
        }

        [Property(Arbitrary = new[] { typeof(CourseArbitraries) })]
        public bool Repository_Consistency_MatchesOperations(List<Course> courses)
        {
            var repo = new EnrollmentRepository();
            foreach (var c in courses) repo.Save(c);
            return repo.All().Count == courses.Count;
        }

        [Property(Arbitrary = new[] { typeof(CourseArbitraries) })]
        public bool Enrollment_HasNoSideEffects_OnOtherCourses(Course c1, Course c2, Student s)
        {
            if (c1.Id == c2.Id) return true;
            int initialCount2 = c2.Students.Count;
            c1.Enroll(s);
            return c2.Students.Count == initialCount2;
        }

        [Property(Arbitrary = new[] { typeof(CourseArbitraries) })]
        public bool StudentId_RemainsStable(Course course, Student student)
        {
            course.Enroll(student);
            return course.Students.Any(s => s.Id == student.Id);
        }

        [Property(Arbitrary = new[] { typeof(CourseArbitraries) })]
        public bool CourseProperties_AreImmutable(Course course, Student student)
        {
            var id = course.Id;
            var cap = course.Capacity;
            course.Enroll(student);
            return course.Id == id && course.Capacity == cap;
        }
    }
}