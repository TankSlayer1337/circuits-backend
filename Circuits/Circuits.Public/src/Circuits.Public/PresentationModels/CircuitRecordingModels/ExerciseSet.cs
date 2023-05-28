namespace Circuits.Public.PresentationModels.CircuitRecordingModels
{
    public class ExerciseSet
    {
        public int Index { get; init; } = 0;
        public string RepetitionSize { get; init; } = string.Empty; // string representation of int or time format depending on the RepetitionType
        public List<EquipmentInstance> EquipmentItems { get; init; } = new List<EquipmentInstance>();
    }
}
