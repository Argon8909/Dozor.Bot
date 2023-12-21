using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bot.BLL;

public class FileDownloader
{
    private readonly HttpClient _httpClient;
    private readonly string _botToken;

    public FileDownloader(string botToken)
    {
        _httpClient = new HttpClient();
        _botToken = botToken;
    }

    public async Task<byte[]> DownloadFileAsync(string fileId)
    {
        try
        {
            var filePath = await GetFilePathAsync(fileId);
            if (filePath == null)
            {
                Console.WriteLine($"Failed to get file path for fileId: {fileId}");
                return null;
            }

            var fileBytes = await _httpClient.GetByteArrayAsync(filePath);
            return fileBytes;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading file: {ex.Message}");
            return null;
        }
    }

    private async Task<string> GetFilePathAsync(string fileId)
    {
        try
        {
            var fileDetails = await GetFileDetailsAsync(fileId);
            if (fileDetails == null || string.IsNullOrEmpty(fileDetails.FilePath))
            {
                Console.WriteLine($"Failed to get file details for fileId: {fileId}");
                return null;
            }

            var filePath = $"https://api.telegram.org/file/bot{_botToken}/{fileDetails.FilePath}";
            return filePath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting file path: {ex.Message}");
            return null;
        }
    }

    private async Task<TelegramFileDetails> GetFileDetailsAsync(string fileId)
    {
        try
        {
            var apiUrl = $"https://api.telegram.org/bot{_botToken}/getFile?file_id={fileId}";
            var response = await _httpClient.GetStringAsync(apiUrl);

            var fileDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<TelegramFileDetails>(response);
            return fileDetails;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting file details: {ex.Message}");
            return null;
        }
    }
}

public class TelegramFileDetails
{
    public string FilePath { get; set; }
    // Другие свойства, если необходимо
}
