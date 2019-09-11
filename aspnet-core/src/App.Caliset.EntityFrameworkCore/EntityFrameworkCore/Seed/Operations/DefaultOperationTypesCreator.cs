using System;
using System.Collections.Generic;
using System.Text;

namespace App.Caliset.EntityFrameworkCore.Seed.Operations
{
    public class DefaultOperationTypesCreator
    {
        private readonly CalisetDbContext _context;

        public DefaultOperationTypesCreator(CalisetDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            ;
        }
    }
}
