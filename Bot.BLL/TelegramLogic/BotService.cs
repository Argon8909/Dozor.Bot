using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bot.Ws;
using Bot.Ws.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Bot.BLL.TelegramLogic
{
    public class BotService : IBotService
    {
        private readonly SemaphoreSlim _updateSemaphore = new(1, 1);
        private readonly string _botToken;
        private readonly HttpClient _httpClient;
        private int _offset = 0;
        private readonly object _eventLock = new();

        public BotService(IOptions<BotSettings> options)
        {
            _botToken = options.Value.Token;
            _httpClient = new HttpClient();
        }

        public async Task InputMessagesHandler(CancellationToken cancellationToken)
        {
            await _updateSemaphore.WaitAsync(cancellationToken);
            try
            {
                var updates = await GetUpdatesAsync(_offset, cancellationToken);
                if (updates != null && updates.Results.Count > 0)
                {
                    _offset = GetNewOffset(updates);
                }
            }
            finally
            {
                _updateSemaphore.Release();
            }
        }

        private int GetNewOffset(MessagesJson updates)
        {
            var offset = 0;
            var args = new InputMessagesEventArgs(updates.Results);
            foreach (var update in updates.Results)
            {
                offset = update.UpdateId + 1;
                OnInputMessages(this, args);
            }
            return offset;
        }

        private async Task<MessagesJson?> GetUpdatesAsync(int offset, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return null;

            try
            {
                var apiUrl = new StringBuilder($"https://api.telegram.org/bot{_botToken}/getUpdates?offset={offset}");
                var response = await _httpClient.GetAsync(apiUrl.ToString(), cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    // Handle non-success status code
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var updates = JsonConvert.DeserializeObject<MessagesJson>(content);
                return updates;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }

        public void SendMessageAsync(string chatId, string text)
        {
            try
            {
                var apiUrl = $"https://api.telegram.org/bot{_botToken}/sendMessage?chat_id={chatId}&text={text}";
                var response = _httpClient.GetStringAsync(apiUrl).Result;
                Console.WriteLine("Response: " + response);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        public ConcurrentQueue<ResultJson> GetMessages()
        {
            return default; //InputMessagesQueue;
        }

        public delegate void InputMessagesDelegate(object sender, InputMessagesEventArgs args);

        public event EventHandler<InputMessagesEventArgs> OnInputMessages;

       
    }

    public class InputMessagesEventArgs : EventArgs
    {
        public List<ResultJson> InputMessagesData { get; }

        public InputMessagesEventArgs(List<ResultJson> data)
        {
            InputMessagesData = data;
        }
    }
}



/*
using System.Collections.Concurrent;
using Bot.Ws;
using Bot.Ws.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bot.BLL.TelegramLogic;
public class BotService : IBotService
{
    private readonly SemaphoreSlim _updateSemaphore = new(1, 1);
    private readonly string _botToken;
    private readonly object _inputMessagesHandlerLock = new object();
    private readonly HttpClient _httpClient;
    private int _offset = 0;
    private List<List<ResultJson>> _inputMessages = new();
    public BotService(IOptions<BotSettings> options)
    {
        _botToken = options.Value.Token;
        _httpClient = new HttpClient();
    }

    public async Task InputMessagesHandler(CancellationToken cancellationToken)
    {
        MessagesJson updates;
        // Используем SemaphoreSlim для синхронизации
        await _updateSemaphore.WaitAsync(cancellationToken);
        try
        {
            // Получаем обновления после захвата SemaphoreSlim
            updates = await GetUpdatesAsync(_offset, cancellationToken);
            var tmpOffset = InputMessagesHandler(updates, cancellationToken);
            if (tmpOffset > _offset)
            {
                _offset = tmpOffset;
               
            }
        }
        finally
        {
            _updateSemaphore.Release();
        }
    }
    
    private int InputMessagesHandler(MessagesJson updates, CancellationToken cancellationToken)
    {
        var offset = 0;
        var args = new InputMessagesEventArgs(updates.Results);
        if (!cancellationToken.IsCancellationRequested && updates.Results.Count > 0)
        {
            foreach (var update in updates.Results)
            {
              
                offset = update.UpdateId + 1;
                OnInputMessages(this,args); 
            }
        }

        return offset;
    }


    private async Task<MessagesJson?> GetUpdatesAsync(int offset, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return null;
        }

        try
        {
            var apiUrl = $"https://api.telegram.org/bot{_botToken}/getUpdates?offset={offset}";
            // Console.WriteLine("apiUrl ===> " + apiUrl);
            var response = await _httpClient.GetAsync(apiUrl, cancellationToken);
            var content = await response.Content.ReadAsStringAsync();
            // Console.WriteLine("response ===> " + content);
            var updates = JsonConvert.DeserializeObject<MessagesJson>(content);
            return updates;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP Request Exception: {ex.Message}");
            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public void SendMessageAsync(string chatId, string text)
    {
        using (var httpClient = new HttpClient())
        {
            var apiUrl = $"https://api.telegram.org/bot{_botToken}/sendMessage?chat_id={chatId}&text={text}";
            // Console.WriteLine("apiUrl ===> " + apiUrl);
            var res = httpClient.GetStringAsync(apiUrl);
            Console.WriteLine("res: " + res.Result.ToString());
        }
    }

    public ConcurrentQueue<ResultJson> GetMessages()
    {
        return default; //InputMessagesQueue;
        // return new Queue<ResultJson>(InputMessagesQueue.Where(msg => msg.Message.Chat.Id == chatId));
    }

    public delegate void InputMessagesDelegate(); 

  
    public event EventHandler<InputMessagesEventArgs> OnInputMessages;
}

public class InputMessagesEventArgs : EventArgs
{
    public List<ResultJson> InputMessagesData { get; }

    public InputMessagesEventArgs(List<ResultJson> data)
    {
        InputMessagesData = data;
    }
}
*/