﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models
{
    public class Transfer : BaseEntity
    {
        //TODO: In BaseEntity 
        [Key]
        public int Id
        {
            get; set;
        }

        // TODO: Comment ist jetzt hier ein string haben aber eine Entity
        public string Comment

        {
            get; set;
        }
        [Required]
        public StorageLocation StorageLocation
        {
            get; set;
        }
        [Required]
        public User NotedBy
        {
            get; set;
        }
        public Pcb Pcb
        {
            get; set;
        }
    }
}
