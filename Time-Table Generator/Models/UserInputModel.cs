using System.ComponentModel.DataAnnotations;

namespace Time_Table_Generator.Models
{
    public class UserInputModel
    {
        #region Properties

        [Display(Name = "No. of Working Days")]
        public int NoOfWorkingDays { get; set; }

        [Display(Name = "No. of Subjects per Day")]
        public int NoOfSubjectsPerDay { get; set; }

        [Display(Name = "Total Hours for Week")]
        public int TotalHoursForWeek { get; set; }

        [Display(Name = "Subject Names")]
        public string CommaSeparatedSubjectNames { get; set; }

        [Display(Name = "Subject Hours")]
        public string CommaSeparatedSubjectHours { get; set; }

        public List<string> DynamicTimeTable { get; set; }

        #endregion
    }
}
