﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Model
{
    [Table("Categories")]
    public class Category
    {
        [Key]
        public string Name { get; set; }


    }
}
