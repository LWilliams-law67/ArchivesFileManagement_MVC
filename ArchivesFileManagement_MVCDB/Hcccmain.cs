﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ArchivesFileManagement_MVCDB
{
    [Table("HCCCMAIN")]
    public partial class Hcccmain
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        public int Type { get; set; }
        [Required]
        [StringLength(250)]
        public string CaseNo { get; set; }
        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        [Required]
        [StringLength(250)]
        public string Location { get; set; }
        [Required]
        [StringLength(250)]
        public string SessionNo { get; set; }
        [Required]
        public byte[] Attachment { get; set; }
        [Required]
        [StringLength(250)]
        public string UploadedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UploadedDate { get; set; }
        [StringLength(250)]
        public string EditedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EditedDate { get; set; }
        [Required]
        [StringLength(250)]
        public string DestructionDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreationDate { get; set; }

        [ForeignKey(nameof(Type))]
        [InverseProperty(nameof(Hccccasetypes.Hcccmain))]
        public virtual Hccccasetypes TypeNavigation { get; set; }
    }
}