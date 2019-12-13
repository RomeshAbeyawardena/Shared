using System;
using System.Threading.Tasks;

namespace Shared.Contracts
{
    public interface IValidate<T>
    {
        IValidate<T> Regex(Func<T, string> getMember, string regexPattern);
        IValidate<T> IsNotNull<TMember>(Func<T, TMember> member);
        IValidate<T> IsInRange<TMember>(Func<T, TMember> member, TMember minimumValue, 
            TMember maximumValue, Func<TMember, TMember, TMember, bool> isInRangeComparer);
        IValidate<T> IsValid<TMember>(Func<T, TMember> member, TMember value, Func<TMember, TMember, bool> equalityComparer);
        IValidate<T> IsDuplicateEntry<TMember, TService>(Func<T, TMember> member, Func<TService, TMember, bool> checkDuplicateServiceComparer);
        Task<IValidate<T>> IsDuplicateEntryAsync<TMember, TService>(Func<T, TMember> member, Func<TService, TMember, Task<bool>> checkDuplicateServiceComparer);
    }
}