﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ArchivesFileManagement_MVCDB
{
    [Table("HCCCCASETYPES")]
    public partial class Hccccasetypes
    {
        public Hccccasetypes()
        {
            Hcccmain = new HashSet<Hcccmain>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Description { get; set; }
        public bool Active { get; set; }

        [InverseProperty("TypeNavigation")]
        public virtual ICollection<Hcccmain> Hcccmain { get; set; }
    }
}