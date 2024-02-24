using DiffService.src.DiffService.Api;
using DiffService.src.models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace DiffService.Tests.DiffServiceTests
{
    public class UnitTests
    {
        private readonly DiffController _controller;

        public UnitTests()
        {
            _controller = new DiffController();
        }

        [Fact]
        public void SuccessfullyCreatedRight()
        {
            string ID = "1";
            DiffData data = new DiffData { Data = "AAAAAA==" };

            var result = _controller.Right(ID, data) as StatusCodeResult;

            Assert.True(201 == result.StatusCode);
        }

        [Fact]
        public void BadCompare()
        {
            string ID = "1";
            var result = _controller.Diff(ID) as StatusCodeResult;

            Assert.True(404 == result.StatusCode);
        }
    }
}
