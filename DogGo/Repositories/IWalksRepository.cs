using DogGo.Models;
using System;
using System.Collections.Generic;
using DogGo.Controllers;
using DogGo.Repositories;

namespace DogGo.Repositories
{
    public interface IWalksRepository
    {
        List<Walks> GetWalksByWalkerId(int id); 
    }
}
