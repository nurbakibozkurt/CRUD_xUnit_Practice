using CRUD_Practice.Data;
using CRUD_Practice.Models;
using CRUD_Practice.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDPractice.Tests.Repositories
{
    public class EntityRepositoryTests
    {
        private readonly Mock<AppDbContext> _appDbcontextMock;
        private readonly EntityRepository<Person> _entityRepository;

        public EntityRepositoryTests()
        {
            _appDbcontextMock = new Mock<AppDbContext>();
            _entityRepository = new EntityRepository<Person>(_appDbcontextMock.Object);
        }

        private List<Person> _mockData = new List<Person>()
        {       new Person { Id = 1, Name = "Nurbaki", Surname = "Bozkurt", Age = 25 },
                new Person { Id = 2, Name = "Faruk", Surname = "Çelik", Age = 25 },
                new Person { Id = 3, Name = "Kerem", Surname = "Solmaz", Age = 42 }
        };

        public AppDbContext GetMock(List<Person> mockList)
        {
            var queryable = mockList.AsQueryable();
            var mockDbSet = new Mock<DbSet<Person>>();
            mockDbSet.As<IQueryable<Person>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockDbSet.As<IQueryable<Person>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockDbSet.As<IQueryable<Person>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockDbSet.As<IQueryable<Person>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            mockDbSet.Setup(mockSet => mockSet.Add(It.IsAny<Person>())).Callback<Person>((person) => mockList.Add(person));
            mockDbSet.Setup(mockSet => mockSet.Remove(It.IsAny<Person>())).Callback<Person>((person) => mockList.Remove(person));
            _appDbcontextMock.Setup(x => x.Set<Person>()).Returns(mockDbSet.Object);
            _appDbcontextMock.Setup(x => x.Remove(It.IsAny<Person>())).Callback<Person>((person) => mockList.Remove(person));
            return _appDbcontextMock.Object;
        }



        [Fact]
        public void GetAll_Returns_AllResults()
        {
            //Arrange
            GetMock(_mockData);

            //Act
            var result = _entityRepository.GetAll();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void GetById_Returns_CorrectResult()
        {
            //Arrange
            int id = 1;
            GetMock(_mockData);
            _appDbcontextMock.Setup(dbMock => dbMock.Set<Person>().Find(id)).Returns(_mockData.Find(x => x.Id == id));

            //Act
            var result = _entityRepository.GetById(id);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Person>(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("Nurbaki", result.Name);
        }

        [Fact]
        public void GetById_Returns_Null()
        {
            //Arrange
            int id = 10;
            GetMock(_mockData);
            _appDbcontextMock.Setup(dbMock => dbMock.Set<Person>().Find(id)).Returns(_mockData.Find(x => x.Id == id));

            //Act
            var result = _entityRepository.GetById(id);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void Add_Adds_ItemToDbSet()
        {
            //Arrange
            int data_count_before_add = _mockData.Count;
            Person personToAdd = new Person() { Id = 4, Name = "AddTest", Surname = "AddTestSurname", Age = 1 };
            GetMock(_mockData);


            //Act
            _entityRepository.Add(personToAdd);

            _appDbcontextMock.Setup(dbMock => dbMock.Set<Person>().Find(personToAdd.Id)).Returns(_mockData.Find(x => x.Id == personToAdd.Id));
            var expectedPerson = _entityRepository.GetById(personToAdd.Id);
            //var expectedPerson = _appDbcontextMock.Object.Set<Person>().FirstOrDefault(x => x.Id == personToAdd.Id);


            //Assert
            Assert.NotNull(expectedPerson);
            Assert.IsType<Person>(expectedPerson);
            Assert.Equal(data_count_before_add + 1, _mockData.Count());
            Assert.Equal(personToAdd.Id, expectedPerson.Id);
            Assert.Equal(personToAdd.Name, expectedPerson.Name);
        }

        [Fact]
        public void Update_Updates_Item()
        {
            //Arrange
            GetMock(_mockData);
            int toBeUpdated_id = 2;
            _appDbcontextMock.Setup(dbMock => dbMock.Set<Person>().Find(toBeUpdated_id)).Returns(_mockData.Find(x => x.Id == toBeUpdated_id));
            Person personToUpdate = _entityRepository.GetById(toBeUpdated_id);
            personToUpdate.Age = 26;
            personToUpdate.Name = "Muhammed Faruk";

            //Act
            _entityRepository.Update(personToUpdate);
            var updatedPerson = _entityRepository.GetById(toBeUpdated_id);

            //Assert
            Assert.NotNull(updatedPerson);
            Assert.IsType<Person>(updatedPerson);
            Assert.Equal(personToUpdate.Id, updatedPerson.Id);
            Assert.Equal(personToUpdate.Name, updatedPerson.Name);
            Assert.Equal(26, updatedPerson.Age);
        }


        [Fact]
        public void Delete_Removes_Item()
        {
            //Arrange
            int data_count_before_delete = _mockData.Count;
            int toBeRemoved_id = 3;
            GetMock(_mockData);
            _appDbcontextMock.Setup(dbMock => dbMock.Set<Person>().Find(toBeRemoved_id)).Returns(_mockData.Find(x => x.Id == toBeRemoved_id));

            //Act
            _entityRepository.Delete(toBeRemoved_id);
            var expectedResult = _appDbcontextMock.Object.Set<Person>().FirstOrDefault(x => x.Id == toBeRemoved_id);


            //Assert
            Assert.Null(expectedResult);
            Assert.Equal(data_count_before_delete - 1, _mockData.Count);
        }


    }
}