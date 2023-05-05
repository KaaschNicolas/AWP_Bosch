﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models
{
    public class Diagnose : BaseEntity
    {
        // TODO: Brauchen wir 650 Zeichen für den Endfehler? Bezeichnungen wie Hardware, Software etc. sollten ja kurz sein
        [Required]
        [Column(TypeName = "nvarchar(650)")]
        public string Name
        {
            get; set;
        }
        public List<Pcb> Pcbs
        {
            get; set;
        }

    }
}
