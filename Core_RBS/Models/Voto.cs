﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Core_RBS.Models
{
    public class Voto
    {
        [Key]
        public long VotID { get; set; }
        [DefaultValue(-1)]
        public int Nota { get; set; }
        [Display(Name = "Comentario:")]
        public string Comentario { get; set; }
        public int CamId { get; set; }
        [Display(Name = "Data do Voto")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime DataVoto { get; set; }
        public Campanha Campanha { get; set; }
    }
}
