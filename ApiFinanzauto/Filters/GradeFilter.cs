namespace ApiFinanzauto.Filters
{
    public class GradeFilter
    {
        public int? StudentId { get; set; }
        public int? CourseId { get; set; }
        public decimal? GradeValueMin { get; set; }
        public decimal? GradeValueMax { get; set; }
    }
}
