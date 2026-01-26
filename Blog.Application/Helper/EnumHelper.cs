using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Helper
{
    public enum ActivityType
    {
        PostPublished,
        CategoryAdded,
        BookmarkAdded
    }
    public enum IconLabel
    {
       bookmark,
       Tag,
       Plus
    }
    public static class EnumHelper
    {
        public static string GetTypeActivity(ActivityType type)
        {
            var result = type switch
            {
                ActivityType.PostPublished => "Published new post",
                ActivityType.CategoryAdded => "Added new category",
                ActivityType.BookmarkAdded => "Added bookmark",
                _ => "N/A"              
            };
            return result;
        }

        public static string GetIconLabel(IconLabel type)
        {
            var result = type switch
            {
                IconLabel.bookmark => "bookmark",
                IconLabel.Plus => "plus",
                IconLabel.Tag => "tag",
                _ => "N/A"
            };
            return result;
        }
    }
}
