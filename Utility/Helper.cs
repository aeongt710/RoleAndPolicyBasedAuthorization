using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityUserManagement.Utility
{
    public class Helper
    {
        public static string Admin="Admin";
        public static string Teacher = "Teacher";
        public static string Student = "Student";
        public static List<SelectListItem> GetRolesDropDownList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem{Value=Helper.Admin, Text=Admin},
                new SelectListItem{Value=Helper.Teacher, Text=Teacher},
                new SelectListItem{Value=Helper.Student, Text=Student}
            };
        }
    }
}
