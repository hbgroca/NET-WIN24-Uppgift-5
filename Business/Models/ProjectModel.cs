namespace Business.Models
{
    public class ProjectModel
    {
        public Guid Id { get; set; }
        public string? ImageUrl { get; set; }
        public string ProjectName { get; set; } = null!;

        public string? Description { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateOnly CreateDate { get; set; }
        public DateOnly UpdateDate { get; set; }

        public bool IsCompleted { get; set; }

        public decimal Budget { get; set; }

        public ClientModel Client { get; set; } = null!;

        public List<MemberModel> Members { get; set; } = [];





        // Calculate time left
        public string TimeLeft()
        {
            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            if(IsCompleted)
                return "Completed";

            if(EndDate < currentDate)
                return "Overdue";

            // Calculate weeks, days or hours left
            var daysLeft = EndDate.DayNumber - currentDate.DayNumber;

            if (daysLeft == 0)
                return "Today";

            if (daysLeft > 7)
            {
                var weeksLeft = daysLeft / 7;
                if(weeksLeft == 1)
                    return $"{weeksLeft} week left";

                return $"{weeksLeft} weeks left";
            }

            return $"{daysLeft} days left";
        }

        public string TimeLeftBgColor()
        {
            if (IsCompleted)
                return "IsCompleted";

            if(EndDate.DayNumber - DateOnly.FromDateTime(DateTime.Now).DayNumber <= 7)
                return "timesAlmostUpYouNeedToHurry";

            return "";
        }
    }
}
