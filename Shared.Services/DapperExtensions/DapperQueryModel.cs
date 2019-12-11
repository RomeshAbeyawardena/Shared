using Remotion.Linq;
using Remotion.Linq.Clauses;

namespace Shared.Services.DapperExtensions
{
    public class DapperQueryModelVisitor : QueryModelVisitorBase
    {
        public override void VisitMainFromClause(MainFromClause fromClause, QueryModel queryModel)
        {
            base.VisitMainFromClause(fromClause, queryModel);
        }
    }
}