using DiffService.src.models;
using Microsoft.AspNetCore.Mvc;

namespace DiffService.src.services
{
    public class DiffServiceImplementation : IDiffService
    {
        public Dictionary<string, (byte[], byte[])> DataStore { get; set; }

        public DiffServiceImplementation()
        {
            DataStore = new();
        }

        public bool Left(string ID, [FromBody] DiffData diffData)
        {
            try
            {
                string dataString = diffData.Data;
                byte[] data = Convert.FromBase64String(dataString);
                if (!DataStore.TryGetValue(ID, out (byte[], byte[]) value))
                {
                    DataStore.Add(ID, (data, null));
                    DataStore[ID] = (null, data);
                }
                else
                {
                    DataStore[ID] = (value.Item1, data);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Right(string ID, [FromBody] DiffData diffData)
        {
            try
            {
                string dataString = diffData.Data;
                byte[] data = Convert.FromBase64String(dataString);
                if (!DataStore.TryGetValue(ID, out (byte[], byte[]) value))
                {
                    DataStore.Add(ID, (null, data));
                    DataStore[ID] = (null, data);
                }
                else
                {
                    DataStore[ID] = (data, value.Item2);
                }

                return true;
            }
            catch { return false; }
        }

        public DiffResult Diff(string ID)
        {
            var result = new DiffResult();

            if (!DataStore.TryGetValue(ID, out (byte[], byte[]) value))
            {
                result.IsSuccess = false;
                result.MessageType = "Not Found";
                return result;
            }

            var leftData = value.Item1;
            var rightData = value.Item2;

            if (leftData == null || rightData == null)
            {
                result.IsSuccess = false;
                result.MessageType = "Bad Request";
                return result;
            }

            if (leftData.Length != rightData.Length)
            {
                result.IsSuccess = true;
                result.MessageType = "OK";
                result.Message = "SizeDoNotMatch";
                return result;
            }

            result.Differences = new List<Difference>();
            int length = 0;
            int offset = -1; // Initialize offset to -1 to handle diffs at the beginning of the data

            for (int i = 0; i < leftData.Length; i++)
            {
                if (leftData[i] != rightData[i])
                {
                    if (length == 0)
                    {
                        // Start of a new diffs
                        offset = i;
                    }
                    length++;
                }
                else
                {
                    if (length > 0)
                    {
                        // End of a difference
                        result.Differences.Add(new Difference { Offset = offset, Length = length });
                        length = 0;
                    }
                    offset = -1; // Reset offset
                }
            }

            if (length > 0)
            {
                // If there's a difference at the end of the data
                result.Differences.Add(new Difference { Offset = offset, Length = length });
            }

            if (result.Differences.Count == 0)
            {
                // If there are no differences
                result.IsSuccess = true;
                result.Message = "Equals";
                result.MessageType = "OK";

                return result;
            }

            result.IsSuccess = true;
            result.Message = "ContentDoNotMatch";
            result.MessageType = "OK";
            result.Differences = result.Differences;
            return result;
        }
    }
}
