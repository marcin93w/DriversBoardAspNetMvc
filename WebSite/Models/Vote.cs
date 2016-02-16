﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.WebSite.Models
{
    public abstract class Vote
    {
        [Key]
        public int Id { set; get; }
        [Index("IX_Rate", 2, IsUnique = true)]
        public ApplicationUser User { get; set; }
        public bool Positive { get; set; }


        //public abstract IVotable Votable { get; set; }
    }
}
