using System.Threading;
using System.Threading.Tasks;

namespace Sum.Core
{
    public class SumHelper
    {
        public async Task<long> GetSumAsync(long end, CancellationToken cancellationToken)
        {
            GuardClauses.IsNonNegative(end);

            return await Task.Run(() => GetSum(end, cancellationToken), cancellationToken);
        }

        private long GetSum(long end, CancellationToken cancellationToken)
        {
            long sumResult = 0;
            for (int i = 0; i < end; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Thread.Sleep(50);
                sumResult += i;
            }

            return sumResult;        
        }
    }
}
