using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyAuthServer.Core.UnitOfWork;
using UdemyAuthServer.Data.Context;

namespace UdemyAuthServer.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public void Commit()
        {
           _context.SaveChanges();                
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
