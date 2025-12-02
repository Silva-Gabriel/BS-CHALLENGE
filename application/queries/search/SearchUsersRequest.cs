using Azure.Core;
using domain.enums;
using MediatR;

namespace application.queries.search
{
    public class SearchUsersRequest : IRequest<SearchUsersResponse>
    {
        public string? Username { get; set; }
        public ClientStatus? Status { get; set; }
    }
}