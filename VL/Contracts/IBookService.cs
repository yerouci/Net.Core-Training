using Entities;
using Entities.Models;
using Entities.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VL.Contracts
{
    public interface IBookService
    {
        Task<Object> GetBooks(BookParameters bookParameters);
    }
}
