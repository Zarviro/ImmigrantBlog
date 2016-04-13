using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImmigrantBlog.WEB.Models
{
    public class PostModel
    {
        [HiddenInput(DisplayValue=false)]
        public int Id { get; set; }

        [Required(ErrorMessage="*Поле не должно быть пустым")]
        //[StringLength(100, MinimumLength=3, ErrorMessage="Длина заголовка должна быть от 3 до 100 символов")]
        [Display(Name="Заголовок:")]
        public string Title { get; set; }

        [Required(ErrorMessage="*Поле не должно быть пустым")]
        //[StringLength(3000, MinimumLength=50, ErrorMessage="*Краткое описание должно содержать от 50 до 3000 символов")]
        [Display(Name="Краткое описание:")]
        [DataType(DataType.MultilineText)]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage="*Поле не должно быть пустым")]
        //[StringLength(100000, MinimumLength=1000, ErrorMessage="*Текст поста должен содержать от 1000 до 100000 символов")]
        [Display(Name="Текст:")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage="*Поле не должно быть пустым")]
        //[StringLength(1000, MinimumLength=3, ErrorMessage="*Метки должены содержать от 3 до 1000 символов")]
        [Display(Name="Метки:")]
        public string Meta { get; set; }

        public bool IsPublished { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime PostedOn { get; set; }

        [DataType(DataType.Date)]
        [Display(Name="Последнее редактирование:")]
        public DateTime? Modified { get; set; }


        [Required(ErrorMessage="*Укажите раздел")]
        [Display(Name="Раздел:")]
        public CategoryModel Category { get; set; }

        public UserModel User { get; set; }


        [Required(ErrorMessage="*Укажите от 1 до 5 тэгов")]
        [Display(Name="Тэги:")]
        public ICollection<TagModel> Tags { get; set; }

        public ICollection<CommentModel> Comments { get; set; }
        public ICollection<PostCountHitModel> CountHits { get; set; }

        // not in model
        public IEnumerable<IGrouping<int?, CommentModel>> GroupComments
        {
            get { return Comments.GroupBy(c => c.ResponsToCommentId); }
        }
    }
}