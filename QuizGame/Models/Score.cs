namespace QuizGame.Models
{
    public class Score
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int TotalScore { get; set; }
    }
}
