using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCFilmSatis.Models
{
    public class HomeViewModel
    {
        public List<Movie> Movies { get; set; }
        public List<Slider> Sliders { get; set; }
    }
}