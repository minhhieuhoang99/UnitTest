using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using MVC_2.Services;
using MVC_2.Models;
namespace MVC_2.Services
{
    public class Service : IService
    {
        private readonly string _dataFile = @"Data\data.xml";
        private readonly XmlSerializer _serializer = new XmlSerializer(typeof(HashSet<PersonModel>));
        public HashSet<PersonModel> persons { get; set; }
        public Service()
        {
            persons = new HashSet<PersonModel>() {
                    new PersonModel{id=1,firstName="hieu1",lastName="hoang1",dateOfBirth = new DateTime (2000,08,09),gender="nam",phoneNumber=123456789,birthPlace="HN",age=10,isGraduated=true,email="hieu@gmai.com"},
                    new PersonModel{id=2,firstName="hieu2",lastName="hoang2",dateOfBirth = new DateTime (2000,08,09),gender="nu",phoneNumber=123456789,birthPlace="HCM",age=10,isGraduated=true,email="hieu@gmai.com"},
                    new PersonModel{id=3,firstName="hieu3",lastName="hoang3",dateOfBirth = new DateTime (2000,08,09),gender="nam",phoneNumber=123456789,birthPlace="QN",age=10,isGraduated=true,email="hieu@gmai.com"},
                    new PersonModel{id=4,firstName="hieu4",lastName="hoang4",dateOfBirth = new DateTime (2000,08,09),gender="nu",phoneNumber=123456789,birthPlace="TB",age=10,isGraduated=true,email="hieu@gmai.com"},
                    new PersonModel{id=5,firstName="hieu5",lastName="hoang5",dateOfBirth = new DateTime (2000,08,09),gender="nam",phoneNumber=123456789,birthPlace="BA",age=10,isGraduated=true,email="hieu@gmai.com"},
                };
        }
        public PersonModel[] GetAll() => persons.ToArray();
        public PersonModel Get(int id) => persons.FirstOrDefault(b => b.id == id);
        public bool Add(PersonModel person) => persons.Add(person);
        public PersonModel Create()
        {
            var max = persons.Max(b => b.id);
            var b = new PersonModel()
            {
                id = max + 1,
            };
            return b;
        }
        public bool Update(PersonModel person)
        {
            var b = Get(person.id);
            return b != null && persons.Remove(b) && persons.Add(person);
        }
        public bool Delete(int id)
        {
            var b = Get(id);
            return b != null && persons.Remove(b);
        }
        public void SaveChanges()
        {
            using var stream = File.Create(_dataFile);
            _serializer.Serialize(stream, persons);
        }

    }
}
