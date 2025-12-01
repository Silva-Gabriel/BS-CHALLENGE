using MediatR;

namespace application.queries.search
{
    public class SearchUsersRequest : IRequest<SearchUsersResponse>
    {
        public string? Username { get; set; }
    }
}