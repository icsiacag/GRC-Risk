using MediatR;
using GRC.Application.Common.Models;

namespace GRC.Application.Common.Models;

public interface ICommand<T> : IRequest<Result<T>>
{
}

public interface ICommand : IRequest<Result>
{
}
