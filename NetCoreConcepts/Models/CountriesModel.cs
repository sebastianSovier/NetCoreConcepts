﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreConcepts.Models
{
    public class CountriesModel
    {
        public string name { get; set; }
        public string capital { get; set; }
        public string region { get; set; }
        public string population { get; set; }

    }
    public class PaisesModel {
        public Int64 pais_id { get; set; }
        public string nombre_pais { get; set; }
        public string capital { get; set; }
        public string region { get; set; }
        public string poblacion { get; set; }
    }
}
