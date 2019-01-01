namespace Prevea.Model.ViewModel
{
    public class RiskEvaluationViewModel
    {
        public int Id { get; set; }
        public int WorkStationId { get; set; }
        public string WorkStationName  { get; set; }
        public string WorkStationDescription { get; set; }
        public int DeltaCodeId { get; set; }
        public string DuplicateDeltaCodeId { get; set; }
        public string DeltaCodeName { get; set; }
        public string DeltaCodeDescription { get; set; }
        public int Probability { get; set; }
        public string ProbabilityName { get; set; }
        public int Severity { get; set; }
        public string SeverityName { get; set; }
        public int RiskValue { get; set; }
        public string RiskValueName { get; set; }
        public int Priority { get; set; }
        public string PriorityName { get; set; }
        public string RiskDetected { get; set; }
    }
}
