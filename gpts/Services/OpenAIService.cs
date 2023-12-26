using Azure.AI.OpenAI;
using Azure;
using System.Threading.Tasks;
using System.IO;
using System;

namespace gpts.Services
{
    public class OpenAIService
    {
        OpenAIClient client;
        static readonly char[] trimChars = new char[] { '\n', '?' };

        public void Initialize(string openAIKey, string? openAIEndpoint = null)
        {
            client = !string.IsNullOrWhiteSpace(openAIEndpoint)
                ? new OpenAIClient(
                    new Uri(openAIEndpoint),
                    new AzureKeyCredential(openAIKey))
                : new OpenAIClient(openAIKey);
        }

        internal async Task<string> CallOpenAI(string recommendationType, string location)
        {
            string prompt = GeneratePrompt(recommendationType, location);
            Response<Completions> response = await client.GetCompletionsAsync(
                "text-davinci-003", // 使用中文模型（假設此模型支援中文）
                prompt);
            StringWriter sw = new StringWriter();
            foreach (Choice choice in response.Value.Choices)
            {
                var text = choice.Text.TrimStart(trimChars);
                sw.WriteLine(text);
            }
            var message = sw.ToString();
            return message;
        }

        internal async Task<string> CallOpenAIChat(string recommendationType, string location)
        {
            string prompt = GeneratePrompt(recommendationType, location);
            ChatCompletionsOptions options = new ChatCompletionsOptions();
            options.ChoiceCount = 1;
            options.Messages.Add(new ChatMessage(ChatRole.User, prompt));
            var response = await client.GetChatCompletionsAsync(
                "gpt-3.5-turbo-1106", // 使用中文模型（假設此模型支援中文）
                options);
            StringWriter sw = new StringWriter();
            foreach (ChatChoice choice in response.Value.Choices)
            {
                var text = choice.Message.Content.TrimStart(trimChars);
                sw.WriteLine(text);
            }
            var message = sw.ToString();
            return message;
        }

        private static string GeneratePrompt(string recommendationType, string location)
        {
            return $"請推薦一個在 {location} 附近的 {recommendationType}";
        }
    }
}
