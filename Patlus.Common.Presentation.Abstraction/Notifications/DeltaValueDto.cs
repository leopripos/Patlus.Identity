namespace Patlus.Common.Presentation.Notifications
{
    public class DeltaValueDto : IDto
    {
        public object OldValue { get; set; }
        public object NewValue { get; set; }
        public DeltaValueDto() { }

        public DeltaValueDto(object oldValue, object newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
