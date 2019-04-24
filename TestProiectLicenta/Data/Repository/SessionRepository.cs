using System;
using System.Collections.Generic;
using System.Linq;
using TestProiectLicenta.Data.Interfaces;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Repository
{
    public class SessionRepository : ISessionRepository
    {
        readonly AppDbContext _context;

        public SessionRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Session session)
        {
            _context.Sessions.Add(session);
            _context.SaveChanges();
        }

        public void Delete()
        {
            foreach(var session in _context.Sessions)
            {
                _context.Sessions.Remove(session);
            }
        }

        public List<Session> GetAll()
        {
            return _context.Sessions.ToList();
        }

        public Session GetSession()
        {
            return _context.Sessions.FirstOrDefault();
        }

        public void Update(Session session)
        {
            throw new NotImplementedException();
        }
    }
}
