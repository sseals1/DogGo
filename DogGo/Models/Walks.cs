﻿using System;

namespace DogGo.Models
{
    public class Walks
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Duration {  get; set; }
        public int TotalWalkTime { get; set; }
        public Dog Dog { get; set; }
        public Owner Owner { get; set; }    
    }
}
