
using AutoMapper;
using CRUD_Practice.Controllers;
using CRUD_Practice.Models;
using CRUD_Practice.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDPractice.Tests.Controllers
{
    public class PersonControllerTests
    {
        private readonly Mock<IEntityRepository<Person>> _entityRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PersonController _personController;

        public PersonControllerTests()
        {
            _entityRepositoryMock = new Mock<IEntityRepository<Person>>();
            _mapperMock = new Mock<IMapper>();
            _personController = new PersonController(_entityRepositoryMock.Object, _mapperMock.Object);
        }

        private Person[] personTestList = new Person[]
            {
                new Person { Id = 1, Name = "Nurbaki", Surname = "Bozkurt", Age = 25 },
                new Person { Id = 2, Name = "Faruk", Surname = "Çelik", Age = 25 },
                new Person { Id = 3, Name = "Kerem", Surname = "Solmaz", Age = 42 }
            };


        [Fact]
        public void GetAllPersons_Returns_Ok()
        {
            //Arrange
            _entityRepositoryMock.Setup(repository => repository.GetAll()).Returns(personTestList);

            //Act
            var result = _personController.GetAllPersons();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetAllPersons_Throws_Exception()
        {
            //Arrange
            _entityRepositoryMock.Setup(repository => repository.GetAll()).Throws(new Exception());

            //Act
            var result = _personController.GetAllPersons();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GetPerson_Returns_Ok()
        {
            //Arrange
            _entityRepositoryMock.Setup(repository => repository.GetById(1)).Returns(personTestList[0]);

            //Act
            var result = _personController.GetPerson(1);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }


        [Fact]
        public void GetPerson_Throws_Exception()
        {
            //Arrange
            _entityRepositoryMock.Setup(repository => repository.GetById(2)).Throws(new Exception());

            //Act
            var result = _personController.GetPerson(2);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GetPerson_Returns_NotFound()
        {
            //Arrange
            _entityRepositoryMock.Setup(repository => repository.GetById(10)).Returns((Person)null);

            //Act
            var result = _personController.GetPerson(10);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void PostPerson_Returns_Ok()
        {
            //Arrange
            PersonDto personToBeAddedDto = new PersonDto() { Name = "Test", Surname = "Test", Age = 1 };
            Person personToBeAdded = personTestList[0];
            _mapperMock.Setup(mapper => mapper.Map<Person>(personToBeAddedDto)).Returns(personToBeAdded);
            _entityRepositoryMock.Setup(repository => repository.Add(personToBeAdded)).Verifiable();

            //Act
            var result = _personController.PostPerson(personToBeAddedDto);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void PostPerson_Throws_Exception()
        {
            //Arrange
            PersonDto personToBeAddedDto = new PersonDto() { Name = "Test", Surname = "Test", Age = 1 };
            Person personToBeAdded = personTestList[0];
            _mapperMock.Setup(mapper => mapper.Map<Person>(personToBeAddedDto)).Throws(new Exception());
            //_entityRepositoryMock.Setup(repository => repository.Add(personToBeAdded)).Throws(new Exception());

            //Act
            var result = _personController.PostPerson(personToBeAddedDto);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void PutPerson_Returns_Ok()
        {
            //Arrange
            PersonDto personDto = new PersonDto() { Name = "Test", Surname = "Test", Age = 1 };
            _entityRepositoryMock.Setup(repository => repository.GetById(2)).Returns(personTestList[1]);
            _entityRepositoryMock.Setup(repository => repository.Update(personTestList[1])).Verifiable();

            //Act
            var result = _personController.PutPerson(2, personDto);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

        }

        [Fact]
        public void PutPerson_Returns_NotFound()
        {
            //Arrange
            PersonDto personDto = new PersonDto() { Name = "Test", Surname = "Test", Age = 1 };
            _entityRepositoryMock.Setup(repository => repository.GetById(10)).Returns((Person)null);

            //Act
            var result = _personController.PutPerson(10, personDto);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }


        [Fact]
        public void PutPerson_Throws_Exception()
        {
            //Arrange
            PersonDto personDto = new PersonDto() { Name = "Test", Surname = "Test", Age = 1 };
            //_entityRepositoryMock.Setup(repository => repository.GetById(2)).Throws(new Exception());

            _entityRepositoryMock.Setup(repository => repository.GetById(2)).Returns(personTestList[1]);
            _entityRepositoryMock.Setup(repository => repository.Update(personTestList[1])).Throws(new Exception());


            //Act
            var result = _personController.PutPerson(2, personDto);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }


        [Fact]
        public void DeletePerson_Returns_Ok()
        {
            //Arrange     
            _entityRepositoryMock.Setup(repository => repository.GetById(1)).Returns(personTestList[0]);
            _entityRepositoryMock.Setup(repository => repository.Delete(1)).Verifiable();

            //Act
            var result = _personController.DeletePerson(1);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void DeletePerson_Returns_NotFound()
        {
            //Arrange
            _entityRepositoryMock.Setup(repository => repository.GetById(10)).Returns((Person)null);
            

            //Act
            var result = _personController.DeletePerson(10);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }


        [Fact]
        public void DeletePerson_Throws_Exception()
        {
            //Arrange
            //_entityRepositoryMock.Setup(repository => repository.GetById(2)).Throws(new Exception());
            _entityRepositoryMock.Setup(repository => repository.GetById(2)).Returns(personTestList[1]);
            _entityRepositoryMock.Setup(repository => repository.Delete(2)).Throws(new Exception());


            //Act
            var result = _personController.DeletePerson(2);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }











    }
}
