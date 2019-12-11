using System;

namespace Shared.Contracts
{
    public interface IValidate<T>
    {
        IValidate<T> IsNotNull<TMember>(Func<T, TMember> member);
        IValidate<T> IsInRange<TMember>(Func<T, TMember> member, Func<TMember, TMember, bool> isInRangeComparer);
        IValidate<T> IsEqual<TMember>(Func<T, TMember> member, Func<TMember, TMember, bool> equalityComparer);
    }
}