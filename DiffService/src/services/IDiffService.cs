using DiffService.src.models;
using Microsoft.AspNetCore.Mvc;

namespace DiffService.src.services
{
    public interface IDiffService
    {
        public Dictionary<string, (byte[], byte[])> DataStore { get; }
        public DiffResult Diff(string ID);

        public bool Left(string ID, [FromBody] DiffData diffData);

        public bool Right(string ID, [FromBody] DiffData diffData);
    }
}