﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Person
    {
        [Key]
        public Guid PersonID { get; set; }
        [StringLength(50)]
        public string? PersonName { get; set; }
        [StringLength(50)]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [StringLength(10)]
        public string? Gender { get; set; }
        public Guid? CountryID { get; set; }
        [StringLength(200)]
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public string? TIN { get; set; }

        [ForeignKey("CountryID")]
        public Country? Country { get; set; }


        public override string ToString()
        {
            return $"Person ID: {PersonID}, Person Name: {PersonName}, Email: {Email}, Date of Birth:{DateOfBirth?.ToString("MMM dd yyyy")}, Gender: {Gender}, Country ID: {CountryID}, Country: {Country}, Address: {Address}, Receive News Letters: {ReceiveNewsLetters}"; 
        }
    }
}
