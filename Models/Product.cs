using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_app.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdProduct { get; set; }

        [Display(Name = "Enter Product Code")]
        [StringLength(20)]
        [Required(ErrorMessage = "{0} is required")]
        public string ProductCode { get; set; }

        [Display(Name = "Enter Product Type")]
        [StringLength(50)]
        [Required(ErrorMessage = "{0} is required")]
        public string ProductType { get; set; }

        [Display(Name = "Enter Product Description")]
        [StringLength(150)]
        public string Description { get; set; }

        [Display(Name = "Unit Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Required(ErrorMessage = "{0} is required")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Quantity in Stock")]
        [Required(ErrorMessage = "{0} is required")]
        public int QtyInStock { get; set; }

        [Display(Name = "Select an image.")]
        public string ImagePath { get; set; }

        [Display(Name = "Copy a link to your youtube video")]
        public string YoutubeVideo { get; set; }
    }
}