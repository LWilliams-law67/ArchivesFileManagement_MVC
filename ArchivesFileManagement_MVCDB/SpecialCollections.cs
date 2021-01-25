﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ArchivesFileManagement_MVCDB
{
    public partial class SpecialCollections
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("TypeID")]
        public int TypeId { get; set; }
        [Required]
        [StringLength(50)]
        public string Location { get; set; }
        [Required]
        [StringLength(50)]
        public string AccessionNo { get; set; }
        [Required]
        public byte[] Attachment { get; set; }
        [Required]
        [StringLength(50)]
        public string UploadedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UploadDate { get; set; }
        [StringLength(50)]
        public string EditBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EditDate { get; set; }
        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [ForeignKey(nameof(TypeId))]
        [InverseProperty(nameof(SpecialCollectionsTypes.SpecialCollections))]
        public virtual SpecialCollectionsTypes Type { get; set; }
    }
}