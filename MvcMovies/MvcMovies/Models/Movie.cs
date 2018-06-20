using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovies.Models
{
    public class Movie
    {
        public int ID { get; set; }


        [StringLength(60, MinimumLength = 3, ErrorMessage ="最少3位最大60位")]
        public string Title { get; set; }


        [Display(Name ="Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Genre { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        [Range(1,100)]
        public decimal Price { get; set; }
        /// <summary>
        /// 评分
        /// </summary>
        public string Rating { get; set; }
    }
}

