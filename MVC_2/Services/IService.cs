using MVC_2.Models;

namespace MVC_2.Services
{
    public interface IService
    {
        PersonModel[] GetAll();
        PersonModel Get(int id);
        bool Add(PersonModel person);
        PersonModel Create();
        bool Update(PersonModel person);
        bool Delete(int id);
        void SaveChanges();
    }
}