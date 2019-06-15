using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Interfaces
{
    public interface ICarsSoldService
    {
        Task<List<CarsSold>> GetAll();
        Task<CarsSold> GetById(int id);
        Task Update(CarsSold carsSold);
        Task<List<CarsSold>> GetByUserId(int userId);
        Task Add(CarsSold carsSold);
        Task Delete(int id);
    }
}
