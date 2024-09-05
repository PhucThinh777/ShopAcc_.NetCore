using ShopAcc.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopAcc.Models
{
    public class NickModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập level acc")]
        public int LevelAcc {  get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mô tả")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tổng tướng")]
        public int TongTuong { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tổng trấn")]
        public int TongTran { get; set; }
        public string TranNoiBat { get; set; }
        public string TuongNoiBat { get; set; }
        public string AnhBia { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập giá")]
        public decimal Price { get; set; }
        public int CategoryId {  get; set; }
        public int CharacterId { get; set; }
        public int ServerId { get; set; }
        public CategoryModel Category {  get; set; }
        public CharacterModel Character { get; set; }
        public ServerModel Server { get; set; }

        [NotMapped]
        [FileExtension("jpg", "jpeg", "png", ErrorMessage = "Chỉ chấp nhận các định dạng: jpg, jpeg, png")]

        public IFormFile[]? UploadTranNoiBat { get; set; }
        [NotMapped]
        [FileExtension("jpg", "jpeg", "png", ErrorMessage = "Chỉ chấp nhận các định dạng: jpg, jpeg, png")]

        public IFormFile[]? UploadTuongNoiBat { get; set; }
        [NotMapped]
        [FileExtension("jpg", "jpeg", "png", ErrorMessage = "Chỉ chấp nhận các định dạng: jpg, jpeg, png")]
        public IFormFile? UploadAnhBia { get; set; }
    }
}
