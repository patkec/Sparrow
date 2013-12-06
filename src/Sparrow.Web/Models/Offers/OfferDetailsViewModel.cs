﻿using System;
using Sparrow.Domain.Models;

namespace Sparrow.Web.Models.Offers
{
    public class OfferDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string OwnerName { get; set; }
        public string CustomerName { get; set; }
        public OfferStatus Status { get; set; }
        public double Discount { get; set; }
        public decimal OfferPrice { get; set; }
    }
}