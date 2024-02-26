using DiffService.src.DiffService.Api;
using DiffService.src.models;
using DiffService.src.services;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace DiffService.Tests.DiffServiceTests
{
    public class UnitTests
    {
        private readonly IDiffService _diffService;

        public UnitTests(IDiffService diffService)
        {
            _diffService = diffService;
        }

        [Fact]
        public void SuccessfullyCreatedRight()
        {
            string ID = "1";
            DiffData data = new DiffData { Data = "AAAAAA==" };

            var result = _diffService.Right(ID, data);

            Assert.True(true == result);
        }

        [Fact]
        public void SuccessfullyCreatedLeft()
        {
            string ID = "1";
            DiffData data = new DiffData { Data = "AQAQAQ==" };

            var result = _diffService.Left(ID, data);

            Assert.True(true == result);
        }
    }
}
