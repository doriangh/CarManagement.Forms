using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Interfaces
{
    public interface ICarsSoldService
    {
        Task<List<CarsSold>> GetAll(bool force = false);
        Task<CarsSold> GetById(int id, bool force = false);
        Task Update(CarsSold carsSold);
        Task<List<CarsSold>> GetByUserId(int userId, bool force = false);
        Task Add(CarsSold carsSold);
        Task Delete(int id);
        Task<CarsSold> GetByCarId(int carId, bool force = false);
    }
}
