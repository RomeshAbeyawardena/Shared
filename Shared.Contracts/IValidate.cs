using System;

namespace Shared.Contracts
{
    public interface IValidate<T>
    {
        IValidate<T> IsNotNull<TMember>(Func<T, TMember> member);
        IValidate<T> IsInRange<TMember>(Func<T, TMember> member, TMember minimumValue, 
            TMember maximumValue, Func<TMember, TMember, TMember, bool> isInRangeComparer);
        IValidate<T> IsValid<TMember>(Func<T, TMember> member, TMember value, Func<TMember, TMember, bool> equalityComparer);
    }
}