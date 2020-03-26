namespace Patlus.Common.UseCase.Notifications
{
    public class DeltaValue
    {
        public readonly object OldValue;
        public readonly object NewValue;
        public DeltaValue(object oldValue, object newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
