using Microsoft.AspNetCore.Mvc;
using Time_Table_Generator.Models;

namespace Time_Table_Generator.Controllers
{
    public class TimeTableController : Controller
    {
        #region Action Methods

        [HttpGet]
        public IActionResult Index()
        {
            UserInputModel model = new UserInputModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult GenerateTimeTable(UserInputModel model)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(model.CommaSeparatedSubjectNames) && !string.IsNullOrWhiteSpace(model.CommaSeparatedSubjectHours))
            {
                // Parse comma-separated subject names
                var subjectNames = model.CommaSeparatedSubjectNames.Split(',').Select(s => s.Trim()).ToList();

                // Parse comma-separated subject hours
                var subjectHours = model.CommaSeparatedSubjectHours.Split(',').Select(s => int.TryParse(s.Trim(), out int h) ? h : 0).ToList();

                // Verify total hours
                int totalRequiredHours = subjectHours.Sum();

                // Check timetable capacity
                int totalSlots = model.NoOfWorkingDays * model.NoOfSubjectsPerDay;

                //Build a subject pool => repeated subject names according to their required hours
                var subjectPool = new List<string>();
                for (int i = 0; i < subjectNames.Count; i++)
                {
                    for (int j = 0; j < subjectHours[i]; j++)
                    {
                        subjectPool.Add(subjectNames[i]);
                    }
                }

                Shuffle(subjectPool);

                //Fill a 2D array: rows = NoOfSubjectsPerDay, cols = NoOfWorkingDays
                var timetable2D = new string[model.NoOfSubjectsPerDay, model.NoOfWorkingDays];
                int index = 0;
                for (int row = 0; row < model.NoOfSubjectsPerDay; row++)
                {
                    for (int col = 0; col < model.NoOfWorkingDays; col++)
                    {
                        timetable2D[row, col] = subjectPool[index];
                        index++;
                    }
                }

                //Convert 2D -> a list of row-strings to display in the view
                var timetableRows = new List<string>();
                for (int row = 0; row < model.NoOfSubjectsPerDay; row++)
                {
                    var rowSubjects = new List<string>();
                    for (int col = 0; col < model.NoOfWorkingDays; col++)
                    {
                        rowSubjects.Add(timetable2D[row, col]);
                    }
                    // e.g. "Gujarati | Maths | Science | Science | Gujarati"
                    timetableRows.Add(string.Join(" | ", rowSubjects));
                }
                model.DynamicTimeTable = timetableRows;

                // Return a view that shows the final timetable
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        #endregion

        #region Private Methods

        // Fisher-Yates shuffle helper
        private void Shuffle<T>(List<T> list)
        {
            var rand = new Random();
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
        
        #endregion
    }
}
