using Skechers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skechers.Areas.Manage.ViewModels
{
    public class OfferViewModel
    {
        public List<Offer>  Offers { get; set; }
        public PagenatedList<Offer> PagenationOffers { get; set; }
    }
}
