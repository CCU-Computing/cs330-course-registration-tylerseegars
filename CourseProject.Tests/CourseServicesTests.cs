using System;
using Xunit;
using cs330_proj1;
using System.Collections.Generic;
using Moq;
using System.Linq;


namespace CourseProject.Tests
{
    public class CourseServicesTests
    {
        [Fact]
        public void GetOfferingsByGoalIdAndSemester_GoalNotFound_ExceptionThrown()
        {
            // Arrange
            var mockRepository = new Mock<ICourseRepository>();
            mockRepository.Setup(m => m.Courses).Returns(GetTestCourses());
            mockRepository.Setup(m => m.Goals).Returns(new List<CoreGoal>(){
            new CoreGoal() {
                Courses = GetTestCourses(),
                Description = "test",
                Id = "CG1",
                Name = "English Literacy"
            }
            });

            mockRepository.Setup(m => m.Offerings).Returns(new List<CourseOffering>() {
                new CourseOffering() {
                    Section = "1",
                    Semester = "Spring 2021",
                    TheCourse = GetTestCourses().First()
                }
            });

            var courseServices = new CourseServices(mockRepository.Object);
            var goalId = "CG5";
            var semester = "Spring 2021";

            // Act/Assert
            Assert.Throws<Exception>(() => courseServices.getOfferingsByGoalIdAndSemester(goalId, semester));
        }


        [Fact]
        public void GetOfferingsByGoalIdAndSemester_GoalIsFoundAndOneCourseOfferingIsInSemester_OfferingIsReturned()
        {
            // Arrange
            var course = new Course() {
                Name= "ARTD 201",
                Title="graphic design",
                Credits=3.0,
                Description="graphic design descr"
            };

            var mockRepository = new Mock<ICourseRepository>();
            mockRepository.Setup(m => m.Courses).Returns(new List<Course> {
                course});

            mockRepository.Setup(m => m.Goals).Returns(new List<CoreGoal>(){
            new CoreGoal() {
                Courses = GetTestCourses(),
                Description = "test",
                Id = "CG1",
                Name = "English Literacy"
            }
            });

            mockRepository.Setup(m => m.Offerings).Returns(new List<CourseOffering>() {
                new CourseOffering() {
                    Section = "1",
                    Semester = "Spring 2021",
                    TheCourse = course
                }
            });

            
            var goalId = "CG1";
            var semester = "Spring 2021";
            var courseServices = new CourseServices(mockRepository.Object);

            //Act
            var result = courseServices.getOfferingsByGoalIdAndSemester(goalId, semester);

            // Assert
            var itemInList = Assert.Single(result);
            // Assert.Equal(2, result.Count());
            Assert.Equal(semester, itemInList.Semester);
            Assert.Equal(course.Name, itemInList.TheCourse.Name);
            
           
        }

        //Add unit tests for GetOfferingsByGoalIdAndSemester_GoalIsFoundAndMultipleCourseOfferingsAreInSemester_OfferingsAreReturned()
        [Fact]
        public void GetOfferingsByGoalIdAndSemester_GoalIsFoundAndMultipleCourseOfferingsAreInSemester_OfferingsAreReturned()
        {
            // Arrange
            var course = new Course() {
                Name= "ARTD 201",
                Title="graphic design",
                Credits=3.0,
                Description="graphic design descr"
            };

            var mockRepository = new Mock<ICourseRepository>();
            mockRepository.Setup(m => m.Courses).Returns(new List<Course> {
                course});

            mockRepository.Setup(m => m.Goals).Returns(new List<CoreGoal>(){
            new CoreGoal() {
                Courses = GetTestCourses(),
                Description = "test",
                Id = "CG1",
                Name = "English Literacy"
            }
            });

            mockRepository.Setup(m => m.Offerings).Returns(new List<CourseOffering>() {
                new CourseOffering() {
                    Section = "1",
                    Semester = "Spring 2021",
                    TheCourse = course
                },
                new CourseOffering() {
                    Section = "2",
                    Semester = "Spring 2021",
                    TheCourse = course
                }
            });

            
            var goalId = "CG1";
            var semester = "Spring 2021";
            var courseServices = new CourseServices(mockRepository.Object);

            //Act
            var result = courseServices.getOfferingsByGoalIdAndSemester(goalId, semester);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, o =>
                o.Section == "1" &&
                o.Semester == "Spring 2021" &&
                o.TheCourse.Name == course.Name);

            Assert.Contains(result, o =>
                o.Section == "2" &&
                o.Semester == "Spring 2021" &&
                o.TheCourse.Name == course.Name);
            
           
        }
        // Add unit test for GetOfferingsByGoalIdAndSemester_GoalIsFoundAndNoCourseOfferingIsInSemester_EmptyListIsReturned()
        [Fact]
        public void GetOfferingsByGoalIdAndSemester_GoalIsFoundAndNoCourseOfferingIsInSemester_EmptyListIsReturned()
        {
            // Arrange
            var course = new Course() {
                Name= "ARTD 201",
                Title="graphic design",
                Credits=3.0,
                Description="graphic design descr"
            };

            var mockRepository = new Mock<ICourseRepository>();
            mockRepository.Setup(m => m.Courses).Returns(new List<Course> {
                course});

            mockRepository.Setup(m => m.Goals).Returns(new List<CoreGoal>(){
            new CoreGoal() {
                Courses = GetTestCourses(),
                Description = "test",
                Id = "CG1",
                Name = "English Literacy"
            }
            });

            mockRepository.Setup(m => m.Offerings).Returns(new List<CourseOffering>() {
                new CourseOffering() {
                    Section = "1",
                    Semester = "Spring 2021",
                    TheCourse = course
                },
                new CourseOffering() {
                    Section = "2",
                    Semester = "Spring 2021",
                    TheCourse = course
                }
            });

            
            var goalId = "CG1";
            var semester = "Spring 2022";
            var courseServices = new CourseServices(mockRepository.Object);

            //Act
            var result = courseServices.getOfferingsByGoalIdAndSemester(goalId, semester);

            // Assert
            Assert.Empty(result);
        }

        //user story 2 getCourses()
        [Fact]
        public void GetCourses_MultipleCoursesFound_ReturnsMultipleCourses()
        {
            // Arrange
            var course1 = new Course() {
                Name= "ARTD 201",
                Title="graphic design",
                Credits=3.0,
                Description="graphic design descr"
            };
            var course2 = new Course() {
                Name= "STAT 201",
                Title="stats",
                Credits=3.0,
                Description="stats descr"
            };

            var mockRepository = new Mock<ICourseRepository>();
            mockRepository.Setup(m => m.Courses).Returns(new List<Course> {
                course1, course2});
            
            var courseServices = new CourseServices(mockRepository.Object);

            //Act
            var result = courseServices.getCourses();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(course1, result);
            Assert.Contains(course2, result);
        }
        [Fact]
        public void GetCourses_OneCourseFound_ReturnsOneCourse()
        {
            // Arrange
            var course = new Course() {
                Name= "ARTD 201",
                Title="graphic design",
                Credits=3.0,
                Description="graphic design descr"
            };

            var mockRepository = new Mock<ICourseRepository>();
            mockRepository.Setup(m => m.Courses).Returns(new List<Course> {
                course});
            
            var courseServices = new CourseServices(mockRepository.Object);

            //Act
            var result = courseServices.getCourses();

            // Assert
            var itemInList = Assert.Single(result);
            Assert.Equal(course.Name, itemInList.Name);
        }
        [Fact]
        public void GetCourses_NoCoursesFound_ReturnsEmptyList()
        {
            // Arrange
            var mockRepository = new Mock<ICourseRepository>();
            mockRepository.Setup(m => m.Courses).Returns(new List<Course> {});
            
            var courseServices = new CourseServices(mockRepository.Object);

            //Act
            var result = courseServices.getCourses();

            // Assert
            Assert.Empty(result);
        }

        //user story 3 getCourseOfferingsBySemester
        [Fact]
        public void GetCourseOfferingsBySemester_MultipleOfferingsFound_MultipleOfferingsReturned()
        {
            // Arrange
            var course = new Course() {
                Name= "ARTD 201",
                Title="graphic design",
                Credits=3.0,
                Description="graphic design descr"
            };

            var mockRepository = new Mock<ICourseRepository>();
            mockRepository.Setup(m => m.Courses).Returns(new List<Course> {
                course});

            var offering1 = new CourseOffering() {
                Section = "1",
                Semester = "Spring 2021",
                TheCourse = course
            };
            var offering2 = new CourseOffering() {
                Section = "2",
                Semester = "Spring 2021",
                TheCourse = course
            };

            mockRepository.Setup(m => m.Offerings).Returns(new List<CourseOffering>() {
                offering1, offering2
            });
            
            var semester = "Spring 2021";
            var courseServices = new CourseServices(mockRepository.Object);

            //Act
            var result = courseServices.getCourseOfferingsBySemester(semester);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(offering1, result);
            Assert.Contains(offering2, result);
        }

        [Fact]
        public void GetCourseOfferingsBySemester_OneOfferingFound_OneOfferingReturned()
        {
            // Arrange
            var course = new Course() {
                Name= "ARTD 201",
                Title="graphic design",
                Credits=3.0,
                Description="graphic design descr"
            };

            var mockRepository = new Mock<ICourseRepository>();
            mockRepository.Setup(m => m.Courses).Returns(new List<Course> {
                course});

            var offering1 = new CourseOffering() {
                Section = "1",
                Semester = "Spring 2021",
                TheCourse = course
            };

            mockRepository.Setup(m => m.Offerings).Returns(new List<CourseOffering>() {
                offering1
            });
            
            var semester = "Spring 2021";
            var courseServices = new CourseServices(mockRepository.Object);

            //Act
            var result = courseServices.getCourseOfferingsBySemester(semester);

            // Assert
            var itemInList = Assert.Single(result);
            Assert.Equal(offering1, itemInList);
        }

        [Fact]
        public void GetCourseOfferingsBySemester_NoOfferingsFound_ExceptionThrown()
        {
            // Arrange
            var mockRepository = new Mock<ICourseRepository>();

            mockRepository.Setup(m => m.Offerings).Returns(new List<CourseOffering>() {
                
            });
            
            var semester = "Spring 2021";
            var courseServices = new CourseServices(mockRepository.Object);

            // Act/Assert
            Assert.Throws<Exception>(() => courseServices.getCourseOfferingsBySemester(semester));
        }

        //user story 4 getCourseOfferingsBySemesterAndDept
        [Fact]
        public void GetCourseOfferingsBySemesterAndDept_MultipleOfferingsFound_MultipleOfferingsReturned()
        {
            // Arrange
            var course = new Course() {
                Name= "ARTD 201",
                Title="graphic design",
                Credits=3.0,
                Description="graphic design descr",
                Department = "ARTS"
            };

            var mockRepository = new Mock<ICourseRepository>();
            mockRepository.Setup(m => m.Courses).Returns(new List<Course> {
                course});

            var offering1 = new CourseOffering() {
                    Section = "1",
                    Semester = "Spring 2021",
                    TheCourse = course
                };
            var offering2 = new CourseOffering() {
                    Section = "2",
                    Semester = "Spring 2021",
                    TheCourse = course
                };
            mockRepository.Setup(m => m.Offerings).Returns(new List<CourseOffering>() {
                offering1, offering2
            });

            var courseServices = new CourseServices(mockRepository.Object);
            var dept = "ARTS";
            var semester = "Spring 2021";

            //Act
            var result = courseServices.getCourseOfferingsBySemesterAndDept(semester, dept);

            // Assert
            Assert.Contains(result, o =>
                o.Section == "1" &&
                o.Semester == "Spring 2021" &&
                o.TheCourse.Name == course.Name &&
                o.TheCourse.Department == "ARTS");

            Assert.Contains(result, o =>
                o.Section == "2" &&
                o.Semester == "Spring 2021" &&
                o.TheCourse.Name == course.Name &&
                o.TheCourse.Department == "ARTS");
        }
        [Fact]
        public void GetCourseOfferingsBySemesterAndDept_OneOfferingFound_OneOfferingReturned()
        {
            // Arrange
            var course = new Course() {
                Name= "ARTD 201",
                Title="graphic design",
                Credits=3.0,
                Description="graphic design descr",
                Department = "ARTS"
            };

            var mockRepository = new Mock<ICourseRepository>();
            mockRepository.Setup(m => m.Courses).Returns(new List<Course> {
                course});

            var offering1 = new CourseOffering() {
                    Section = "1",
                    Semester = "Spring 2021",
                    TheCourse = course
                };
            mockRepository.Setup(m => m.Offerings).Returns(new List<CourseOffering>() {
                offering1
            });

            var courseServices = new CourseServices(mockRepository.Object);
            var dept = "ARTS";
            var semester = "Spring 2021";

            //Act
            var result = courseServices.getCourseOfferingsBySemesterAndDept(semester, dept);

            //Assert
            var itemInList = Assert.Single(result);
            Assert.Equal(offering1, itemInList);
            
        }
        [Fact]
        public void GetCourseOfferingsBySemesterAndDept_NoOfferingsFound_ExceptionThrown()
        {
            // Arrange

            var mockRepository = new Mock<ICourseRepository>();
            mockRepository.Setup(m => m.Courses).Returns(new List<Course> {
                });

            mockRepository.Setup(m => m.Offerings).Returns(new List<CourseOffering>() {
                
            });

            var courseServices = new CourseServices(mockRepository.Object);
            var dept = "ARTS";
            var semester = "Spring 2021";

            // Act/Assert
            Assert.Throws<Exception>(() => courseServices.getCourseOfferingsBySemesterAndDept(semester, dept));
        }


    
        private List<Course> GetTestCourses()
        {
            return new List<Course>(){
            new Course() {
                Name="ARTD 201",
                Title="graphic design",
                Credits=3.0,
                Description="graphic design descr",
                Department="ARTS"

            },
            new Course() {
                Name="ARTS 101",
                Title="art studio",
                Credits=3.0,
                Description="studio descr",
                Department="ARTS"

            }
            };
        }

    }
}
