using System;
using Microsoft.Extensions.Options;

namespace MySite.Shared.Utilities.Helpers.Abstract
{
    public interface IWritableOptions<out T> : IOptionsSnapshot<T> where T : class, new()
    {
        void Update(Action<T> applyChanges);
    }
}
