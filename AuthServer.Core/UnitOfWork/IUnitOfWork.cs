using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.UnitOfWork
{
    public interface IUnitOfWork
    {
        // about for saveChanges, thats design pattern include transaction

        Task CommitAsync();
        void Commit();
    }
}
