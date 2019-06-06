using System.Collections.Generic;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Interfaces
{
    public interface ISessionRepository
    {
        void Add(Session session);
        List<Session> GetAll();
        Session GetSession();
        void Update(Session session);
        void Delete();
    }
}