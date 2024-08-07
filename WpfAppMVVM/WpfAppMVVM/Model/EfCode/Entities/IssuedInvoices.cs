﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    internal class IssuedInvoices
    {
        public int IssuedInvoicesId { get; set; }
        public int TransportationId { get; set; }
        public int AccountNumber { get; set; }
        public DateTime AccountDate { get; set; }
        public Transportation Transportation { get; set; }
    }
}
