using Infrastructure.Entities;

namespace ASPNET_MVC.ViewModels;

public class CoursePageViewModel
{
    public IEnumerable<CourseModel> Courses { get; set; } = [];
}
