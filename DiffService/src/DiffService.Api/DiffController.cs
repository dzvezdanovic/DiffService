using DiffService.src.models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Xml;
using Serilog;

namespace DiffService.src.DiffService.Api
{
    [ApiController]
    [Route("v1/diff")]
    public class DiffController : ControllerBase
    {
        private static readonly Dictionary<string, (byte[], byte[])> dataStore;
        private readonly ILogger<DiffController> _logger;

        static DiffController()
        {
            dataStore = new Dictionary<string, (byte[], byte[])>();
        }

        [HttpPost]
        [Route("{ID}/left")]
        public IActionResult Left(string ID, [FromBody] DiffData diffData)
        {
            try
            {
                string dataString = diffData.Data;
                // Adding padding characters if necessary
                int padding = (4 - dataString.Length % 4) % 4;
                dataString += new string('=', padding);

                byte[] data = Convert.FromBase64String(dataString);
                if (!dataStore.TryGetValue(ID, out (byte[], byte[]) value))
                {
                    dataStore.Add(ID, (data, null));
                    //_logger.LogInformation($"Adding key {ID} to dataStore");
                    dataStore[ID] = (null, data);
                }
                else
                {
                    dataStore[ID] = (value.Item1, data);
                    //_logger.LogInformation($"Key {ID} already exists in dataStore. Updating value.");
                }

                return StatusCode(201, "Created");
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpPost]
        [Route("{ID}/right")]
        public IActionResult Right(string ID, [FromBody] DiffData diffData)
        {
            try
            {
                string dataString = diffData.Data;
                // Adding padding characters if necessary
                int padding = (4 - dataString.Length % 4) % 4;
                dataString += new string('=', padding);

                byte[] data = Convert.FromBase64String(dataString);
                if (!dataStore.TryGetValue(ID, out (byte[], byte[]) value))
                {
                    //_logger.LogInformation($"Adding key {ID} to dataStore");
                    dataStore.Add(ID, (null, data));
                    dataStore[ID] = (null, data);
                }
                else
                {
                    dataStore[ID] = (value.Item2, data);
                    //_logger.LogInformation($"Key {ID} already exists in dataStore. Updating value.");
                }

                return StatusCode(201,"Created");
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpGet]
        [Route("{ID}")]
        public IActionResult Diff(string ID)
        {
            try
            {
                if (!dataStore.TryGetValue(ID, out (byte[], byte[]) value))
                    return NotFound(new { error = "ID not found" });

                var leftData = value.Item1;
                var rightData = value.Item2;

                if (leftData == null || rightData == null)
                    return BadRequest(new { error = "Left or right data is missing" });

                if (leftData.Length != rightData.Length)
                    return Ok(new { message = "SizeDoNotMatch" });

                var differences = new List<Difference>();
                int length = 0;
                int offset = -1; // Initialize offset to -1 to handle differences at the beginning of the data

                for (int i = 0; i < leftData.Length; i++)
                {
                    if (leftData[i] != rightData[i])
                    {
                        if (length == 0)
                        {
                            // Start of a new difference
                            offset = i;
                        }
                        length++;
                    }
                    else
                    {
                        if (length > 0)
                        {
                            // End of a difference
                            differences.Add(new Difference { Offset = offset, Length = length });
                            length = 0;
                        }
                        offset = -1; // Reset offset
                    }
                }

                if (length > 0)
                {
                    // If there's a difference at the end of the data
                    differences.Add(new Difference { Offset = offset, Length = length });
                }

                if (differences.Count == 0)
                {
                    // If there are no differences
                    return Ok(new { message = "Equals" });
                }

                return Ok(new { message = "ContentDoNotMatch", differences });
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }
    }
}
