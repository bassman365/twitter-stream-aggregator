﻿using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TwitterStreamAggregator.Clients
{
    public interface ITwitterClient
    {
        Task<Stream?> GetSampleStreamAsync(CancellationToken cancellationToken = default);
    }
}
